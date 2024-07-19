using common.Api;
using Email.Api.Entities;
using MassTransit;
using static Api.Payment.Contracts.Contracts;

namespace Email.Api.Consumers;

public class PaymentDeletedConsumer : IConsumer<PaymentItemDeleted>
{
    private readonly IRepository<PaymentItem> repository;

    public PaymentDeletedConsumer(IRepository<PaymentItem> repository)
    {
        this.repository = repository;
    }
    public async Task Consume(ConsumeContext<PaymentItemDeleted> context)
    {
        var msg = context.Message;
        var item = repository.GetAsync(msg.PaymentItemId);
        if (item == null)
        {
            return;
        }
        await repository.RemoveAsync(msg.PaymentItemId);
    }

}
