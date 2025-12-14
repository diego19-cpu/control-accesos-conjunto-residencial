-- Reemplaza "[NombreDeTuBaseDeDatos]" con el nombre real de tu base de datos
USE [Control_de_accesos];
GO

-- Bloque de código para eliminar y crear tablas
BEGIN TRANSACTION;
BEGIN TRY

    -- Deshabilitar restricciones de verificación para evitar conflictos de clave externa
    EXEC sp_MSforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT ALL";

    -- Eliminar tablas en el orden correcto para evitar problemas de dependencias
    IF OBJECT_ID('Acceso', 'U') IS NOT NULL DROP TABLE Acceso;
    IF OBJECT_ID('Novedades', 'U') IS NOT NULL DROP TABLE Novedades;
    IF OBJECT_ID('Vigilante', 'U') IS NOT NULL DROP TABLE Vigilante;
    IF OBJECT_ID('Personal', 'U') IS NOT NULL DROP TABLE Personal;
    IF OBJECT_ID('Turno', 'U') IS NOT NULL DROP TABLE Turno;
    IF OBJECT_ID('Administrador', 'U') IS NOT NULL DROP TABLE Administrador;
    IF OBJECT_ID('Inmueble', 'U') IS NOT NULL DROP TABLE Inmueble;
    IF OBJECT_ID('Propietario', 'U') IS NOT NULL DROP TABLE Propietario;

    -- Crear la tabla Propietario
    CREATE TABLE Propietario (
        id_propietario INT PRIMARY KEY,
        nombre VARCHAR(100) NOT NULL,
        apellido VARCHAR(100) NOT NULL,
        correo VARCHAR(100) UNIQUE,
        telefono VARCHAR(20)
    );

    -- Crear la tabla Inmueble (Relación 1:N con Propietario)
    CREATE TABLE Inmueble (
        id_inmueble INT PRIMARY KEY,
        numero_torre VARCHAR(10) NOT NULL,
        piso INT NOT NULL,
        apartamento VARCHAR(10) NOT NULL,
        id_propietario INT,
        CONSTRAINT fk_inmueble_propietario FOREIGN KEY (id_propietario) REFERENCES Propietario(id_propietario)
    );
    
    -- Crear la tabla Administrador
    CREATE TABLE Administrador (
        id_administrador INT PRIMARY KEY,
        nombre VARCHAR(100) NOT NULL,
        correo VARCHAR(100) UNIQUE NOT NULL,
        cargo VARCHAR(100)
    );
    
    -- Crear la tabla Turno
    CREATE TABLE Turno (
        id_turno INT PRIMARY KEY,
        asignacion_turno VARCHAR(100) NOT NULL,
        hora_inicio TIME NOT NULL,
        hora_fin TIME NOT NULL
    );
    
    -- Crear la tabla Vigilante
  CREATE TABLE Vigilante (
      id_vigilante INT PRIMARY KEY,
      nombre VARCHAR(100) NOT NULL,
      apellido VARCHAR(100) NOT NULL,
      id_turno INT,
      id_administrador INT,
      CONSTRAINT fk_vigilante_turno FOREIGN KEY (id_turno) REFERENCES Turno(id_turno),
      CONSTRAINT fk_vigilante_administrador FOREIGN KEY (id_administrador) REFERENCES Administrador(id_administrador)
  );
    


    -- Crear la tabla Personal (con relación de 1:N a Inmueble, sin relación a Propietario)
    CREATE TABLE Personal (
        id_personal INT PRIMARY KEY,
        nombre VARCHAR(100) NOT NULL,
        apellido VARCHAR(100) NOT NULL,
        documento_identidad VARCHAR(50) UNIQUE,
        tipo_persona VARCHAR(50) NOT NULL CHECK (tipo_persona IN ('Empleado', 'Visitante', 'Residente')), 
        id_inmueble INT, -- Columna para la relación con Inmueble (1:N)
        CONSTRAINT fk_personal_inmueble FOREIGN KEY (id_inmueble) REFERENCES Inmueble(id_inmueble)
    );

    -- Recrear 'Acceso' con la referencia actualizada a 'Personal'
   
   CREATE TABLE Acceso (
        id_acceso INT PRIMARY KEY,
        id_vigilante INT NOT NULL,
        id_personal INT NOT NULL,
        momento_ingreso DATETIME DEFAULT GETDATE(),
        momento_salida DATETIME,
        numero_visitas INT,
        autorizado_por VARCHAR(100),
        numero_placa_vehiculo VARCHAR(20),
        CONSTRAINT fk_acceso_personal FOREIGN KEY (id_personal) REFERENCES Personal(id_personal),
        CONSTRAINT fk_acceso_vigilante FOREIGN KEY (id_vigilante) REFERENCES Vigilante(id_vigilante)
    );

    -- Recrear 'Novedades' con la referencia actualizada a 'Personal'
    CREATE TABLE Novedades (
        id_novedad INT PRIMARY KEY,
        id_vigilante INT NOT NULL,
        id_administrador INT,
        id_personal INT,
        descripcion VARCHAR(500) NOT NULL,
        momento_novedad DATETIME DEFAULT GETDATE(),
        CONSTRAINT fk_novedades_vigilante FOREIGN KEY (id_vigilante) REFERENCES Vigilante(id_vigilante),
        CONSTRAINT fk_novedades_administrador FOREIGN KEY (id_administrador) REFERENCES Administrador(id_administrador),
        CONSTRAINT fk_novedades_personal FOREIGN KEY (id_personal) REFERENCES Personal(id_personal)
    );
    
    -- Habilitar restricciones de verificación
    EXEC sp_MSforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL";
    
    COMMIT TRANSACTION;
    PRINT 'Las tablas han sido creadas y actualizadas correctamente.';
    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    
    DECLARE @ErrorMessage NVARCHAR(MAX), @ErrorSeverity INT, @ErrorState INT;
    SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
    
    RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH;
