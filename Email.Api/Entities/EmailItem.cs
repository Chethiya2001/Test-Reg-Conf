using common.Api;

namespace Email.Api.Entities;

public class EmailItem : IEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid PaymentItemId { get; set; }

    public int Quantity { get; set; }

    public DateTimeOffset AcquiredDate { get; set; }

}
