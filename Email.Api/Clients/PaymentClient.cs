using static Email.Api.Dtos;

namespace Email.Api.Clients;

public class PaymentClient
{
    private readonly HttpClient httpClient;

    public PaymentClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public async Task<IReadOnlyCollection<PaymentItemDto>> GetPaymentItemDtosAsync()
    {
        var response = await httpClient.GetFromJsonAsync<IReadOnlyCollection<PaymentItemDto>>("api/payments");
        return response!;
    }
}
