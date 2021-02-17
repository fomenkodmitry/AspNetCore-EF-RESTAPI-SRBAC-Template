namespace Infrastructure.SMS
{
    public class SmsConfiguration
    {
        public string ProviderName { get; set; }
        public bool IsStub => ProviderName == "stub";
    }
}
