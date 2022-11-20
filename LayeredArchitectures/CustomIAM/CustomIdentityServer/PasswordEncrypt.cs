using System.Buffers;
using System.Security.Cryptography;

namespace CustomIdentityServer;

public class PasswordEncrypt
{
    public string GeneratePasswordHashUsingSalt(string password)
    {
        var salt = ArrayPool<byte>.Shared.Rent(16);
        Random.Shared.NextBytes(salt);
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
        byte[] hash = pbkdf2.GetBytes(36);
        Array.Copy(salt, 0, hash, 0, 16);
        var passwordHash = Convert.ToBase64String(hash);
        ArrayPool<byte>.Shared.Return(salt, true);
        
        return passwordHash;
    }

    public byte[] ExtractSaltFromEncryptedPassword(string encryptedPassword)
    {
        if (encryptedPassword.Length != 48)
            throw new ArgumentException("Password has incorrect format");

        byte[] fullPassword = Convert.FromBase64String(encryptedPassword);
        return fullPassword[..16];
    }
}
