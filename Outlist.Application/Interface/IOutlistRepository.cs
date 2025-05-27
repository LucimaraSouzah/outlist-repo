using Outlist.Domain.Entities;

namespace Outlist.Application.Interface;
public interface IOutlistRepository
{
    Task AddProductAsync(OutlistEntry entry);
    Task RemoveProductAsync(Guid productId);
    Task UpdateValidityAsync(Guid productId, DateTime startDate, DateTime endDate);
    Task<List<OutlistEntry>> GetAllProductsAsync(int page, int pageSize);
    Task<bool> IsProductBlockedAsync(Guid productId);
}
