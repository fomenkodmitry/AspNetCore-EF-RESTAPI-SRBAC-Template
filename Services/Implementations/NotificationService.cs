using System.Threading.Tasks;
using Domain.Push;
using Infrastructure.Template;

namespace Services.Implementations
{
    public class NotificationService : IPushService
    {
        private readonly IPushSender _pushSender;
        private readonly TemplateContainer _templateContainer;
        public NotificationService(IPushSender pushSender, TemplateContainer templateContainer)
        {
            _pushSender = pushSender;
            _templateContainer = templateContainer;
        }

        public async Task SendTest(string pushToken)
        {
            if (!string.IsNullOrWhiteSpace(pushToken))
            {
                var data = _templateContainer.Push[_templateContainer.DefaultLanguage].Test;
                await _pushSender.PushToDevice(pushToken, data.Tittle, data.Body,
                    new NotificationDataContainer(NotificationTypes.Test, ObjectType.Test, "Test")
                );
            }
        }
    }
}