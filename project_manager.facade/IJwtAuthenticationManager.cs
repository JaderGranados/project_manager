
namespace project_manager.contract
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(string username, string password);
    }
}
