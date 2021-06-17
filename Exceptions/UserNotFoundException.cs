using Exceptions.ErrorCodes;
using Exceptions.Localized;

namespace Exceptions
{
    public class UserNotFoundException : LocalizedBusinessLogicException
    {
        public UserNotFoundException()
            : base(CustomErrorCodes.UserNotFound, Resources.Exception.ResourceManager)
        { }
    }
}