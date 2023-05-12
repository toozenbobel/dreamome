using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace UnitTestsCommon;

public static class MockHelper
{
    public static Mock<IServiceProvider> MockServiceProvider()
    {
        var scope = new Mock<IServiceScope>();
        var serviceScopeFactory = new Mock<IServiceScopeFactory>();
        serviceScopeFactory.Setup(x => x.CreateScope()).Returns(scope.Object);

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(serviceScopeFactory.Object);

        scope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);

        return serviceProvider;
    }

    public static Mock<IMediator> MockMediator()
    {
        return new Mock<IMediator>(MockBehavior.Strict);
    }

    public static IMapper MockAutoMapper<T>() where T : Profile, new()
    {
        var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile<T>(); });
        return mappingConfig.CreateMapper();
    }
}