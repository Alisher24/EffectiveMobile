using EffectiveMobile.Domain;
using EffectiveMobile.Domain.Shared;

namespace EffectiveMobile.Application;

/// <summary>
/// Service for working with orders
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Added order
    /// </summary>
    /// <param name="order"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result> AddOrderAsync(Order order, CancellationToken cancellationToken = default);

    /// <summary>
    /// Filtering orders by district with time
    /// </summary>
    /// <param name="district"></param>
    /// <param name="firstOrderTime"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result<List<OrderDto>>> FilteringOrdersByDistrict(
        string district,
        DateTime firstOrderTime,
        CancellationToken cancellationToken = default);
}