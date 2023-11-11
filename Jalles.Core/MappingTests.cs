using System.Reflection;
using AutoMapper;
using Xunit;

namespace Jalles.Core;

public class MappingTests
{
    [Fact]
    public void All_Profiles_Integration_Validation()
    {
        var configuration = new MapperConfiguration(configuration => configuration.AddMaps(Assembly.Load("Jalles.Core")));

        configuration.AssertConfigurationIsValid();
    }
}