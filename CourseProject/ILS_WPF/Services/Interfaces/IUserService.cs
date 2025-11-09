using ILS_WPF.Models.Database;

namespace ILS_WPF.Services.Interfaces
{
    internal interface IUserService
    {
        void SetUser(User user);
        User? GetUser();
    }
}
