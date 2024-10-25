using EffectiveMobile.Application.AddOrder;
using EffectiveMobile.Application.FilteringOrdersByDistrict;
using Microsoft.AspNetCore.Mvc;

namespace EffectiveMobile.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
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