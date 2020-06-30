using System.Threading.Tasks;

namespace Infrastructure.Email
{
    public interface IEmailSender
    {
        Task<EmailResponseModel> SendAsync(string toAddress, string subject, string body, string attachmentPath = null);
        EmailResponseModel Send(string toAddress, string subject, string body, string attachmentPath = null);
    }
}