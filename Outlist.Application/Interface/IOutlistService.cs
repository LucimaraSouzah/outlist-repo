using Outlist.Application.DTOs;

namespace Outlist.Application.Interface;
public interface IOutlistService
{
    Task AddProductAsync(Guid productId, DateTime startDate, DateTime endDate);
    Task RemoveProductAsync(Guid productId);
    Task UpdateValidityAsync(Guid productId, DateTime startDate, DateTime endDate);
    Task<List<OutlistProductDto>> GetAllProductsAsync(int page, int pageSize);
    Task<bool> IsProductBlockedAsync(Guid productId);
}
