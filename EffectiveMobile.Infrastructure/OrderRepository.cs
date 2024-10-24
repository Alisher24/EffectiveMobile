using EffectiveMobile.Application;
using EffectiveMobile.Domain;

namespace EffectiveMobile.Infrastructure;

public class OrderRepository : IOrderRepository
{
    private readonly string _fileName;
    public OrderRepository(string fileName)
    {
        _fileName = fileName;
        if (File.Exists(fileName) == false)
            File.Create(fileName);
    }

    public async Task AddOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        var orderText = $"{order.Id};{order.Weight.Value};{order.District.Value};{order.DeliveryTime.Value}";
        await File.AppendAllTextAsync(_fileName, orderText + Environment.NewLine, cancellationToken);
    }
}