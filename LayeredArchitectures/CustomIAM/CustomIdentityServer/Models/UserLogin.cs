namespace CustomIdentityServer.Models;

public class UserLogin
{
    public Guid Id { get; set; }
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
    public DateTime LoginAt { get; set;}
}
