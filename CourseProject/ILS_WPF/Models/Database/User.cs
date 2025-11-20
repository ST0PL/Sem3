using ILS_WPF.Models.Core;

namespace ILS_WPF.Models.Database
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; } = null!;
        public string Hash { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public Role Role { get; set; }
        public int? StaffId { get; set; }
        public virtual Staff? Staff { get; set; }

        protected User() { }
        public User(string username, string hash, string salt, Role role, Staff profile)
        {
            Username = username;
            Hash = hash;
            Salt = salt;
            Role = role;
            Staff = profile;
        }
    }
}
