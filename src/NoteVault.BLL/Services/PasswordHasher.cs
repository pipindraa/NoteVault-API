using Microsoft.Extensions.Options;
using NoteVault.BLL.Common;
using NoteVault.BLL.Interfaces;
using System.Security.Cryptography;

namespace NoteVault.BLL.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

        private const char PhcSeparator = '$';
        private const int PhcPartsCount = 6;
        private const int AlgorithmIndex = 1;
        private const int SubAlgorithmIndex = 2;
        private const int ParamsIndex = 3;
        private const int SaltIndex = 4;
        private const int HashIndex = 5;

        private const string AlgorithmId = "pbkdf2";
        private const string SubAlgorithmId = "sha256";

        private readonly PasswordHashingOptions _options;

        private const char ParamSeparator = ',';
        private const char KeyValueSeparator = '=';
        private const int ParamPartsCount = 2;
        private const int KeyIndex = 0;
        private const int ValueIndex = 1;

        private const string IterationsParamName = "i";
        private const string KeySizeParamName = "l";

        public PasswordHasher(IOptions<PasswordHashingOptions> options)
        {
            _options = options.Value;
        }

        public string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(_options.SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _options.Iterations, HashAlgorithm, _options.KeySize);

            return $"${AlgorithmId}${SubAlgorithmId}${IterationsParamName}={_options.Iterations},{KeySizeParamName}={_options.KeySize}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        public bool Verify(string password, string passwordHash)
        {
            var parts = passwordHash.Split(PhcSeparator);
            if (parts.Length != PhcPartsCount)
            {
                return false;
            }

            if (parts[AlgorithmIndex] != AlgorithmId || parts[SubAlgorithmIndex] != SubAlgorithmId)
            {
                return false;
            }

            var paramsPart = parts[ParamsIndex].Split(ParamSeparator);
            int iterations = _options.Iterations;
            int keySize = _options.KeySize;

            foreach (var param in paramsPart)
            {
                var keyValue = param.Split(KeyValueSeparator);
                if (keyValue.Length != ParamPartsCount)
                {
                    continue;
                }

                if (keyValue[KeyIndex] == IterationsParamName && int.TryParse(keyValue[ValueIndex], out var parsedIterations))
                {
                    iterations = parsedIterations;
                }
                else if (keyValue[KeyIndex] == KeySizeParamName && int.TryParse(keyValue[ValueIndex], out var parsedKeySize))
                {
                    keySize = parsedKeySize;
                }
            }
            
            var salt = Convert.FromBase64String(parts[SaltIndex]);
            var hash = Convert.FromBase64String(parts[HashIndex]);

            var inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _options.Iterations, HashAlgorithm, _options.KeySize);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }
    }
}
