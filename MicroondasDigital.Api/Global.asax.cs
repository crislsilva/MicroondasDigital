using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using MicroondasDigital.Api.InjecaoDependencia;
using MicroondasDigital.Aplicacao.Interfaces;
using MicroondasDigital.Aplicacao.Services;
using MicroondasDigital.Dominio.Interfaces;
using MicroondasDigital.Dominio.Strategies;
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

            //Registra as strategies em uma lista
            var strategies = new IProgramaAquecimento[]
            {
                new PipocaStrategy(),
                new LeiteStrategy(),
                new CarneStrategy(),
                new FrangoStrategy(),
                new FeijaoStrategy()
            };

            service.AddSingleton<IEnumerable<IProgramaAquecimento>>(strategies);
            service.AddSingleton<IProgramaAquecimentoService, ProgramaAquecimentoService>();
            service.AddTransient<Controllers.MicroondasDigitalController>();

            var provider = service.BuildServiceProvider();
            config.DependencyResolver = new ResolucaoDependencia(provider);
        }
    }
}