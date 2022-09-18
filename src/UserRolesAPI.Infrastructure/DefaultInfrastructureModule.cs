using System.Reflection;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using UserRolesAPI.Core.Constant;
using UserRolesAPI.Core.Interfaces.Settings;
using UserRolesAPI.Infrastructure.Data.Repositories;
using UserRolesAPI.Infrastructure.Settings;
using Module = Autofac.Module;

namespace UserRolesAPI.Infrastructure;

public class DefaultInfrastructureModule : Module
{
    private readonly bool _isDevelopment = false;
    private readonly ConfigurationManager _configuration;
    private readonly List<Assembly> _assemblies = new List<Assembly>();

    public DefaultInfrastructureModule(bool isDevelopment, ConfigurationManager configuration, Assembly? callingAssembly = null)
    {
        _isDevelopment = isDevelopment;
        _configuration = configuration;
        var infrastructureAssembly = Assembly.GetAssembly(typeof(StartupSetup));

        if (infrastructureAssembly != null)
        {
            _assemblies.Add(infrastructureAssembly);
        }
        if (callingAssembly != null)
        {
            _assemblies.Add(callingAssembly);
        }
    }

    protected override void Load(ContainerBuilder builder)
    {
        if (_isDevelopment)
        {
            RegisterDevelopmentOnlyDependencies(builder);
        }
        else
        {
            RegisterProductionOnlyDependencies(builder);
        }
        RegisterCommonDependencies(builder);
    }

    private void RegisterCommonDependencies(ContainerBuilder builder)
    {
        var appSettings = _configuration.BindSettings<AppSettings>(SettingsConstants.AppSettings);

        builder.RegisterType<Repository>()
        .AsSelf()
        .AsImplementedInterfaces()
          .InstancePerLifetimeScope();

        builder
            .RegisterType<Mediator>()
            .As<IMediator>()
            .InstancePerLifetimeScope();

        builder.RegisterInstance<IAppSettings>(appSettings)
          .SingleInstance();

        builder.Register<ServiceFactory>(context =>
        {
            var c = context.Resolve<IComponentContext>();
            return t => c.Resolve(t);
        });

        var mediatrOpenTypes = new[]
        {
                typeof(IRequestHandler<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>),
                typeof(INotificationHandler<>),
            };

        foreach (var mediatrOpenType in mediatrOpenTypes)
        {
            builder
            .RegisterAssemblyTypes(_assemblies.ToArray())
            .AsClosedTypesOf(mediatrOpenType)
            .AsImplementedInterfaces();
        }
    }

    private void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
    {

    }

    private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
    {

    }
}
