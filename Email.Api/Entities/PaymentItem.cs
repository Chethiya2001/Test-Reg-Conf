using common.Api;

namespace Email.Api.Entities;

public class PaymentItem : IEntity
{
    public Guid Id { get; set; }

    public string? Email { get; set; }

    public decimal Amount { get; set; }

    public int Status { get; set; }


}
