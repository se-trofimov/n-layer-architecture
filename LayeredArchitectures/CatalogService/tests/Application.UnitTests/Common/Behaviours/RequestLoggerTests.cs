using CatalogService.Application.Common.Behaviours;
using CatalogService.Application.UseCases.Items.Commands;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace CatalogService.Application.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{
    private Mock<ILogger<CreateItemCommand>> _logger = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateItemCommand>>();
    }

 

    [Test]
    public async Task Logger_Should_LogRequest()
    {
        //arrange
        var requestLogger = new LoggingBehaviour<CreateItemCommand>(_logger.Object);
        var nameOfRequest = nameof(CreateItemCommand);
        var request = new CreateItemCommand()
        {
            Name = "Product",
            Price = 1000,
            Amount = 10,
            Description = "Product description",
            CategoryId = 1
        };
        var logMessage = $"Request: {nameOfRequest} {request}";

        //act
        await requestLogger.Process(request, CancellationToken.None);
 
        //assert
        VerifyInformationWasCalled(_logger, logMessage);
    }

    public static void VerifyInformationWasCalled<T>(Mock<ILogger<T>> logger, string expectedMessage)
    {
        Func<object, Type, bool> state = (v, t) => String.Compare(v.ToString(), expectedMessage, StringComparison.Ordinal) == 0;
        
        logger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => state(v, t)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
    }
}
