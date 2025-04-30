using System.Security.Cryptography;
using System.Text;

namespace Common.Security;

public static class PasswordHasher
{
    public static string ComputeStringToSha256Hash(string plainText)
    {
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plainText));
        StringBuilder stringBuilder = new();
        for (int i = 0; i < bytes.Length; i++)
        {
            stringBuilder.Append(bytes[i].ToString("x2"));
        }
        return stringBuilder.ToString();
    }

    public static bool VerifyPassword(string plainTextPassword, string hashedPassword)
    {
        var computedHash = ComputeStringToSha256Hash(plainTextPassword);
        return computedHash.Equals(hashedPassword, StringComparison.OrdinalIgnoreCase);
    }
}
