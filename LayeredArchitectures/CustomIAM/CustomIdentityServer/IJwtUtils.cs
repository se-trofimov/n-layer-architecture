using CustomIdentityServer.Models;

namespace CustomIdentityServer;

public interface IJwtUtils
{
    public string GenerateToken(User user, string clientSecret);
}