GO

---
-- Procedimientos Almacenados
---

-- Procedimientos para la tabla Propietario
IF OBJECT_ID('RegistrarPropietario', 'P') IS NOT NULL DROP PROCEDURE RegistrarPropietario;
GO
CREATE PROCEDURE RegistrarPropietario
    @p_id_propietario INT,
    @p_nombre VARCHAR(100),
    @p_apellido VARCHAR(100),
    @p_correo VARCHAR(100),
    @p_telefono VARCHAR(20)
AS
BEGIN
    INSERT INTO Propietario (id_propietario, nombre, apellido, correo, telefono)
    VALUES (@p_id_propietario, @p_nombre, @p_apellido, @p_correo, @p_telefono);
END;
GO

IF OBJECT_ID('ActualizarPropietario', 'P') IS NOT NULL DROP PROCEDURE ActualizarPropietario;
GO
CREATE PROCEDURE ActualizarPropietario
    @p_id_propietario INT,
    @p_nombre VARCHAR(100),
    @p_apellido VARCHAR(100),
    @p_correo VARCHAR(100),
    @p_telefono VARCHAR(20)
AS
BEGIN
    UPDATE Propietario
    SET
        nombre = @p_nombre,
        apellido = @p_apellido,
        correo = @p_correo,
        telefono = @p_telefono
    WHERE id_propietario = @p_id_propietario;
END;
GO

IF OBJECT_ID('EliminarPropietario', 'P') IS NOT NULL DROP PROCEDURE EliminarPropietario;
GO
CREATE PROCEDURE EliminarPropietario
    @p_id_propietario INT
AS
BEGIN
    DELETE FROM Propietario
    WHERE id_propietario = @p_id_propietario;
END;
GO

IF OBJECT_ID('ConsultarPropietario', 'P') IS NOT NULL DROP PROCEDURE ConsultarPropietario;
GO
CREATE PROCEDURE ConsultarPropietario
    @p_id_propietario INT
AS
BEGIN
    SELECT id_propietario, nombre, apellido, correo, telefono
    FROM Propietario
    WHERE id_propietario = @p_id_propietario;
END;
GO

