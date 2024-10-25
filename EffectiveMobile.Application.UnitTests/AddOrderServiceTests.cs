using EffectiveMobile.Application.AddOrder;
using EffectiveMobile.Domain;
using EffectiveMobile.Domain.Shared;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace EffectiveMobile.Application.UnitTests;

public class AddOrderServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IValidator<AddOrderRequest>> _validatorMock = new();
    private readonly Mock<ILogger<AddOrderService>> _loggerMock = new();

    [Fact]
    public async Task AddOrderService_Should_Return_Success_When_All_Data_Succeed()
    {
        // Arrange
        var cancellationToken = new CancellationTokenSource().Token;
        var orderWeight = 2.2f;
        var orderDistrict = "Test";
        var orderDeliveryTime = "2024-10-27 10:10:10";

        var orderRequest = new AddOrderRequest(orderWeight, orderDistrict, orderDeliveryTime);

        _orderRepositoryMock.Setup(o => o.AddOrderAsync(It.IsAny<Order>(), cancellationToken))
            .ReturnsAsync(Result.Success);

        _validatorMock
            .Setup(v => v.ValidateAsync(orderRequest, cancellationToken))
            .ReturnsAsync(new ValidationResult());

        var service = new AddOrderService(
            _orderRepositoryMock.Object,
            _validatorMock.Object,
            _loggerMock.Object);

        // Act
        var result = await service.ExecuteAsync(orderRequest, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }
    
    [Fact]
    public async Task AddOrderService_Should_Return_Failure_When_Validation_If_Fail()
    {
        // Arrange
        var cancellationToken = new CancellationTokenSource().Token;
        var orderWeight = 2.2f;
        var orderDistrict = "Test";
        var orderDeliveryTime = "2024-10-27 10:10:10";
        var validationResult = new ValidationResult([
            new ValidationFailure("test", "test||test||Validation")
            {
                ErrorCode = "test"
            }
        ]);

        var orderRequest = new AddOrderRequest(orderWeight, orderDistrict, orderDeliveryTime);

        _orderRepositoryMock.Setup(o => o.AddOrderAsync(It.IsAny<Order>(), cancellationToken))
            .ReturnsAsync(Result.Success);

        _validatorMock
            .Setup(v => v.ValidateAsync(orderRequest, cancellationToken))
            .ReturnsAsync(validationResult);

        var service = new AddOrderService(
            _orderRepositoryMock.Object,
            _validatorMock.Object,
            _loggerMock.Object);

        // Act
        var result = await service.ExecuteAsync(orderRequest, cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.ErrorList);
    }
}