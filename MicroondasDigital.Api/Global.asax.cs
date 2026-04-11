using System.Web;
using System.Web.Http;
using MicroondasDigital.Api.InjecaoDependencia;
using MicroondasDigital.Aplicacao.Interfaces;
using MicroondasDigital.Aplicacao.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MicroondasDigital.Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var config = GlobalConfiguration.Configuration;
            WebApiConfig.Register(config);

            var service = new ServiceCollection();

            //Uso de Singleton para manter o estado do microondas entre requisições
            service.AddSingleton<IAquecimentoService, AquecimentoService>();
            service.AddTransient<Controllers.MicroondasDigitalController>();

            var provider = service.BuildServiceProvider();
            config.DependencyResolver = new ResolucaoDependencia(provider);
        }
    }
}   