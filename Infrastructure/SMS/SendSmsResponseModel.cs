using System;

namespace Infrastructure.SMS
{
    public class SendSmsResponseModel
    {
        public Guid MessageId { get; set; }
        public DateTime SentAt { get; set; }
    }
}
