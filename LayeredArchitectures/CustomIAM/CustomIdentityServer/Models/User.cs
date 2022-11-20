namespace CustomIdentityServer.Models;

public class User
{
    public User()
    {
        Roles = new List<Role>();
    }
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public ICollection<Role> Roles { get; set; }
}