namespace NoteVault.BLL.Common
{
    public class PasswordHashingOptions
    {
        public const string SectionName = "PasswordHashingOptions";

        public int SaltSize { get; set; } = 16;
        public int KeySize { get; set; } = 32;
        public int Iterations { get; set; } = 100000;
    }
}
