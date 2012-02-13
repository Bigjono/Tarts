using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Bronson.Utils
{
    public class Encryption
    {

        #region Private Methods

        #region AES
        
        private const string constAES_Password = "Wrj78hEiUm3";
        private const string constAES_Salt_Password2 = "bL945ghyQ7B";
        private const string constAES_InitialVector = "yefp9s2PqYY8Nwev";//always 16 characters

        private static string AES_Encrypt(string PlainText, string Password, string Salt, string HashAlgorithm, int PasswordIterations, string InitialVector, int KeySize)
        {
            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] PlainTextBytes = Encoding.UTF8.GetBytes(PlainText);
            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
            RijndaelManaged SymmetricKey = new RijndaelManaged();
            SymmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(KeyBytes, InitialVectorBytes);
            MemoryStream MemStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] CipherTextBytes = MemStream.ToArray();
            MemStream.Close();
            cryptoStream.Close();
            return System.Convert.ToBase64String(CipherTextBytes);
        }

        private static string AES_Decrypt(string CipherText, string Password, string Salt, string HashAlgorithm, int PasswordIterations, string InitialVector, int KeySize)
        {
            try
            {
                byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
                byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
                byte[] CipherTextBytes = System.Convert.FromBase64String(CipherText);
                PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
                byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
                RijndaelManaged SymmetricKey = new RijndaelManaged();
                SymmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor(KeyBytes, InitialVectorBytes);
                MemoryStream MemStream = new MemoryStream(CipherTextBytes);
                CryptoStream cryptoStream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read);
                byte[] PlainTextBytes = new byte[CipherTextBytes.Length];
                int ByteCount = cryptoStream.Read(PlainTextBytes, 0, PlainTextBytes.Length);
                MemStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(PlainTextBytes, 0, ByteCount);
            }
            catch
            {
                return CipherText;
            }
        }

        #endregion

        #region NBit

        private static string constants = "0123456789abcdefghijklmnopqrstuvwxyz";
        /// <summary>
        /// Convert long value to representation using given maximum value per digit (uses alphabet for missing digit representation)
        /// </summary>
        /// <param name="lValue"></param>
        /// <param name="iBitsPerDigit"></param>
        /// <returns></returns>
        private static string ConvertToNBit(long lValue, int iMaxDigitValue)
        {
            if (iMaxDigitValue > constants.Length)
                throw new Exception("This engine supports only " + constants.Length + " values");
            string retValue = string.Empty;
            while (lValue > 0)
            {
                retValue = constants.Substring((int)(lValue % iMaxDigitValue), 1) + retValue;
                lValue = lValue / iMaxDigitValue;
            }
            if (retValue.Length == 0)
                retValue = constants.Substring(0, 1);
            return retValue;
        }

        /// <summary>
        /// Converts string representation of n-valued digit series to long value
        /// </summary>
        /// <param name="sRepresentation"></param>
        /// <param name="iBitsPerDigit"></param>
        /// <returns></returns>
        private static long ConvertFromNBit(string sRepresentation, int iMaxDigitValue)
        {
            if (iMaxDigitValue > constants.Length)
                throw new Exception("This engine supports only " + constants.Length + " values");
            long retValue = 0;
            int position = 0;
            sRepresentation = sRepresentation.ToLower();
            while (sRepresentation.Length > 0)
            {
                string digit = sRepresentation.Substring(0, 1);
                if (!constants.Contains(digit))
                    throw new Exception("Value can't be converted to this representation - incorrect character at position " + position.ToString());
                retValue = retValue * iMaxDigitValue + (long)constants.IndexOf(digit);
                sRepresentation = sRepresentation.Substring(1);
                position++;
            }
            return retValue;
        }

        #endregion

        #region SHA

        private static string SHA_Encrypt(string content)
        {
            string functionReturnValue = null;
            SHA1Managed shaM = new SHA1Managed();
            if (!string.IsNullOrEmpty(content))
            {
                System.Convert.ToBase64String(shaM.ComputeHash(Encoding.ASCII.GetBytes(content)));
                byte[] eNC_data = ASCIIEncoding.ASCII.GetBytes(content);
                string eNC_str = System.Convert.ToBase64String(eNC_data);
                functionReturnValue = eNC_str;
            }
            else
            {
                return content;
            }
            return functionReturnValue;
        }

        private static string SHA_Decrypt(string content)
        {
            try
            {
                string functionReturnValue = null;
                if (!string.IsNullOrEmpty(content))
                {
                    byte[] dEC_data = System.Convert.FromBase64String(content);
                    string dEC_Str = ASCIIEncoding.ASCII.GetString(dEC_data);
                    functionReturnValue = dEC_Str;
                }
                else
                {
                    return content;
                }
                return functionReturnValue;
            }
            catch { return content; }
        }

        #endregion

        #region MD5

        /// <summary>
        /// Encode string to md5 and return base64 string representing given string
        /// </summary>
        public static string EncodeMD5(string content)
        {
            MD5 md5Class = MD5.Create();
            byte[] encoded = md5Class.ComputeHash(Encoding.Default.GetBytes(content));
            string retVal = Convert.ToBase64String(encoded);
            return retVal;
        }

        #endregion

        #endregion

        public static string Encrypt(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                //return AES_Encrypt(content, constAES_Password, constAES_Salt_Password2, "SHA1", 2, constAES_InitialVector, 256);
                try
                {
                    return SHA_Encrypt(content);
                }
                catch
                {
                    return content;
                }
            }
            else
                return content;
        }
        public static string Decrypt(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                //return AES_Decrypt(content, constAES_Password, constAES_Salt_Password2, "SHA1", 2, constAES_InitialVector, 256);
                try
                {
                    return SHA_Decrypt(content);
                }
                catch
                {
                    return content;
                }
            }
            else
                return content;
        }

        public static string EncryptInteger(int objID)
        {
            long lFirst = objID * 3;
            string first = ConvertToNBit(lFirst, 36);
            System.Random rnd = new System.Random();
            long lSecond = (long)rnd.Next(5, 1200);
            string second = ConvertToNBit(lSecond, 36);
            long lThird = (lFirst * lSecond) % 1201;
            string third = ConvertToNBit(lThird, 36);

            return first + "-" + second + "-" + third;
        }

        public static int DecryptInteger(string code)
        {
            string[] parts = code.Split(new char[] { '-' });
            if (parts.Length == 3)
            {
                long lFirst = ConvertFromNBit(parts[0].Trim(), 36);
                long lSecond = ConvertFromNBit(parts[1].Trim(), 36);
                long lThird = ConvertFromNBit(parts[2].Trim(), 36);
                long lThirdCorrect = (lFirst * lSecond) % 1201;
                if (lThird != lThirdCorrect)
                {
                    return -1;
                }
                else
                {
                    return System.Convert.ToInt32(lFirst / 3);
                }
            }
            else
                return -1;
        }

        

    }
}