IF OBJECT_ID('ListarPropietarios', 'P') IS NOT NULL DROP PROCEDURE ListarPropietarios;
GO
CREATE PROCEDURE ListarPropietarios
AS
BEGIN
    SELECT id_propietario, nombre, apellido, correo, telefono
    FROM Propietario;
END;
GO

-- Procedimientos para la tabla Inmueble
IF OBJECT_ID('RegistrarInmueble', 'P') IS NOT NULL DROP PROCEDURE RegistrarInmueble;
GO
CREATE PROCEDURE RegistrarInmueble
    @p_id_inmueble INT,
    @p_numero_torre VARCHAR(10),
    @p_piso INT,
    @p_apartamento VARCHAR(10),
    @p_id_propietario INT
AS
BEGIN
    INSERT INTO Inmueble (id_inmueble, numero_torre, piso, apartamento, id_propietario)
    VALUES (@p_id_inmueble, @p_numero_torre, @p_piso, @p_apartamento, @p_id_propietario);
END;
GO

IF OBJECT_ID('ActualizarInmueble', 'P') IS NOT NULL DROP PROCEDURE ActualizarInmueble;
GO
CREATE PROCEDURE ActualizarInmueble
    @p_id_inmueble INT,
    @p_numero_torre VARCHAR(10),
    @p_piso INT,
    @p_apartamento VARCHAR(10),
    @p_id_propietario INT
AS
BEGIN
    UPDATE Inmueble
    SET
        numero_torre = @p_numero_torre,
        piso = @p_piso,
        apartamento = @p_apartamento,
        id_propietario = @p_id_propietario
    WHERE id_inmueble = @p_id_inmueble;
END;
GO

IF OBJECT_ID('EliminarInmueble', 'P') IS NOT NULL DROP PROCEDURE EliminarInmueble;
GO
CREATE PROCEDURE EliminarInmueble
    @p_id_inmueble INT
AS
BEGIN
    DELETE FROM Inmueble
    WHERE id_inmueble = @p_id_inmueble;
END;
GO

IF OBJECT_ID('ConsultarInmueble', 'P') IS NOT NULL DROP PROCEDURE ConsultarInmueble;
GO
CREATE PROCEDURE ConsultarInmueble
    @p_id_inmueble INT
AS
BEGIN
    SELECT id_inmueble, numero_torre, piso, apartamento, id_propietario
    FROM Inmueble
    WHERE id_inmueble = @p_id_inmueble;
END;
GO

IF OBJECT_ID('ListarInmuebles', 'P') IS NOT NULL
    DROP PROCEDURE ListarInmuebles;
GO

CREATE PROCEDURE ListarInmuebles
AS
BEGIN
    SELECT
        I.id_inmueble,
        I.numero_torre,
        I.piso,
        I.apartamento,
        I.id_propietario, -- Coluna adicionada para corrigir o erro
        P.nombre + ' ' + P.apellido AS Propietario
    FROM Inmueble AS I
    LEFT JOIN Propietario AS P ON I.id_propietario = P.id_propietario;
END;
GO

-- Procedimientos para la tabla Administrador
IF OBJECT_ID('RegistrarAdministrador', 'P') IS NOT NULL DROP PROCEDURE RegistrarAdministrador;
GO
CREATE PROCEDURE RegistrarAdministrador
    @p_id_administrador INT,
    @p_nombre VARCHAR(100),
    @p_correo VARCHAR(100),
    @p_cargo VARCHAR(100)
AS
BEGIN
    INSERT INTO Administrador (id_administrador, nombre, correo, cargo)
    VALUES (@p_id_administrador, @p_nombre, @p_correo, @p_cargo);
END;
GO

IF OBJECT_ID('ActualizarAdministrador', 'P') IS NOT NULL DROP PROCEDURE ActualizarAdministrador;
GO
CREATE PROCEDURE ActualizarAdministrador
    @p_id_administrador INT,
    @p_nombre VARCHAR(100),
    @p_correo VARCHAR(100),
    @p_cargo VARCHAR(100)
AS
BEGIN
    UPDATE Administrador
    SET
        nombre = @p_nombre,
        correo = @p_correo,
        cargo = @p_cargo
    WHERE id_administrador = @p_id_administrador;
