namespace CustomIdentityServer.Models;

public class Permission
{
    public Permission()
    {
        Roles = new List<Role>();
    }
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public ICollection<Role> Roles { get; set; }
}
