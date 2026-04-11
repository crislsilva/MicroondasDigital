using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace MicroondasDigital.Api.InjecaoDependencia
{
    public class EscopoDependencia : IDependencyScope
    {
        private readonly IServiceScope _serviceScope;
        public EscopoDependencia(IServiceScope serviceScope)
        {
            _serviceScope = serviceScope;
        }

        public object GetService(Type serviceType)
        {
            return _serviceScope.ServiceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return (IEnumerable<object>)_serviceScope.ServiceProvider.GetService(typeof(IEnumerable<>).MakeGenericType(serviceType))
                ?? new List<object>();
        }

        public void Dispose()
        {
            _serviceScope.Dispose();
        }
    }
}
