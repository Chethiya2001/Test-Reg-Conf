using Payment.Api.Entities;
using static Payment.Api.Dtos;

namespace Payment.Api;

public static class Extentions
{
    public static PaymentDto AsDto(this PaymentItem payment)
    {
        return new PaymentDto(payment.Id, payment.Email!, payment.Amount, payment.Currency!, payment.Status, payment.CreatedDate);
    }
}
