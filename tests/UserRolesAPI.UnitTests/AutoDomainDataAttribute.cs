using System.Reflection;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using AutoFixture.NUnit3;

namespace UserRolesAPI.UnitTests;

/// <summary>
/// Class to avoid circular references as a result of EntityFramework and virtual members,
/// by overriding the default ISpecimenBuilder.
/// </summary>
public class IgnoreVirtualMembers : ISpecimenBuilder
{
    public Type ReflectedType { get; }

    public object Create(object request, ISpecimenContext context)
    {
        var propertyInfo = request as PropertyInfo;
        if (propertyInfo != null) //// is a property
        {
            if (ReflectedType == null || //// is hosted anywhere
                ReflectedType == propertyInfo.ReflectedType) //// is hosted in defined type
            {
                if (propertyInfo.GetGetMethod().IsVirtual)
                {
                    return new OmitSpecimen();
                }
            }
        }
        return new NoSpecimen();
    }
}

public class AutoDomainDataAttribute : AutoDataAttribute
{
    public AutoDomainDataAttribute() : base(() =>
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        fixture.Customizations.Add(new IgnoreVirtualMembers());
        return fixture;
    })
    {
    }
}
