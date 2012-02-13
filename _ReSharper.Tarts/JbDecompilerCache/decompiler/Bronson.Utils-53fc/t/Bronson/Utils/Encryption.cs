// Type: Bronson.Utils.Encryption
// Assembly: Bronson.Utils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\@Projects\Tarts\packages\Bronson.Utils.1.0.0.57\lib\net40\Bronson.Utils.dll

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Bronson.Utils
{
  public class Encryption
  {
    private static string constants = "0123456789abcdefghijklmnopqrstuvwxyz";
    private const string constAES_Password = "Wrj78hEiUm3";
    private const string constAES_Salt_Password2 = "bL945ghyQ7B";
    private const string constAES_InitialVector = "yefp9s2PqYY8Nwev";

    static Encryption()
    {
    }

    private static string AES_Encrypt(string PlainText, string Password, string Salt, string HashAlgorithm, int PasswordIterations, string InitialVector, int KeySize)
    {
      byte[] bytes1 = Encoding.ASCII.GetBytes(InitialVector);
      byte[] bytes2 = Encoding.ASCII.GetBytes(Salt);
      byte[] bytes3 = Encoding.UTF8.GetBytes(PlainText);
      byte[] bytes4 = new PasswordDeriveBytes(Password, bytes2, HashAlgorithm, PasswordIterations).GetBytes(KeySize / 8);
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      rijndaelManaged.Mode = CipherMode.CBC;
      ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(bytes4, bytes1);
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write);
      cryptoStream.Write(bytes3, 0, bytes3.Length);
      cryptoStream.FlushFinalBlock();
      byte[] inArray = memoryStream.ToArray();
      memoryStream.Close();
      cryptoStream.Close();
      return Convert.ToBase64String(inArray);
    }

    private static string AES_Decrypt(string CipherText, string Password, string Salt, string HashAlgorithm, int PasswordIterations, string InitialVector, int KeySize)
    {
      try
      {
        byte[] bytes1 = Encoding.ASCII.GetBytes(InitialVector);
        byte[] bytes2 = Encoding.ASCII.GetBytes(Salt);
        byte[] buffer = Convert.FromBase64String(CipherText);
        byte[] bytes3 = new PasswordDeriveBytes(Password, bytes2, HashAlgorithm, PasswordIterations).GetBytes(KeySize / 8);
        RijndaelManaged rijndaelManaged = new RijndaelManaged();
        rijndaelManaged.Mode = CipherMode.CBC;
        ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(bytes3, bytes1);
        MemoryStream memoryStream = new MemoryStream(buffer);
        CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read);
        byte[] numArray = new byte[buffer.Length];
        int count = cryptoStream.Read(numArray, 0, numArray.Length);
        memoryStream.Close();
        cryptoStream.Close();
        return Encoding.UTF8.GetString(numArray, 0, count);
      }
      catch
      {
        return CipherText;
      }
    }

    private static string ConvertToNBit(long lValue, int iMaxDigitValue)
    {
      if (iMaxDigitValue > Bronson.Utils.Encryption.constants.Length)
        throw new Exception("This engine supports only " + (object) Bronson.Utils.Encryption.constants.Length + " values");
      string str = string.Empty;
      while (lValue > 0L)
      {
        str = Bronson.Utils.Encryption.constants.Substring((int) (lValue % (long) iMaxDigitValue), 1) + str;
        lValue /= (long) iMaxDigitValue;
      }
      if (str.Length == 0)
        str = Bronson.Utils.Encryption.constants.Substring(0, 1);
      return str;
    }

    private static long ConvertFromNBit(string sRepresentation, int iMaxDigitValue)
    {
      if (iMaxDigitValue > Bronson.Utils.Encryption.constants.Length)
        throw new Exception("This engine supports only " + (object) Bronson.Utils.Encryption.constants.Length + " values");
      long num1 = 0L;
      int num2 = 0;
      sRepresentation = sRepresentation.ToLower();
      while (sRepresentation.Length > 0)
      {
        string str = sRepresentation.Substring(0, 1);
        if (!Bronson.Utils.Encryption.constants.Contains(str))
          throw new Exception("Value can't be converted to this representation - incorrect character at position " + num2.ToString());
        num1 = num1 * (long) iMaxDigitValue + (long) Bronson.Utils.Encryption.constants.IndexOf(str);
        sRepresentation = sRepresentation.Substring(1);
        ++num2;
      }
      return num1;
    }

    private static string SHA_Encrypt(string content)
    {
      SHA1Managed shA1Managed = new SHA1Managed();
      if (string.IsNullOrEmpty(content))
        return content;
      Convert.ToBase64String(shA1Managed.ComputeHash(Encoding.ASCII.GetBytes(content)));
      return Convert.ToBase64String(Encoding.ASCII.GetBytes(content));
    }

    private static string SHA_Decrypt(string content)
    {
      try
      {
        if (!string.IsNullOrEmpty(content))
          return Encoding.ASCII.GetString(Convert.FromBase64String(content));
        else
          return content;
      }
      catch
      {
        return content;
      }
    }

    public static string EncodeMD5(string content)
    {
      return Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.Default.GetBytes(content)));
    }

    public static string Encrypt(string content)
    {
      if (string.IsNullOrEmpty(content))
        return content;
      try
      {
        return Bronson.Utils.Encryption.SHA_Encrypt(content);
      }
      catch
      {
        return content;
      }
    }

    public static string Decrypt(string content)
    {
      if (string.IsNullOrEmpty(content))
        return content;
      try
      {
        return Bronson.Utils.Encryption.SHA_Decrypt(content);
      }
      catch
      {
        return content;
      }
    }

    public static string EncryptInteger(int objID)
    {
      long lValue1 = (long) (objID * 3);
      string str1 = Bronson.Utils.Encryption.ConvertToNBit(lValue1, 36);
      long lValue2 = (long) new System.Random().Next(5, 1200);
      string str2 = Bronson.Utils.Encryption.ConvertToNBit(lValue2, 36);
      string str3 = Bronson.Utils.Encryption.ConvertToNBit(lValue1 * lValue2 % 1201L, 36);
      return str1 + "-" + str2 + "-" + str3;
    }

    public static int DecryptInteger(string code)
    {
      string[] strArray = code.Split(new char[1]
      {
        '-'
      });
      if (strArray.Length != 3)
        return -1;
      long num1 = Bronson.Utils.Encryption.ConvertFromNBit(strArray[0].Trim(), 36);
      long num2 = Bronson.Utils.Encryption.ConvertFromNBit(strArray[1].Trim(), 36);
      if (Bronson.Utils.Encryption.ConvertFromNBit(strArray[2].Trim(), 36) != num1 * num2 % 1201L)
        return -1;
      else
        return Convert.ToInt32(num1 / 3L);
    }
  }
}
