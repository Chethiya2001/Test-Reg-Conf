using common.Api;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Payment.Api.Entities;

public class PaymentItem : IEntity
{
    public Guid Id { get; set; }

    public string? Email { get; set; }

    public decimal Amount { get; set; }

    public int Status { get; set; }

    public string? Currency { get; set; }

    public DateTimeOffset CreatedDate { get; set; }
}
