using Microsoft.Extensions.Options;
using NoteVault.BLL.Common;
using NoteVault.BLL.Constants;
using NoteVault.BLL.Interfaces;
using System.Security.Cryptography;

namespace NoteVault.BLL.Services
{
    public class PasswordHasher : IPasswordHasher
    {
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

            return $"${PasswordHasherConstants.AlgorithmId}${PasswordHasherConstants.SubAlgorithmId}${PasswordHasherConstants.IterationsParamName}={_options.Iterations},{PasswordHasherConstants.KeySizeParamName}={_options.KeySize}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        public bool Verify(string password, string passwordHash)
        {
            var parts = passwordHash.Split(PasswordHasherConstants.PhcSeparator);
            if (parts.Length != PasswordHasherConstants.PhcPartsCount)
            {
                return false;
            }

            if (parts[PasswordHasherConstants.AlgorithmIndex] != PasswordHasherConstants.AlgorithmId || parts[PasswordHasherConstants.SubAlgorithmIndex] != PasswordHasherConstants.SubAlgorithmId)
            {
                return false;
            }

            var paramsPart = parts[PasswordHasherConstants.ParamsIndex].Split(PasswordHasherConstants.ParamSeparator);
            int iterations = _options.Iterations;
            int keySize = _options.KeySize;

            foreach (var param in paramsPart)
            {
                var keyValue = param.Split(PasswordHasherConstants.KeyValueSeparator);
                if (keyValue.Length != PasswordHasherConstants.ParamPartsCount)
                {
                    continue;
                }

                if (keyValue[PasswordHasherConstants.KeyIndex] == PasswordHasherConstants.IterationsParamName && int.TryParse(keyValue[PasswordHasherConstants.ValueIndex], out var parsedIterations))
                {
                    iterations = parsedIterations;
                }
                else if (keyValue[PasswordHasherConstants.KeyIndex] == PasswordHasherConstants.KeySizeParamName && int.TryParse(keyValue[PasswordHasherConstants.ValueIndex], out var parsedKeySize))
                {
                    keySize = parsedKeySize;
                }
            }
            
            var salt = Convert.FromBase64String(parts[PasswordHasherConstants.SaltIndex]);
            var hash = Convert.FromBase64String(parts[PasswordHasherConstants.HashIndex]);

            var inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithm, keySize);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }
    }
}
