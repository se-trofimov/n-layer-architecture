using CustomIdentityServer.Data;
using CustomIdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CustomIdentityServer;

public class UserIdentityService
{
    private readonly IdentityDbContext _identityDbContext;
    private readonly PasswordEncrypt _passwordEncrypt;
    private readonly IJwtUtils _jwtUtils;
    private readonly IMemoryCache _cache;

    public UserIdentityService(IdentityDbContext identityDbContext, 
        PasswordEncrypt passwordEncrypt, 
        IJwtUtils jwtUtils,
        IMemoryCache cache)
    {
        _identityDbContext = identityDbContext ?? throw new ArgumentNullException(nameof(identityDbContext));
        _passwordEncrypt = passwordEncrypt ?? throw new ArgumentNullException(nameof(passwordEncrypt));
        _jwtUtils = jwtUtils ?? throw new ArgumentNullException(nameof(jwtUtils));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
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
       return user.Password == regenerate;
    }

    public async Task<string> LoginAsync(User user, string password)
    {
        if (!PasswordIsValid(user, password))
            throw new InvalidOperationException("Password is not valid");

        var guid = Guid.NewGuid();
        var code = guid.ToString("N");

        _identityDbContext.UserLogins.Add(new UserLogin() { UserId = user.Id, LoginAt = DateTime.UtcNow, Id = guid});
        await _identityDbContext.SaveChangesAsync();

        return code;
    }

    public async ValueTask<string> GetAccessTokenAsync(string code, string clientSecret)
    {
        if (!Guid.TryParse(code, out var parsedCode))
            throw new ArgumentException("Code is invalid");

        if (!_cache.TryGetValue(code, out User user))
        {
            var loginUser = await _identityDbContext.UserLogins
                .Include(x=>x.User)
                .ThenInclude(x=>x.Roles)
                .ThenInclude(x=>x.Permissions)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == parsedCode);

            if (loginUser is null)
                throw new InvalidOperationException("User Login Session not found");

            user = loginUser.User;

            _cache.Set(code, user, absoluteExpirationRelativeToNow: TimeSpan.FromMinutes(5));
        }

        var jwt = _jwtUtils.GenerateToken(user, clientSecret);
        return jwt;
    }
}
