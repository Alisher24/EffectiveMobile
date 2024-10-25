using EffectiveMobile.Application;
using EffectiveMobile.Domain;
using EffectiveMobile.Domain.Shared;

namespace EffectiveMobile.Infrastructure;

public class OrderRepository(string ordersPath, string districtsPath, string deliveryOrder)
    : IOrderRepository
{
    public async Task<Result> AddOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        if (File.Exists(ordersPath) == false)
            File.Create(ordersPath);

        var districtResult = await GetDistrict(order.District.Value, cancellationToken);
        if (districtResult.IsFailure)
            return districtResult.ErrorList;

        var orderText = $"{order.Id};{order.Weight.Value};{districtResult.Value};{order.DeliveryTime.Value}";
        await File.AppendAllTextAsync(ordersPath, orderText + Environment.NewLine, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<List<OrderDto>>> FilteringOrdersByDistrict(
        string district,
        DateTime firstOrderTime,
        CancellationToken cancellationToken = default)
    {
        var districtResult = await GetDistrict(district, cancellationToken);
        if (districtResult.IsFailure)
            return districtResult.ErrorList;
        
        if (File.Exists(ordersPath) == false)
            return Error.FileNotExist("Orders ");

        var maxOrderTime = firstOrderTime.AddMinutes(30);
        var ordersByDistrict = new List<OrderDto>();
        using var orders = new StreamReader(ordersPath);
        var orderText = string.Empty;
        while ((orderText = await orders.ReadLineAsync(cancellationToken)) != null)
        {
            var orderArr = orderText.Split(";");
            var orderId = Guid.Parse(orderArr[0]);
            var orderWeight = orderArr[1];
            var orderDistrict = orderArr[2];
            var orderDeliveryTime = DateTime.Parse(orderArr[3]);

            if (orderDistrict == district
                && orderDeliveryTime >= firstOrderTime
                && orderDeliveryTime <= maxOrderTime)
                ordersByDistrict.Add(new OrderDto(orderId, orderWeight, orderDistrict, orderDeliveryTime));
        }

        if (File.Exists(deliveryOrder))
            File.Delete(deliveryOrder);

        await using var ordersToRead = new StreamWriter(deliveryOrder);
        if (ordersByDistrict.Count == 0)
            return ordersByDistrict;

        ordersByDistrict = ordersByDistrict.OrderBy(o => o.DeliveryTime).ToList();

        foreach (var orderDto in ordersByDistrict)
        {
            await ordersToRead.WriteLineAsync(
                $"{orderDto.Id};{orderDto.Weight};{orderDto.DeliveryTime};{orderDto.District}");
        }

        return ordersByDistrict;
    }

    private async Task<Result<string>> GetDistrict(string districtName, CancellationToken cancellationToken = default)
    {
        using var districts = new StreamReader(districtsPath);
        var district = string.Empty;
        var districtResult = false;
        while ((district = await districts.ReadLineAsync(cancellationToken)) != null)
        {
            if (district.Equals(districtName, StringComparison.CurrentCultureIgnoreCase))
            {
                districtResult = true;
                break;
            }
        }

        if (districtResult == false)
            return Error.ValueNotFound($"District with name: {districtName} ");

        return district!;
    }
}