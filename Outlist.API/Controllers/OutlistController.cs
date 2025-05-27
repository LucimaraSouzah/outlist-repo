using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Outlist.Application.DTOs;
using Outlist.Application.Interface;

namespace Outlist.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OutlistController : ControllerBase
{
    private readonly IOutlistService _service;

    public OutlistController(IOutlistService service)
    {
        _service = service;
    }

    /// <summary>
    /// Adiciona um produto na Outlist para bloquear alteração de preço.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddProductAsync([FromBody] AddOutlistRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _service.AddProductAsync(request.ProductId, request.StartDate, request.EndDate);

        return Created($"/api/outlist/check/{request.ProductId}", null);
    }

    /// <summary>
    /// Remove um produto da Outlist, liberando alteração de preço.
    /// </summary>
    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveProductAsync(Guid productId)
    {
        await _service.RemoveProductAsync(productId);
        return NoContent();
    }

    /// <summary>
    /// Atualiza o período de bloqueio do produto na Outlist.
    /// </summary>
    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateValidityAsync(Guid productId, [FromBody] UpdateValidityRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _service.UpdateValidityAsync(productId, request.StartDate, request.EndDate);
        return NoContent();
    }

    /// <summary>
    /// Retorna lista paginada dos produtos na Outlist (máx 200 por página).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (pageSize < 1 || pageSize > 200)
            return BadRequest("pageSize must be between 1 and 200");

        var result = await _service.GetAllProductsAsync(page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Verifica se o produto está bloqueado para alteração de preço.
    /// </summary>
    [HttpGet("check/{productId}")]
    public async Task<IActionResult> CheckProductAsync(Guid productId)
    {
        var isBlocked = await _service.IsProductBlockedAsync(productId);
        return Ok(isBlocked);
    }
}
