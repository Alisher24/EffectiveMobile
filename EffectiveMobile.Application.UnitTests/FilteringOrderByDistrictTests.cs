using EffectiveMobile.Application.FilteringOrdersByDistrict;
using EffectiveMobile.Domain.Shared;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace EffectiveMobile.Application.UnitTests;

public class FilteringOrderByDistrictTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IValidator<FilteringOrdersByDistrictRequest>> _validatorMock = new();
    private readonly Mock<ILogger<FilteringOrdersByDistrictService>> _loggerMock = new();

    [Fact]
    public async Task FilteringOrderByDistrictService_Should_Return_Success_When_All_Data_Succeed()
    {
        // Arrange
        var cancellationToken = new CancellationTokenSource().Token;
        var district = "Test";
        var firstDeliveryTime = "2024-10-27 10:10:10";

        var request = new FilteringOrdersByDistrictRequest(district, firstDeliveryTime);

        _orderRepositoryMock.Setup(o =>
                o.FilteringOrdersByDistrict(district, DateTime.Parse(firstDeliveryTime), cancellationToken))
            .ReturnsAsync(Result<List<OrderDto>>.Success([]));

        _validatorMock
            .Setup(v => v.ValidateAsync(request, cancellationToken))
            .ReturnsAsync(new ValidationResult());
        
        var service = new FilteringOrdersByDistrictService(
            _orderRepositoryMock.Object,
            _validatorMock.Object,
            _loggerMock.Object);

        // Act
        var result = await service.ExecuteAsync(request, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Empty(result.Value);
    }
    
    [Fact]
    public async Task FilteringOrderByDistrictService_Should_Return_Failure_When_Validation_If_Fail()
    {
        // Arrange
        var cancellationToken = new CancellationTokenSource().Token;
        var district = "Test";
        var firstDeliveryTime = "2024-10-27 10:10:10";
        var validationResult = new ValidationResult([
            new ValidationFailure("test", "test||test||Validation")
            {
                ErrorCode = "test"
            }
        ]);

        var request = new FilteringOrdersByDistrictRequest(district, firstDeliveryTime);

        _orderRepositoryMock.Setup(o =>
                o.FilteringOrdersByDistrict(district, DateTime.Parse(firstDeliveryTime), cancellationToken))
            .ReturnsAsync(Result<List<OrderDto>>.Success([]));

        _validatorMock
            .Setup(v => v.ValidateAsync(request, cancellationToken))
            .ReturnsAsync(validationResult);
        
        var service = new FilteringOrdersByDistrictService(
            _orderRepositoryMock.Object,
            _validatorMock.Object,
            _loggerMock.Object);

        // Act
        var result = await service.ExecuteAsync(request, cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.ErrorList);
    }
}