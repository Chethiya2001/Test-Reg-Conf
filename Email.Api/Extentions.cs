

using Email.Api.Entities;
using static Email.Api.Dtos;

namespace Email.Api;

public static class Extentions
{
    public static EmailItemDto AsDto(this EmailItem item, string email, decimal Amount, int Status)
    {
        return new EmailItemDto(item.PaymentItemId, item.Quantity, email, Amount, Status, item.AcquiredDate);
    }
}
