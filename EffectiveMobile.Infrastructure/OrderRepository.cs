using EffectiveMobile.Application;
using EffectiveMobile.Domain;
using EffectiveMobile.Domain.Shared;

namespace EffectiveMobile.Infrastructure;

public class OrderRepository : IOrderRepository
{
    private readonly string _ordersPath;
    private readonly string _districtsPath;
    public OrderRepository(string ordersPath, string districtsPath)
    {
        _ordersPath = ordersPath;
        _districtsPath = districtsPath;
        if (File.Exists(ordersPath) == false)
            File.Create(ordersPath);
    }

    public async Task<Result> AddOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        using var districts = new StreamReader(_districtsPath);
        var district = string.Empty;
        var districtResult = false;
        while ((district = await districts.ReadLineAsync(cancellationToken)) != null)
        {
            if (district.Equals(order.District.Value, StringComparison.CurrentCultureIgnoreCase))
            {
                districtResult = true;
                break;
            }
        }

        if (districtResult == false)
            return Error.ValueNotFound($"District with name: {order.District.Value} ");
        
        var orderText = $"{order.Id};{order.Weight.Value};{district};{order.DeliveryTime.Value}";
        await File.AppendAllTextAsync(_ordersPath, orderText + Environment.NewLine, cancellationToken);
        return Result.Success();
    }
}