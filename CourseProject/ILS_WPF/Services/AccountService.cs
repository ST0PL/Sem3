using ILS_WPF.Models.Database;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ILS_WPF.Services
{
    class AccountService : IAccountService
    {
        private readonly IDbContextFactory<ILSContext> _dbFactory;
        public AccountService(IDbContextFactory<ILSContext> context)
            => _dbFactory = context;

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await GetUserAsync(username);
            byte[] hashed = SHA256.HashData(Encoding.UTF8.GetBytes(password+user?.Salt));
            return user?.Hash == Convert.ToBase64String(hashed) ? user : null;
        }

        public async Task<User> RegisterAsync(string username, string password, Role role, int? profileId = null)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            if (await GetUserAsync(username) != null)
                throw new InvalidOperationException("Пользователь с данным именем уже существует.");
            var user = CreateUser(username, password, role, profileId);
            await context.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task ChangePasswordAsync(string username, string newPassword)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var user = await GetUserOrThrowAsync(username);
            var (Password, Salt) = ComputeCreds(newPassword);
            user.Hash = Password;
            user.Salt = Salt;
            await context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int accountId)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            int removed = await context.Users.Where(u => u.Id == accountId).ExecuteDeleteAsync();
            if(removed > 0)
                await context.SaveChangesAsync();
        }

        private async Task<User?> GetUserAsync(string username)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            return await context.Users
                .Where(u => u.Username == username.ToLower())
                .Include(u=>u.Staff)
                .FirstOrDefaultAsync();
        }
        
        private async Task<User> GetUserOrThrowAsync(string username)
            => (await GetUserAsync(username)) ?? throw new InvalidOperationException($"Пользователь с именем \"{username}\" не найден.");

        private static User CreateUser(string username, string password, Role role, int? profileId = null)
        {
            var (Password, Salt) = ComputeCreds(password);
            return new User(username, Password, Salt, role, profileId);
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
