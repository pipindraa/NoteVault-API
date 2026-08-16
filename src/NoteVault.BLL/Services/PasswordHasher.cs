using Microsoft.Extensions.Options;
using NoteVault.BLL.Common;
using NoteVault.BLL.Interfaces;
using System.Security.Cryptography;

namespace NoteVault.BLL.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int PasswordHashPartsCount = 2;
        private const int SaltIndex = 0;
        private const int HashIndex = 1;
        private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

        private readonly PasswordHashingOptions _options;

        public PasswordHasher(IOptions<PasswordHashingOptions> options)
        {
            _options = options.Value;
        }

        public string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(_options.SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _options.Iterations, HashAlgorithm, _options.KeySize);

            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public bool Verify(string password, string passwordHash)
        {
            var parts = passwordHash.Split('.', PasswordHashPartsCount);
            if (parts.Length != PasswordHashPartsCount)
            {
                return false;
            }
            
            var salt = Convert.FromBase64String(parts[SaltIndex]);
            var hash = Convert.FromBase64String(parts[HashIndex]);

            var inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _options.Iterations, HashAlgorithm, _options.KeySize);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }
    }
}
