namespace ILS_WPF.Models.Database
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; } = null!;
        public string Hash { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public Role Role { get; set; }

        public User() { }
        public User(string username, string hash, string salt, Role role)
        {
            Username = username;
            Hash = hash;
            Salt = salt;
            Role = role;
        }
    }
}
