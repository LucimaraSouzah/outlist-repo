using MongoDB.Driver;
using Outlist.Domain.Entities;
using Microsoft.Extensions.Logging;
using Outlist.Application.Interface;

namespace Outlist.Infrastructure.Repositories
{
    public class OutlistRepository : IOutlistRepository
    {
        private readonly MongoContext _context;
        private readonly ILogger<OutlistRepository> _logger;

        public OutlistRepository(MongoContext context, ILogger<OutlistRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddProductAsync(OutlistEntry entry)
        {
            await _context.OutlistEntries.InsertOneAsync(entry);
            _logger.LogInformation($"Product {entry.ProductId} added to Outlist.");
        }

        public async Task RemoveProductAsync(Guid productId)
        {
            var filter = Builders<OutlistEntry>.Filter.Eq(e => e.ProductId, productId);
            await _context.OutlistEntries.DeleteOneAsync(filter);
            _logger.LogInformation($"Product {productId} removed from Outlist.");
        }

        public async Task UpdateValidityAsync(Guid productId, DateTime startDate, DateTime endDate)
        {
            var filter = Builders<OutlistEntry>.Filter.Eq(e => e.ProductId, productId);
            var update = Builders<OutlistEntry>.Update
                .Set(e => e.StartDate, startDate)
                .Set(e => e.EndDate, endDate);

            await _context.OutlistEntries.UpdateOneAsync(filter, update);
            _logger.LogInformation($"Product {productId} validity updated.");
        }

        public async Task<List<OutlistEntry>> GetAllProductsAsync(int page, int pageSize)
        {
            if (pageSize > 200) pageSize = 200; // limite máximo 200
            return await _context.OutlistEntries
                .Find(_ => true)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<bool> IsProductBlockedAsync(Guid productId)
        {
            var now = DateTime.UtcNow;
            var filter = Builders<OutlistEntry>.Filter.And(
                Builders<OutlistEntry>.Filter.Eq(e => e.ProductId, productId),
                Builders<OutlistEntry>.Filter.Lte(e => e.StartDate, now),
                Builders<OutlistEntry>.Filter.Gte(e => e.EndDate, now));

            var count = await _context.OutlistEntries.CountDocumentsAsync(filter);
            return count > 0;
        }
    }
}
