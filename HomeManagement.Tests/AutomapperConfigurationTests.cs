using AutoMapper;
using HomeManagement.Application;
using HomeManagement.Infrastructure;
using Microsoft.Extensions.Logging;

namespace HomeManagement.Tests
{
    public class AutomapperConfigurationTests
    {
        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            var loggerFactory = LoggerFactory.Create(builder => { });
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(ApplicationAssemblyMarker).Assembly);
                cfg.AddMaps(typeof(InfrastructureAssemblyMarker).Assembly);
            }, loggerFactory);

            config.AssertConfigurationIsValid();
        }
    }
}
