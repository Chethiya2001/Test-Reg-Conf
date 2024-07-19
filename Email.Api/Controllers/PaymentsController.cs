using common.Api;
using Email.Api.Entities;
using Email.Api.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Email.Api.Dtos;

namespace Email.Api.Controllers;


[ApiController]
[Route("api/email")]

public class PaymentsController : ControllerBase
{
    private readonly IRepository<EmailItem> emailrepository;
    private readonly IRepository<PaymentItem> paymentrepository;

    private readonly EmailService emailService;

    public PaymentsController(IRepository<EmailItem> emailrepository, IRepository<PaymentItem> paymentrepository, EmailService emailService)
    {
        this.emailrepository = emailrepository;
        this.paymentrepository = paymentrepository;
        this.emailService = emailService;
    }
    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<EmailItemDto>>> GetAsync(Guid userId)
    // {
    //     if (userId == Guid.Empty)
    //     {
    //         return BadRequest();
    //     }

    //     var emailItemsEntities = await emailrepository.GetAllAsync(item => item.UserId == userId);
    //     var idemIds = emailItemsEntities.Select(item => item.PaymentItemId);
    //     var paymentItemsEntities = await paymentrepository.GetAllAsync(item => idemIds.Contains(item.Id));

    //     var emailItemsDto = emailItemsEntities.Select(emailItems =>
    //     {
    //         var paymentItem = paymentItemsEntities.Single(paymentItem => paymentItem.Id == emailItems.PaymentItemId);
    //         return emailItems.AsDto(paymentItem.Email!, paymentItem.Amount, paymentItem.Status);
    //     }).ToList();
    //     //send email if status == 1
    //     foreach (var item in emailItemsDto)
    //     {
    //         if (item.Status == 1)
    //         {
    //             await emailService.SendEmailAsync(item.Email, "Payment Confirmed", $"Paid {item.Amount} & {item.Quantity} is Successfully.");
    //         }
    //     }

    //     return Ok(emailItemsDto);

    // }

    [HttpPost]
    public async Task<ActionResult> PostAsync(GrantItemsDto gitem)
    {
        var emailItems = await emailrepository.GetAsync(
            item => item.UserId == gitem.UserId && item.PaymentItemId == gitem.PaymentItemId);
        if (emailItems == null)
        {
            emailItems = new EmailItem
            {

                UserId = gitem.UserId,
                PaymentItemId = gitem.PaymentItemId,
                Quantity = gitem.Quantity,
                AcquiredDate = DateTime.UtcNow
            };
            await emailrepository.CreateAsync(emailItems);

        }
        else
        {
            emailItems.Quantity += gitem.Quantity;
            await emailrepository.UpdateAsync(emailItems);
        }
        var emailItemsDto = await GetEmailItemsDtoAsync(gitem.UserId);
        return Ok(emailItemsDto);




    }
    private async Task<List<EmailItemDto>> GetEmailItemsDtoAsync(Guid userId)
    {

        var emailItemsEntities = await emailrepository.GetAllAsync(item => item.UserId == userId);
        var idemIds = emailItemsEntities.Select(item => item.PaymentItemId);
        var paymentItemsEntities = await paymentrepository.GetAllAsync(item => idemIds.Contains(item.Id));

        var emailItemsDto = emailItemsEntities.Select(emailItems =>
        {
            var paymentItem = paymentItemsEntities.Single(paymentItem => paymentItem.Id == emailItems.PaymentItemId);
            return emailItems.AsDto(paymentItem.Email!, paymentItem.Amount, paymentItem.Status);
        }).ToList();

        // Send email if status == 1
        foreach (var item in emailItemsDto)
        {
            if (item.Status == 1)
            {
                await emailService.SendEmailAsync(item.Email, "Payment Confirmed", $"Paid {item.Amount} is Successfully.");
            }
        }

        return emailItemsDto;
    }
}
