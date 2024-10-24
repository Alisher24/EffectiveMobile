using EffectiveMobile.Domain;
using EffectiveMobile.Domain.Shared;
using EffectiveMobile.Domain.ValueObjects;
using FluentValidation;

namespace EffectiveMobile.Application.AddOrder;

public class AddOrderService(
    IOrderRepository orderRepository,
    IValidator<AddOrderRequest> validator)
{
    public async Task<Result> ExecuteAsync(
        AddOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var order = InitOrder(request);
        
        await orderRepository.AddOrderAsync(order, cancellationToken);

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