using System.Reflection;
using AutoMapper;

namespace Jalles.Core.Tests.MappingProfiles;

public class MappingTests
{
    [Fact]
    public void ConfigurationWithAllProfiles_WhenValidated_IsValid()
    {
        var configuration = new MapperConfiguration(configuration => configuration.AddMaps(Assembly.Load("Jalles.Core")));

        configuration.AssertConfigurationIsValid();
    }
}