END;
GO

IF OBJECT_ID('EliminarAdministrador', 'P') IS NOT NULL DROP PROCEDURE EliminarAdministrador;
GO
CREATE PROCEDURE EliminarAdministrador
    @p_id_administrador INT
AS
BEGIN
    DELETE FROM Administrador
    WHERE id_administrador = @p_id_administrador;
END;
GO

IF OBJECT_ID('ConsultarAdministrador', 'P') IS NOT NULL DROP PROCEDURE ConsultarAdministrador;
GO
CREATE PROCEDURE ConsultarAdministrador
    @p_id_administrador INT
AS
BEGIN
    SELECT id_administrador, nombre, correo, cargo
    FROM Administrador
    WHERE id_administrador = @p_id_administrador;
END;
GO

IF OBJECT_ID('ListarAdministradores', 'P') IS NOT NULL DROP PROCEDURE ListarAdministradores;
GO
CREATE PROCEDURE ListarAdministradores
AS
BEGIN
    SELECT id_administrador, nombre, correo, cargo
    FROM Administrador;
END;
GO

-- Procedimientos para la tabla Turno
IF OBJECT_ID('RegistrarTurno', 'P') IS NOT NULL DROP PROCEDURE RegistrarTurno;
GO
CREATE PROCEDURE RegistrarTurno
    @p_id_turno INT,
    @p_asignacion_turno VARCHAR(100),
    @p_hora_inicio TIME,
    @p_hora_fin TIME
AS
BEGIN
    INSERT INTO Turno (id_turno, asignacion_turno, hora_inicio, hora_fin)
    VALUES (@p_id_turno, @p_asignacion_turno, @p_hora_inicio, @p_hora_fin);
END;
GO

IF OBJECT_ID('ActualizarTurno', 'P') IS NOT NULL DROP PROCEDURE ActualizarTurno;
GO
CREATE PROCEDURE ActualizarTurno
    @p_id_turno INT,
    @p_asignacion_turno VARCHAR(100),
    @p_hora_inicio TIME,
    @p_hora_fin TIME
AS
BEGIN
    UPDATE Turno
    SET
        asignacion_turno = @p_asignacion_turno,
        hora_inicio = @p_hora_inicio,
        hora_fin = @p_hora_fin
    WHERE id_turno = @p_id_turno;
END;
GO

IF OBJECT_ID('EliminarTurno', 'P') IS NOT NULL DROP PROCEDURE EliminarTurno;
GO
CREATE PROCEDURE EliminarTurno
    @p_id_turno INT
AS
BEGIN
    DELETE FROM Turno
    WHERE id_turno = @p_id_turno;
END;
GO

IF OBJECT_ID('ConsultarTurno', 'P') IS NOT NULL DROP PROCEDURE ConsultarTurno;
GO
CREATE PROCEDURE ConsultarTurno
    @p_id_turno INT
AS
BEGIN
    SELECT id_turno, asignacion_turno, hora_inicio, hora_fin
    FROM Turno
    WHERE id_turno = @p_id_turno;
END;
GO

IF OBJECT_ID('ListarTurnos', 'P') IS NOT NULL DROP PROCEDURE ListarTurnos;
GO
CREATE PROCEDURE ListarTurnos
AS
BEGIN
    SELECT id_turno, asignacion_turno, hora_inicio, hora_fin
    FROM Turno;
END;
GO

-- Procedimientos para la tabla Vigilante
IF OBJECT_ID('RegistrarVigilante', 'P') IS NOT NULL DROP PROCEDURE RegistrarVigilante;
GO
CREATE PROCEDURE RegistrarVigilante
    @p_id_vigilante INT,
    @p_nombre VARCHAR(100),
    @p_apellido VARCHAR(100),
    @p_id_turno INT,
    @p_id_administrador INT
AS
BEGIN
    INSERT INTO Vigilante (id_vigilante, nombre, apellido, id_turno, id_administrador)
    VALUES (@p_id_vigilante, @p_nombre, @p_apellido, @p_id_turno, @p_id_administrador);
