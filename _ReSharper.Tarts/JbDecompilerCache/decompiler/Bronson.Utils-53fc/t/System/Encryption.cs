// Type: System.Encryption
// Assembly: Bronson.Utils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\@Projects\Tarts\packages\Bronson.Utils.1.0.0.57\lib\net40\Bronson.Utils.dll

using Bronson.Utils;

namespace System
{
  public static class Encryption
  {
    public static string Encrypt(this string value)
    {
      return Encryption.Encrypt(value);
    }

    public static string Decrypt(this string value)
    {
      return Encryption.Decrypt(value);
    }

    public static string Hash(this string value)
    {
      return Encryption.EncodeMD5(value);
    }

    public static string EncryptInteger(this int value)
    {
      return Encryption.EncryptInteger(value);
    }

    public static int DecryptInteger(this string value)
    {
      return Encryption.DecryptInteger(value);
    }
  }
}
