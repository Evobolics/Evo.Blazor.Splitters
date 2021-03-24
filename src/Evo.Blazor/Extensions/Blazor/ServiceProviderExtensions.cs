using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace System
{
    public static class ServiceProviderExtensions
    {
        public static ConcurrentDictionary<RuntimeTypeHandle, Dictionary<RuntimeTypeHandle, MethodInfo>> _Services 
            = new ConcurrentDictionary<RuntimeTypeHandle, Dictionary<RuntimeTypeHandle, MethodInfo>>();

        public static TService GetServiceOrDefaultFrom<TService, TProvider>(this IServiceProvider serviceProvider)
        {
            return GetServiceOrDefaultFrom<TService>(serviceProvider, typeof(TProvider));
        }

        public static TService GetServiceOrDefaultFrom<TService>(this IServiceProvider serviceProvider, System.Type defaultServicesType)
        {
            var serviceType = typeof(TService);

            var service = (TService)serviceProvider.GetService(serviceType);

            if (service != null) return service;
            
            if (!_Services.TryGetValue(defaultServicesType.TypeHandle, out var services))
            {
                services = GetServices(defaultServicesType);

                _Services.AddOrUpdate(defaultServicesType.TypeHandle, services, (key, existing) => existing);
            }
            
            if (!services.TryGetValue(serviceType.TypeHandle, out var factoryMethod))
            {
                throw new Exception($"Could not find a default service of type {serviceType.TypeHandle}");
            }

            service = (TService)factoryMethod.Invoke(null, null);

            return service;
        }

        private static Dictionary<RuntimeTypeHandle, MethodInfo> GetServices(Type defaultServicesType)
        {
            Dictionary<RuntimeTypeHandle, MethodInfo> services = new Dictionary<RuntimeTypeHandle, MethodInfo>();

            var methods = defaultServicesType.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach(var method in methods)
            {
                services.Add(method.ReturnType.TypeHandle, method);
            }

            return services;
        }
    }
}
