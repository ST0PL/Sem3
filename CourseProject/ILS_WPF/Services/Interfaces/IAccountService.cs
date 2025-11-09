using ILS_WPF.Models.Database;

namespace ILS_WPF.Services.Interfaces
{
    internal interface IAccountService
    {
        Task<User?> LoginAsync(string username, string password);
        Task<User> RegisterAsync(string username, string password, Role role);
        Task RemoveAsync(string username);
        Task ChangePasswordAsync(string username, string newPassword);
    }
}