END;
GO


IF OBJECT_ID('ActualizarVigilante', 'P') IS NOT NULL DROP PROCEDURE ActualizarVigilante;
GO
CREATE PROCEDURE ActualizarVigilante
    @p_id_vigilante INT,
    @p_nombre VARCHAR(100),
    @p_apellido VARCHAR(100),
    @p_id_turno INT,
    @p_id_administrador INT
AS
BEGIN
    UPDATE Vigilante
    SET
        nombre = @p_nombre,
        apellido = @p_apellido,
        id_turno = @p_id_turno,
        id_administrador = @p_id_administrador
    WHERE id_vigilante = @p_id_vigilante;
END;
GO

IF OBJECT_ID('EliminarVigilante', 'P') IS NOT NULL DROP PROCEDURE EliminarVigilante;
GO
CREATE PROCEDURE EliminarVigilante
    @p_id_vigilante INT
AS
BEGIN
    DELETE FROM Vigilante
    WHERE id_vigilante = @p_id_vigilante;
END;
GO

IF OBJECT_ID('ConsultarVigilante', 'P') IS NOT NULL DROP PROCEDURE ConsultarVigilante;
GO
CREATE PROCEDURE ConsultarVigilante
    @p_id_vigilante INT
AS
BEGIN
    SELECT id_vigilante, nombre, apellido, id_turno, id_administrador
    FROM Vigilante
    WHERE id_vigilante = @p_id_vigilante;
END;
GO

IF OBJECT_ID('ListarVigilantes', 'P') IS NOT NULL DROP PROCEDURE ListarVigilantes;
GO
CREATE PROCEDURE ListarVigilantes
AS
BEGIN
    SELECT 
        V.id_vigilante, 
        V.nombre, 
        V.apellido, 
        V.id_turno, -- Adicione esta linha
        V.id_administrador, -- Adicione esta linha
        T.asignacion_turno AS Turno, 
        A.nombre AS Administrador
    FROM Vigilante V
    LEFT JOIN Turno T ON V.id_turno = T.id_turno
    LEFT JOIN Administrador A ON V.id_administrador = A.id_administrador;
END;
GO

-- Procedimientos para la tabla Personal (actualizados)
IF OBJECT_ID('RegistrarPersonal', 'P') IS NOT NULL DROP PROCEDURE RegistrarPersonal;
GO
CREATE PROCEDURE RegistrarPersonal
    @p_id_personal INT,
    @p_nombre VARCHAR(100),
    @p_apellido VARCHAR(100),
    @p_documento_identidad VARCHAR(50),
    @p_tipo_persona VARCHAR(50),
    @p_id_inmueble INT = NULL
AS
BEGIN
    INSERT INTO Personal (id_personal, nombre, apellido, documento_identidad, tipo_persona, id_inmueble)
    VALUES (@p_id_personal, @p_nombre, @p_apellido, @p_documento_identidad, @p_tipo_persona, @p_id_inmueble);
END;
GO

IF OBJECT_ID('ActualizarPersonal', 'P') IS NOT NULL DROP PROCEDURE ActualizarPersonal;
GO
CREATE PROCEDURE ActualizarPersonal
    @p_id_personal INT,
    @p_nombre VARCHAR(100),
    @p_apellido VARCHAR(100),
    @p_documento_identidad VARCHAR(50),
    @p_tipo_persona VARCHAR(50),
    @p_id_inmueble INT = NULL
AS
BEGIN
    UPDATE Personal
    SET
        nombre = @p_nombre,
        apellido = @p_apellido,
        documento_identidad = @p_documento_identidad,
        tipo_persona = @p_tipo_persona,
        id_inmueble = @p_id_inmueble
    WHERE id_personal = @p_id_personal;
END;
GO

IF OBJECT_ID('EliminarPersonal', 'P') IS NOT NULL DROP PROCEDURE EliminarPersonal;
GO
CREATE PROCEDURE EliminarPersonal
    @p_id_personal INT
AS
BEGIN
    DELETE FROM Personal
    WHERE id_personal = @p_id_personal;
