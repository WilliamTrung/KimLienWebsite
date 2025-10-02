using Common.Api.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Common.Api
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Register Module controllers to WebHost
        /// </summary>
        /// <param name="services"></param>
        public static void AddModuleControllers(this IServiceCollection services, List<Assembly> moduelAssemblies)
        {
            // Add mvc
            var mvc = services.AddControllers();

            foreach (var asm in moduelAssemblies)
            {
                // Register controllers from assembly
                mvc.AddApplicationPart(asm);
            }

            mvc.AddControllersAsServices();

            // Turn off auto-400 on invalid ModelState
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        /// <summary>
        /// Add modules for DI
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddModuleDI(this IServiceCollection services, IConfiguration configuration, List<Assembly> moduelAssemblies)
        {
            // Add modules for DI
            AddModules(services, configuration, moduelAssemblies);
        }
        /// <summary>
        /// Register common services
        /// </summary>
        /// <param name="services"></param>
        public static void AddCommonDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMediator, Mediator>();
        }

        /// <summary>
        /// Add modules to WebHost
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="moduleAssemblies"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static IReadOnlyCollection<IModule> AddModules(
            IServiceCollection services,
            IConfiguration configuration,
            List<Assembly> moduleAssemblies)
        {
            if (moduleAssemblies is null || moduleAssemblies.Count == 0)
                throw new ArgumentException("Provide at least one module assembly.", nameof(moduleAssemblies));

            // Discover concrete IModule types in provided assemblies
            var modules = moduleAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                .Select(t => (IModule)Activator.CreateInstance(t.AsType())!)
                .ToList();

            // Let each module configure its own services (each module uses Scrutor inside)
            foreach (var m in modules)
            {
                m.ConfigureServices(services, configuration);
            }

            services.AddSingleton<IReadOnlyCollection<IModule>>(modules);
            return modules;
        }
    }
}
