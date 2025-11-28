using System.Reflection;
using Common.Kernel.Dependencies;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

public static class ServiceRegistrationExtension
{
    // exclude the marker interfaces from being registered as service types
    private static readonly Type[] MarkerInterfaces =
    [
        typeof(IScoped), typeof(ITransient), typeof(ISingleton)
    ];

    /// Returns only the interfaces directly implemented by 'type'
    /// (excludes interfaces coming from base classes and those inherited via other interfaces).
    private static IEnumerable<Type> GetDirectInterfaces(Type type)
    {
        var all = type.GetInterfaces();

        // Interfaces coming from the base class
        var fromBase = type.BaseType is null ? Type.EmptyTypes : type.BaseType.GetInterfaces();

        // Interfaces inherited via other interfaces implemented by this type
        var viaOtherIfaces = new HashSet<Type>();
        foreach (var i in all)
        {
            foreach (var ii in i.GetInterfaces())
                viaOtherIfaces.Add(ii);
        }

        // Direct = all - fromBase - viaOtherIfaces
        var result = all.Except(fromBase).Except(viaOtherIfaces);

        // With generic type return generic type definition
        // For example: IGenericType<T> -> IGenericType<>
        result = result.Select(i => i.IsGenericType ? i.GetGenericTypeDefinition() : i);

        return result;
    }

    private static IEnumerable<Type> ServiceTypesSelector(Type impl) => GetDirectInterfaces(impl).Where(i => !MarkerInterfaces.Contains(i));

    /// <summary>
    /// Scan the given assemblies, register classes implementing
    /// IScoped/ITransient/ISingleton with corresponding lifetimes.
    /// </summary>
    public static IServiceCollection AddMarkedServices(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        if (assemblies == null || assemblies.Length == 0)
            throw new ArgumentException("Provide at least one assembly to scan.", nameof(assemblies));

        services.Scan(scan => scan
            .FromAssemblies(assemblies)

                // SCOPED
                .AddClasses(c => c.AssignableTo<IScoped>()
                                  .Where(t => !t.Name.EndsWith("Decorator")), publicOnly: false)
                                  .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                                  .As(ServiceTypesSelector)
                                  .AsSelf()
                                  .WithScopedLifetime()

                // TRANSIENT
                .AddClasses(c => c.AssignableTo<ITransient>()
                                  .Where(t => !t.Name.EndsWith("Decorator")), publicOnly: false)
                                  .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                                  .As(ServiceTypesSelector)
                                  .AsSelf()
                                  .WithTransientLifetime()

                // SINGLETON
                .AddClasses(c => c.AssignableTo<ISingleton>()
                                  .Where(t => !t.Name.EndsWith("Decorator")), publicOnly: false)
                                  .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                                  .As(ServiceTypesSelector)
                                  .AsSelf()
                                  .WithSingletonLifetime()
        );

        return services;
    }

    /// <summary>
    /// Convenience overload: pass any marker type from an assembly.
    /// </summary>
    public static IServiceCollection AddMarkedServicesFrom<TMarker>(
        this IServiceCollection services) =>
        services.AddMarkedServices(typeof(TMarker).Assembly);
}