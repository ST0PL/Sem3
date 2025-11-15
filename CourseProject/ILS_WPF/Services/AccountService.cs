using ILS_WPF.Models.Database;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ILS_WPF.Services
{
    class AccountService : IAccountService
    {
        private readonly ILSContext _context;
        public AccountService(ILSContext context)
            => _context = context;

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await GetUserAsync(username);
            byte[] hashed = SHA256.HashData(Encoding.UTF8.GetBytes(password+user?.Salt));
            return user?.Hash == Convert.ToBase64String(hashed) ? user : null;
        }

        public async Task<User> RegisterAsync(string username, string password, Role role)
        {
            if (await GetUserAsync(username) != null)
                throw new InvalidOperationException("Пользователь с данным именем уже существует.");
            var user = CreateUser(username, password, role);
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task ChangePasswordAsync(string username, string newPassword)
        {
            var user = await GetUserOrThrowAsync(username);
            var (Password, Salt) = ComputeCreds(newPassword);
            user.Hash = Password;
            user.Salt = Salt;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(string username)
        {
            var user = await GetUserOrThrowAsync(username);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        private async Task<User?> GetUserAsync(string username)
            =>  await _context.Users.Where(u => u.Username == username.ToLower()).FirstOrDefaultAsync();
        
        private async Task<User> GetUserOrThrowAsync(string username)
            => (await GetUserAsync(username)) ?? throw new InvalidOperationException($"Пользователь с именем \"{username}\" не найден.");

        private static User CreateUser(string username, string password, Role role)
        {
            var (Password, Salt) = ComputeCreds(password);
            return new User(username, Password, Salt, role);
        }

        private static (string, string) ComputeCreds(string password)
        {
            var saltBytes = new byte[16];
            RandomNumberGenerator.Fill(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);
            return (Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password + salt))),salt);
        }

        public async Task<User?> LoginWithHashAsync(string username, string hash)
        {
            var user = await GetUserAsync(username);
            return user?.Hash == hash ? user : null;
        }
    }
}
