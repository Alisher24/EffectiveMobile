using EffectiveMobile.Domain.Shared;

namespace EffectiveMobile.Application.FilteringOrdersByDistrict;

public class FilteringOrdersByDistrictService(
    IOrderRepository orderRepository)
{
    public async Task<Result<List<OrderDto>>> ExecuteAsync(
        FilteringOrdersByDistrictRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await orderRepository
            .FilteringOrdersByDistrict(request.District, request.FirstDeliveryTime, cancellationToken);
        if (result.IsFailure)
            return result.ErrorList;

        return result;
    }
}