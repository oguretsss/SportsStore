namespace SportsStore.WebUI.Abstract
{
    public interface IAuthProvider
    {
        bool Authenticate(string username, string password);
    }
}
