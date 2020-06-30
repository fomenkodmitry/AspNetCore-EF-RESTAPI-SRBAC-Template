namespace Domain.Error
{
    public enum ErrorCodes
    {
        RequiredFieldsMissing = 1,
        WrongId = 2,
        WrongFormat = 3,
        NotFound = 5,
        WrongType = 6,

        InvalidPhoneNumber = 7,
        
        YouAreBlocked = 8,
        UserIsBlocked = 9,
        
        UserEmailExists = 10,
        UserPhoneExists = 11,
        
        IncorrectEmailOrPassword = 12,
        
        UserEmailNotExists = 13,
        
        ServerError = 14,
    }
}