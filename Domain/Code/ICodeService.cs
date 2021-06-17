namespace Domain.Code
{
    public interface ICodeService
    {
        string GenerateCode(int len);
        string GeneratePassword(int len);
    }
}