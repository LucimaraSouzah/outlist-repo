using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Microsoft.Extensions.Options;
using Outlist.Domain.Entities;
using MongoDB.Bson;

namespace Outlist.Infrastructure
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        private static bool _isGuidSerializerRegistered;

        public MongoContext(IOptions<MongoDbSettings> settings)
        {
            if (!_isGuidSerializerRegistered)
            {
                BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
                _isGuidSerializerRegistered = true;
            }

            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<OutlistEntry> OutlistEntries => _database.GetCollection<OutlistEntry>("OutlistEntries");
    }
}
