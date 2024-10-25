using EffectiveMobile.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EffectiveMobile.Application.FilteringOrdersByDistrict;

public class FilteringOrdersByDistrictService(
    IOrderRepository orderRepository,
    IValidator<FilteringOrdersByDistrictRequest> validator,
    ILogger<FilteringOrdersByDistrictService> logger)
{
    /// <summary>
    /// Filtering orders for delivery to a specific area of the city in the next half hour after the time of the first order
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<OrderDto>>> ExecuteAsync(
        FilteringOrdersByDistrictRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var firstDeliveryTime = DateTime.Parse(request.FirstDeliveryTime);

        var result = await orderRepository
            .FilteringOrdersByDistrict(request.District, firstDeliveryTime, cancellationToken);
        if (result.IsFailure)
            return result.ErrorList;

        logger.LogInformation("Filtering orders by district: {district} with {deliveryTime}",
            request.District,
            request.FirstDeliveryTime);

        return result;
    }
}