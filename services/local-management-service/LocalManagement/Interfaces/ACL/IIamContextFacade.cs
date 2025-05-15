namespace LocalManagement.Interfaces.ACL
{
    public interface IIamContextFacade
    {
        Task<bool> UserExists(int userId);
    }
}