END;
GO

IF OBJECT_ID('ConsultarPersonal', 'P') IS NOT NULL DROP PROCEDURE ConsultarPersonal;
GO
CREATE PROCEDURE ConsultarPersonal
    @p_id_personal INT
AS
BEGIN
    SELECT id_personal, nombre, apellido, documento_identidad, tipo_persona, id_inmueble
    FROM Personal
    WHERE id_personal = @p_id_personal;
END;
GO

IF OBJECT_ID('ListarPersonal', 'P') IS NOT NULL DROP PROCEDURE ListarPersonal;
GO
CREATE PROCEDURE ListarPersonal
AS
BEGIN
    SELECT
        P.id_personal,
        P.nombre,
        P.apellido,
        P.documento_identidad,
        P.tipo_persona,
        I.numero_torre,
        I.piso,
        I.apartamento,
        P.id_inmueble
    FROM Personal P
    LEFT JOIN Inmueble I ON P.id_inmueble = I.id_inmueble;
END;
GO

-- Procedimientos para la tabla Acceso
IF OBJECT_ID('RegistrarAcceso', 'P') IS NOT NULL DROP PROCEDURE RegistrarAcceso;
GO
CREATE PROCEDURE RegistrarAcceso
    @p_id_acceso INT,
    @p_id_vigilante INT,
    @p_id_personal INT,
   @p_momento_ingreso varchar(100),
    @p_momento_salida varchar(100),
    @p_numero_visitas INT,
    @p_autorizado_por VARCHAR(100),
    @p_numero_placa_vehiculo VARCHAR(20) = NULL
AS
BEGIN
    INSERT INTO Acceso (id_acceso, id_vigilante, id_personal, momento_ingreso,momento_salida,numero_visitas, autorizado_por, numero_placa_vehiculo)
    VALUES (@p_id_acceso, @p_id_vigilante, @p_id_personal,convert(Datetime,@p_momento_ingreso),convert(Datetime,@p_momento_salida), @p_numero_visitas, @p_autorizado_por, @p_numero_placa_vehiculo);
END;
GO

IF OBJECT_ID('ActualizarAcceso', 'P') IS NOT NULL DROP PROCEDURE ActualizarAcceso;
GO
CREATE PROCEDURE ActualizarAcceso
    @p_id_acceso INT,
    @p_id_vigilante INT,
    @p_id_personal INT,
    @p_momento_ingreso DATETIME,
    @p_momento_salida DATETIME = NULL,
    @p_numero_visitas INT = NULL,
    @p_autorizado_por VARCHAR(MAX) = NULL,
    @p_numero_placa_vehiculo VARCHAR(MAX) = NULL
AS
BEGIN
    UPDATE Acceso
    SET
        id_vigilante = @p_id_vigilante,
        id_personal = @p_id_personal,
        momento_ingreso = @p_momento_ingreso,
        momento_salida = @p_momento_salida,
        numero_visitas = @p_numero_visitas,
        autorizado_por = @p_autorizado_por,
        numero_placa_vehiculo = @p_numero_placa_vehiculo
    WHERE id_acceso = @p_id_acceso;
END;
GO

IF OBJECT_ID('EliminarAcceso', 'P') IS NOT NULL DROP PROCEDURE EliminarAcceso;
GO
CREATE PROCEDURE EliminarAcceso
    @p_id_acceso INT
AS
BEGIN
    DELETE FROM Acceso
    WHERE id_acceso = @p_id_acceso;
END;
GO

IF OBJECT_ID('ConsultarAcceso', 'P') IS NOT NULL DROP PROCEDURE ConsultarAcceso;
GO
CREATE PROCEDURE ConsultarAcceso
    @p_id_acceso INT
AS
BEGIN
    SELECT id_acceso, id_vigilante, id_personal, momento_ingreso, momento_salida, numero_visitas, autorizado_por, numero_placa_vehiculo
    FROM Acceso
    WHERE id_acceso = @p_id_acceso;
END;
GO

