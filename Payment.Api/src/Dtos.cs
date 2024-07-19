using System.ComponentModel.DataAnnotations;

namespace Payment.Api;

public class Dtos
{
    public record PaymentDto(Guid Id, string email, decimal Amount, string Currency, int Status, DateTimeOffset CreatedDate);

    public record CreatePaymentDto([Required] string email, [Required][Range(1, 10000)] decimal Amount, string Currency, [Range(0, 2)] int Status);

    public record UpdatePaymentDto([Required] string email, [Required][Range(1, 10000)] decimal Amount, string Currency, [Range(0, 2)] int Status);
}
