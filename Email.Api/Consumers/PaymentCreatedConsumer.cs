using common.Api;
using Email.Api.Entities;
using MassTransit;
using static Api.Payment.Contracts.Contracts;

namespace Email.Api.Consumers;

public class PaymentCreatedConsumer : IConsumer<PaymentItemCreated>
{
    private readonly IRepository<PaymentItem> repository;

    public PaymentCreatedConsumer(IRepository<PaymentItem> repository)
    {
        this.repository = repository;
    }
    public async Task Consume(ConsumeContext<PaymentItemCreated> context)
    {
        var msg = context.Message;
        var item = await repository.GetAsync(msg.PaymentItemId);

        if (item != null)
        {
            return;
        }

        item = new PaymentItem
        {
            Id = msg.PaymentItemId,
            Email = msg.Email,
            Amount = msg.Amount,
            Status = msg.Status
        };
        await repository.CreateAsync(item);
    }
}
