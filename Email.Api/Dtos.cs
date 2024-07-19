namespace Email.Api;

public class Dtos
{
    public record GrantItemsDto(Guid UserId, Guid PaymentItemId, int Quantity);

    public record EmailItemDto(Guid PaymentItemId, int Quantity, string Email, decimal Amount, int Status, DateTimeOffset AcquiredDate);

    public record PaymentItemDto(Guid Id, string email, decimal Amount, int Status);
}
