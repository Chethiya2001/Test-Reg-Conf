using common.Api;
using Email.Api.Entities;
using MassTransit;
using static Api.Payment.Contracts.Contracts;

namespace Email.Api.Consumers;

public class PaymentUpdatedConsumer : IConsumer<PaymentItemUpdated>
{
    private readonly IRepository<PaymentItem> repository;

    public PaymentUpdatedConsumer(IRepository<PaymentItem> repository)
    {
        this.repository = repository;
    }
    public async Task Consume(ConsumeContext<PaymentItemUpdated> context)
    {
        var msg = context.Message;
        var item = repository.GetAsync(msg.PaymentItemId).GetAwaiter().GetResult();
        if (item == null)
        {
            item = new PaymentItem
            {
                Id = msg.PaymentItemId,
                Email = msg.Email,
                Amount = msg.Amount,
                Status = msg.Status
            };
            await repository.CreateAsync(item);
        }
        else
        {
            item.Email = msg.Email;
            item.Amount = msg.Amount;
            item.Status = msg.Status;
            
            await repository.UpdateAsync(item);
        }
    }

}
