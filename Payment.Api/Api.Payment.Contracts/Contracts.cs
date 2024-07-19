namespace Api.Payment.Contracts;

public class Contracts
{
    public record PaymentItemCreated(Guid PaymentItemId, string Email, decimal Amount, int Status, string Currency);

    public record PaymentItemUpdated(Guid PaymentItemId, string Email, decimal Amount, int Status, string Currency);

    public record PaymentItemDeleted(Guid PaymentItemId);
}
