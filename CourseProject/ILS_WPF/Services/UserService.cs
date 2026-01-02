using ILS_WPF.Models.Database;
using ILS_WPF.Services.Interfaces;

namespace ILS_WPF.Services
{
    internal class UserService : IUserService
    {
        private User? _user;

        public User? GetUser()
            => _user;

        public void SetUser(User user)
            => _user = user;
    }
}
