namespace NoteVault.BLL.Constants
{
    public class PasswordHasherConstants
    {
        public const char PhcSeparator = '$';
        public const int PhcPartsCount = 6;
        public const int AlgorithmIndex = 1;   
        public const int SubAlgorithmIndex = 2;
        public const int ParamsIndex = 3;
        public const int SaltIndex = 4;
        public const int HashIndex = 5;

        public const string AlgorithmId = "pbkdf2";
        public const string SubAlgorithmId = "sha256";

        public const char ParamSeparator = ',';
        public const char KeyValueSeparator = '=';
        public const int ParamPartsCount = 2;
        public const int KeyIndex = 0;
        public const int ValueIndex = 1;

        public const string IterationsParamName = "i";
        public const string KeySizeParamName = "l";
    }
}