IF OBJECT_ID('ListarAccesos', 'P') IS NOT NULL DROP PROCEDURE ListarAccesos;
GO
CREATE PROCEDURE ListarAccesos
AS
BEGIN
    SELECT
        A.id_acceso,
        A.id_vigilante, -- <-- Agregar esta línea
        A.id_personal,  -- <-- Agregar esta línea
        V.nombre AS Vigilante_Nombre,
        V.apellido AS Vigilante_Apellido,
        P.nombre AS Personal_Nombre,
        P.apellido AS Personal_Apellido,
        P.tipo_persona,
        A.momento_ingreso,
        A.momento_salida,
        A.numero_visitas,
        A.autorizado_por,
        A.numero_placa_vehiculo
    FROM Acceso A
    LEFT JOIN Vigilante V ON A.id_vigilante = V.id_vigilante
    LEFT JOIN Personal P ON A.id_personal = P.id_personal;
END;
GO

-- Procedimientos para la tabla Novedades
IF OBJECT_ID('RegistrarNovedad', 'P') IS NOT NULL DROP PROCEDURE RegistrarNovedad;
GO
CREATE PROCEDURE RegistrarNovedad
    @p_id_novedad INT,
    @p_id_vigilante INT,
    @p_id_administrador INT = NULL,
    @p_id_personal INT = NULL,
    @p_descripcion VARCHAR(500), -- ¡Aquí estaba la coma que faltaba!
    @p_momento_novedad DATETIME
AS
BEGIN
    INSERT INTO Novedades (id_novedad, id_vigilante, id_administrador, id_personal, descripcion, momento_novedad)
    VALUES (@p_id_novedad, @p_id_vigilante, @p_id_administrador, @p_id_personal, @p_descripcion, @p_momento_novedad);
END;
GO
IF OBJECT_ID('ActualizarNovedad', 'P') IS NOT NULL DROP PROCEDURE ActualizarNovedad;
GO
CREATE PROCEDURE ActualizarNovedad
    @p_id_novedad INT,
    @p_id_vigilante INT,
    @p_id_administrador INT = NULL,
    @p_id_personal INT = NULL,
    @p_descripcion VARCHAR(500)
AS
BEGIN
    UPDATE Novedades
    SET
        id_vigilante = @p_id_vigilante,
        id_administrador = @p_id_administrador,
        id_personal = @p_id_personal,
        descripcion = @p_descripcion,
        momento_novedad = GETDATE()
    WHERE id_novedad = @p_id_novedad;
END;
GO

IF OBJECT_ID('EliminarNovedad', 'P') IS NOT NULL DROP PROCEDURE EliminarNovedad;
GO
CREATE PROCEDURE EliminarNovedad
    @p_id_novedad INT
AS
BEGIN
    DELETE FROM Novedades
    WHERE id_novedad = @p_id_novedad;
END;
GO

IF OBJECT_ID('ConsultarNovedad', 'P') IS NOT NULL DROP PROCEDURE ConsultarNovedad;
GO
CREATE PROCEDURE ConsultarNovedad
    @p_id_novedad INT
AS
BEGIN
    SELECT id_novedad, id_vigilante, id_administrador, id_personal, descripcion, momento_novedad
    FROM Novedades
    WHERE id_novedad = @p_id_novedad;
END;
GO

IF OBJECT_ID('ListarNovedades', 'P') IS NOT NULL
    DROP PROCEDURE ListarNovedades;
GO

CREATE PROCEDURE ListarNovedades
AS
BEGIN
    SELECT
        N.id_novedad,
        N.id_vigilante,
        N.id_administrador,
        N.id_personal,
        V.nombre + ' ' + V.apellido AS Vigilante,
        A.nombre AS Administrador,
        P.nombre + ' ' + P.apellido AS Personal,
        N.descripcion,
        N.momento_novedad
    FROM Novedades AS N
    LEFT JOIN Vigilante AS V ON N.id_vigilante = V.id_vigilante
    LEFT JOIN Administrador AS A ON N.id_administrador = A.id_administrador
    LEFT JOIN Personal AS P ON N.id_personal = P.id_personal;
END;
GO