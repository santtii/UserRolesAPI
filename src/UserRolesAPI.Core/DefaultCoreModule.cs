using Autofac;
using UserRolesAPI.Core.Interfaces;
using UserRolesAPI.Core.Services;

namespace UserRolesAPI.Core;

public class DefaultCoreModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
        .RegisterType<UserService>()
        .As<IUserService>()
        .InstancePerLifetimeScope();

        builder
        .RegisterType<RoleService>()
        .As<IRoleService>()
        .InstancePerLifetimeScope();

        builder
        .RegisterType<PermissionService>()
        .As<IPermissionService>()
        .InstancePerLifetimeScope();
    }
}
