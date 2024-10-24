using EffectiveMobile.Domain;

namespace EffectiveMobile.Application;

public interface IOrderRepository
{
    public Task AddOrderAsync(Order order, CancellationToken cancellationToken = default);
}