using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Web.Http.Dependencies;
using System.Web.Http;

namespace MicroondasDigital.Api.InjecaoDependencia
{
    public class ResolucaoDependencia : IDependencyResolver
    {
        private readonly IServiceProvider _serviceProvider;
        public ResolucaoDependencia(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null) return null;

            // Tenta resolver diretamente pelo provider
            var service = _serviceProvider.GetService(serviceType);
            if (service != null) return service;

            // Se for um controlador Web API, cria a instância usando o container para injeção de dependências
            if (typeof(ApiController).IsAssignableFrom(serviceType) || typeof(System.Web.Http.Controllers.IHttpController).IsAssignableFrom(serviceType))
            {
                try
                {
                    return ActivatorUtilities.CreateInstance(_serviceProvider, serviceType);
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return (IEnumerable<object>)_serviceProvider.GetService(typeof(IEnumerable<>).MakeGenericType(serviceType))
                ?? new List<object>();
        }

        public IDependencyScope BeginScope()
        {
            return new EscopoDependencia(_serviceProvider.CreateScope());
        }

        public void Dispose() { }
    }
}
