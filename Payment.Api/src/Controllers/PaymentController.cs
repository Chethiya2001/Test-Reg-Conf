
using common.Api;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.Api.Entities;
using static Api.Payment.Contracts.Contracts;

using static Payment.Api.Dtos;

namespace Payment.Api.Controllers;

[ApiController]
[Route("api/payments")]

public class PaymentController : ControllerBase
{
    private readonly IRepository<PaymentItem> paymentRepository;
    private readonly IPublishEndpoint publishEndpoint;

    public PaymentController(IRepository<PaymentItem> paymentRepository, IPublishEndpoint publishEndpoint)
    {
        this.paymentRepository = paymentRepository;
        this.publishEndpoint = publishEndpoint;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentDto>>> GetAsync()
    {

        var payments = (await paymentRepository.GetAllAsync()).Select(x => x.AsDto());

        return Ok(payments);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentDto>> GetByIdAsync(Guid id)
    {
        var payments = await paymentRepository.GetAsync(id);
        if (payments == null)
        {
            return NotFound();
        }
        return payments.AsDto();
    }

    [HttpPost]
    public async Task<ActionResult<PaymentDto>> PostAsync(CreatePaymentDto create)
    {
        var payment = new PaymentItem
        {
            Id = Guid.NewGuid(),
            Email = create.email,
            Amount = create.Amount,
            Currency = create.Currency,
            Status = create.Status
        };
        await paymentRepository.CreateAsync(payment);

        await publishEndpoint.Publish(new PaymentItemCreated(payment.Id, payment.Email, payment.Amount, payment.Status, payment.Currency));
        return CreatedAtAction(nameof(GetByIdAsync), new { id = payment.Id }, payment);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PaymentDto>> PutAsynch(Guid id, UpdatePaymentDto update)
    {
        var expayment = await paymentRepository.GetAsync(id);
        if (expayment == null)
        {
            return NotFound();
        }
        expayment.Email = update.email;
        expayment.Amount = update.Amount;
        expayment.Currency = update.Currency;
        expayment.Status = update.Status;

        await paymentRepository.UpdateAsync(expayment);
        await publishEndpoint.Publish(new PaymentItemUpdated(expayment.Id, expayment.Email, expayment.Amount, expayment.Status, expayment.Currency));

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<PaymentDto>> DeleteAsync(Guid id)
    {
        var payment = await paymentRepository.GetAsync(id);
        if (payment == null)
        {
            return NotFound();
        }
        await paymentRepository.RemoveAsync(id);
        await publishEndpoint.Publish(new PaymentItemDeleted(payment.Id));

        return Ok("Deleted");
    }
}
