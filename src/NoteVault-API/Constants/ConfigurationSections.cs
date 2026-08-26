using NoteVault.BLL.Common;

namespace NoteVault_API.Constants
{
    public static class ConfigurationSections
    {
        public const string PasswordHashing = nameof(PasswordHashingOptions);
        public const string Jwt = nameof(JwtOptions);
    }
}
