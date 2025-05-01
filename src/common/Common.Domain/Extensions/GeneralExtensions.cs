using System.Security.Cryptography;
using System.Text;

namespace Common.Domain.Extensions;
public static class GeneralExtensions
{
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using HMACSHA512 hmac = new();
        passwordSalt = hmac.Key;
        passwordHash = hmac
                .ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}


