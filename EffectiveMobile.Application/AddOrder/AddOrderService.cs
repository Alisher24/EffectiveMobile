using EffectiveMobile.Domain;
using EffectiveMobile.Domain.Shared;
using EffectiveMobile.Domain.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EffectiveMobile.Application.AddOrder;

public class AddOrderService(
    IOrderRepository orderRepository,
    IValidator<AddOrderRequest> validator,
    ILogger<AddOrderService> logger)
{
    public async Task<Result> ExecuteAsync(
        AddOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var order = InitOrder(request);
        
        var result = await orderRepository.AddOrderAsync(order, cancellationToken);
        if (result.IsFailure)
            return result.ErrorList;
        
        logger.LogInformation("Added order with id {id}", order.Id);
        
        return Result.Success();
    }

    private Order InitOrder(AddOrderRequest request)
    {
        var orderId = Guid.NewGuid();
        var orderWeight = Weight.Create(request.Weight).Value;
        var orderDistrict = District.Create(request.District).Value;
        var orderDeliveryTime = DeliveryTime.Create(request.DeliveryTime).Value;

        return new Order(orderId, orderWeight, orderDistrict, orderDeliveryTime);
    }
}