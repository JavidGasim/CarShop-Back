namespace CarShop.WepApi.Services.Abstracts
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
