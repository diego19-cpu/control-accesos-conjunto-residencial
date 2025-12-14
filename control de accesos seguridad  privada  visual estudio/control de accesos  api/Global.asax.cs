using System;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using control_de_accesos_api; // Esta línea es la que faltaba

namespace control_de_accesos__api
{
    public class WebApiApplication : HttpApplication
    {
        // Este método se ejecuta cuando la aplicación se inicia
        protected void Application_Start()
        {
            // Configuración de Web API (para enrutamiento, filtros, etc.)
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Aquí puedes agregar otras configuraciones si las necesitas
        }
    }
}
