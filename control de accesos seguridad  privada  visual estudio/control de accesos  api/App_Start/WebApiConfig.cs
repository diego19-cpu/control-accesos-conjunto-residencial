using System.Web.Http;
using System.Web.Http.Cors; // 1. Añadir el namespace CORS

// Corregido el namespace a uno con un solo guion bajo.
namespace control_de_accesos_api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // ** INICIO DE LOS CAMBIOS PARA CORS **
            // 2. Crear el objeto de configuración CORS, permitiendo el origen de tu Live Server
            var cors = new EnableCorsAttribute(
                // ¡IMPORTANTE! Las URL de tu frontend (Live Server)
                origins: "http://127.0.0.1:5500, http://localhost:5500",
                headers: "*",
                methods: "*"
            );

            // 3. Habilitar CORS globalmente
            config.EnableCors(cors);
            // ** FIN DE LOS CAMBIOS PARA CORS **

            // Configuración y servicios de Web API

            // Rutas de Web API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}









