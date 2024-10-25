using EffectiveMobile.Domain;
using EffectiveMobile.Domain.Shared;

namespace EffectiveMobile.Application;

public interface IOrderRepository
{
    public Task<Result> AddOrderAsync(Order order, CancellationToken cancellationToken = default);

    public Task<Result<List<OrderDto>>> FilteringOrdersByDistrict(
        string district,
        DateTime firstOrderTime,
        CancellationToken cancellationToken = default);
}