using EffectiveMobile.Application.AddOrder;
using EffectiveMobile.Application.FilteringOrdersByDistrict;
using Microsoft.AspNetCore.Mvc;

namespace EffectiveMobile.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    /// <summary>
    /// Creating an order
    /// </summary>
    /// <param name="service"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <remarks>
    /// Request for create order
    ///
    ///     POST
    ///     {
    ///         "weight": 2.2,
    ///         "district": "Ленинский",
    ///         "deliveryTime": "2024-10-27 10:10:10"
    ///     }
    /// </remarks>
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromServices] AddOrderService service,
        [FromBody] AddOrderRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.ExecuteAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.ErrorList.ToResponse();

        return Ok();
    }

    /// <summary>
    /// Filtering orders for delivery to a specific area of the city in the next half hour after the time of the first order
    /// </summary>
    /// <param name="service"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <remarks>
    /// Request for filtering orders
    ///
    ///     GET
    ///     {
    ///         "district": "Ленинский",
    ///         "firstDeliveryTime": "2024-10-27 10:10:00"
    ///     }
    /// </remarks>
    [HttpGet("districts")]
    public async Task<ActionResult> GetFilteringOrdersByDistrict(
        [FromServices] FilteringOrdersByDistrictService service,
        [FromQuery] FilteringOrdersByDistrictRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.ExecuteAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.ErrorList.ToResponse();

        return Ok(result);
    }
}