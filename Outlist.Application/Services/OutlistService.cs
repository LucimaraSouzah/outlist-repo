using Outlist.Application.DTOs;
using Outlist.Application.Interface;
using Outlist.Application.UseCases;

namespace Outlist.Application.Services;
public class OutlistService : IOutlistService
{
    private readonly AddProductUseCase _addProduct;
    private readonly RemoveProductUseCase _removeProduct;
    private readonly UpdateValidityUseCase _updateValidity;
    private readonly GetAllProductsUseCase _getAllProducts;
    private readonly CheckProductBlockedUseCase _checkProductBlocked;

    public OutlistService(
        AddProductUseCase addProduct,
        RemoveProductUseCase removeProduct,
        UpdateValidityUseCase updateValidity,
        GetAllProductsUseCase getAllProducts,
        CheckProductBlockedUseCase checkProductBlocked)
    {
        _addProduct = addProduct;
        _removeProduct = removeProduct;
        _updateValidity = updateValidity;
        _getAllProducts = getAllProducts;
        _checkProductBlocked = checkProductBlocked;
    }

    public async Task AddProductAsync(Guid productId, DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate)
            throw new ArgumentException("EndDate must be greater than StartDate.");

        await _addProduct.ExecuteAsync(productId, startDate, endDate);
    }

    public async Task RemoveProductAsync(Guid productId)
    {
        await _removeProduct.ExecuteAsync(productId);
    }

    public async Task UpdateValidityAsync(Guid productId, DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate)
            throw new ArgumentException("EndDate must be greater than StartDate.");

        await _updateValidity.ExecuteAsync(productId, startDate, endDate);
    }

    public async Task<List<OutlistProductDto>> GetAllProductsAsync(int page, int pageSize)
    {
        var entries = await _getAllProducts.ExecuteAsync(page, pageSize);
        return entries.Select(e => new OutlistProductDto
        {
            ProductId = e.ProductId,
            StartDate = e.StartDate,
            EndDate = e.EndDate
        }).ToList();
    }

    public async Task<bool> IsProductBlockedAsync(Guid productId)
    {
        return await _checkProductBlocked.ExecuteAsync(productId);
    }
}
