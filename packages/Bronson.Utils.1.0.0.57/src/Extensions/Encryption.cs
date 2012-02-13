namespace System
{
    public static class Encryption
    {

        #region Encryption

        public static string Encrypt(this string value)
        {
            return Bronson.Utils.Encryption.Encrypt(value);
        }

        public static string Decrypt(this string value)
        {
            return Bronson.Utils.Encryption.Decrypt(value);
        }

        public static string Hash(this string value)
        {
            return Bronson.Utils.Encryption.EncodeMD5(value);
        }

        public static string EncryptInteger(this int value)
        {
            return Bronson.Utils.Encryption.EncryptInteger(value);
        }

        public static int DecryptInteger(this string value)
        {
            return Bronson.Utils.Encryption.DecryptInteger(value);
        }

        #endregion
    }
}
