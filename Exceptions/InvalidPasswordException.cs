using Exceptions.ErrorCodes;
using Exceptions.Localized;

namespace Exceptions
{
    public class InvalidPasswordException : LocalizedBusinessLogicException
    {
        public InvalidPasswordException()
            : base(CustomErrorCodes.InvalidPassword, Resources.Exception.ResourceManager)
        { }
    }
}