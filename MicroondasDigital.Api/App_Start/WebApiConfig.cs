using System.Web.Http;
using System.Web.Http.Cors;

namespace MicroondasDigital.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Deixei tudo liberado somente porque é um projeto de teste, em produção deve ser configurado para aceitar somente os domínios necessários
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            config.MapHttpAttributeRoutes();
            config.EnsureInitialized();

        }
    }
}
