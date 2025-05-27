using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Outlist.Domain.Entities;
public class OutlistEntry
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public Guid ProductId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}