namespace CustomIdentityServer.Models;

public class Role
{
    public Role()
    {
        Permissions = new List<Permission>();
        Users = new List<User>();
    }

    public Guid Id { get; set; }
    public string? Title { get; set; }
    public ICollection<Permission> Permissions { get; set; }
    public ICollection<User> Users { get; set; }
}
