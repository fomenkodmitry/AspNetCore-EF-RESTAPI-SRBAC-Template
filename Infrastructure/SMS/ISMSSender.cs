using System.Threading.Tasks;

namespace Infrastructure.SMS
{
    public interface ISMSSender
    {
        SendSmsResponseModel SendSMS(string phoneNumber, string text);
    }
}
