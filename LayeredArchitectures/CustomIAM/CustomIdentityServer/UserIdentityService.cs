using CustomIdentityServer.Data;
using CustomIdentityServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomIdentityServer;

public class UserIdentityService
{
    private readonly IdentityDbContext _identityDbContext;
    private readonly PasswordEncrypt _passwordEncrypt;

    public UserIdentityService(IdentityDbContext identityDbContext, PasswordEncrypt passwordEncrypt)
    {
        _identityDbContext = identityDbContext ?? throw new ArgumentNullException(nameof(identityDbContext));
        _passwordEncrypt = passwordEncrypt ?? throw new ArgumentNullException(nameof(passwordEncrypt));
    }

    public async Task<User> CreateUserAsync(string userName, string email, string password)
    {
        var userExists = await _identityDbContext.Users
            .AnyAsync(x => x.Email == email.ToLower());

        if (userExists)
            throw new InvalidOperationException($"User with email {email} already exists");

        User user = new()
        {
            Name = userName,
            Email = email,
            Password = _passwordEncrypt.GeneratePasswordHashUsingSalt(password)
        };

        _identityDbContext.Users.Add(user);
        await _identityDbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _identityDbContext.Users
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public bool PasswordIsValid(User user, string password)
    {
       var salt = _passwordEncrypt.ExtractSaltFromEncryptedPassword(user.Password);
       var regenerate = _passwordEncrypt.GeneratePasswordHashUsingSalt(password, salt);
       return password == regenerate;
    }
}
