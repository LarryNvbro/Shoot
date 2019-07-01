using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

[Beebyte.Obfuscator.Rename("NVSCMPEKS")]
public static class NVCrypt 
{
    public static string ENCRYPTAPIKEY;
    public static string DECRYPTAPIKEY;
    public static string DECRYPTJSONKEY;
    public static string CryptDataKey;

    static NVCrypt()
    {
        Init();
    }

    [Beebyte.Obfuscator.ObfuscateLiterals]
    static private void Init()
    {
        ENCRYPTAPIKEY = "0402172018)^)@0987654321";
        DECRYPTAPIKEY = "1234567890@)!$0402172018";

        DECRYPTJSONKEY = "1234567890@)!$0329162018";
        CryptDataKey = "0329162018)^)@0987654321";
    }

    static public RijndaelManaged rijndael = new RijndaelManaged();

    // AES/CBC/PKCS5Padding Base64
    // 40 char
    public static string GetSHA1HashString(string str)
    {
        byte[] encrytArray = new SHA1CryptoServiceProvider().ComputeHash(UTF8Encoding.UTF8.GetBytes(str));
        //byte[] encrytArray = SHA1.Create().ComputeHash(UTF8Encoding.UTF8.GetBytes(str));

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < encrytArray.Length; ++i)
        {
            sb.Append(encrytArray[i].ToString("x2"));
        }
        return sb.ToString();
    }

    // 32 char
    public static string GetMD5HashString(byte[] buffer)
    {
        byte[] encrytArray = new MD5CryptoServiceProvider().ComputeHash(buffer);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < encrytArray.Length; ++i)
        {
            sb.Append(encrytArray[i].ToString("x2"));
        }
        return sb.ToString();
    }

    // 32 char
    public static string GetMD5HashString(Stream stream)
    {
        byte[] encrytArray = new MD5CryptoServiceProvider().ComputeHash(stream);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < encrytArray.Length; ++i)
        {
            sb.Append(encrytArray[i].ToString("x2"));
        }
        return sb.ToString();
    }

    // 32 char
    public static string GetMD5HashString(string str)
    {
        return GetMD5HashString(UTF8Encoding.UTF8.GetBytes(str));
    }



    public static string AesEncryptBase64(string toEncrypt, string key)
    {
        if (string.IsNullOrEmpty(toEncrypt)) return null;

        byte[] bytes = UTF8Encoding.UTF8.GetBytes(toEncrypt);
        return Convert.ToBase64String(AesEncrypt(bytes, key, 256));
    }

    public static string AesDecryptBase64(string toDecrypt, string key)
    {
        if (string.IsNullOrEmpty(toDecrypt)) return null;

        toDecrypt = toDecrypt.Replace(" ", "+");
        int mod4 = toDecrypt.Length % 4;
        if (mod4 > 0)
            toDecrypt += new string('=', 4 - mod4);

        byte[] bytes = Convert.FromBase64String(toDecrypt);
        return UTF8Encoding.UTF8.GetString(AesDecrypt(bytes, key, 256));
    }

    public static string Base64Encrypt(string str)
    {
        byte[] bytes = UTF8Encoding.UTF8.GetBytes(str);
        return Convert.ToBase64String(bytes);
    }

    public static string Base64Decrypt(string str)
    {
        byte[] bytes = Convert.FromBase64String(str);
        return UTF8Encoding.UTF8.GetString(bytes);
    }

    public static byte[] AesEncrypt(byte[] toEncrypt, string key, int keySize = 256)
    {
        //RijndaelManaged rDel = new RijndaelManaged();
        rijndael.KeySize = keySize;
        rijndael.BlockSize = 128;
        rijndael.Mode = CipherMode.CBC;
        rijndael.Padding = PaddingMode.PKCS7;
        rijndael.Key = Encoding.Default.GetBytes(key);
        //rijndael.IV = UTF8Encoding.UTF8.GetBytes(key.Substring(0, 16));
        rijndael.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        ICryptoTransform cTransform = rijndael.CreateEncryptor();
        return cTransform.TransformFinalBlock(toEncrypt, 0, toEncrypt.Length);
    }
    public static byte[] AesDecrypt(byte[] toDecrypt, string key, int keySize = 256)
    {
        //RijndaelManaged rDel = new RijndaelManaged();
        rijndael.KeySize = keySize;
        rijndael.BlockSize = 128;
        rijndael.Mode = CipherMode.CBC;
        rijndael.Padding = PaddingMode.PKCS7;
        rijndael.Key = Encoding.Default.GetBytes(key);
        //rijndael.IV = UTF8Encoding.UTF8.GetBytes(key.Substring(0, 16));
        rijndael.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        ICryptoTransform cTransform = rijndael.CreateDecryptor();
        return cTransform.TransformFinalBlock(toDecrypt, 0, toDecrypt.Length);
    }
}
