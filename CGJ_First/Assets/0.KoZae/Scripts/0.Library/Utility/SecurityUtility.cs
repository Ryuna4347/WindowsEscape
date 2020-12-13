using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class SecurityUtility
{
    public static string EncryptData(string _data,string _key)
    {
        PasswordDeriveBytes secretKey = new PasswordDeriveBytes(_key,Encoding.UTF8.GetBytes(_key.Length.ToString()));
        RijndaelManaged aes = new RijndaelManaged
        {
            BlockSize = 128,
            KeySize = 256,
            Mode = CipherMode.CBC,
            Padding = PaddingMode.PKCS7,
            Key = secretKey.GetBytes(32),
            IV = secretKey.GetBytes(16)
        };

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key,aes.IV);
        byte[] sourceData = Encoding.UTF8.GetBytes(_data);

        MemoryStream mStream = new MemoryStream();
        CryptoStream cStream = new CryptoStream(mStream,encryptor,CryptoStreamMode.Write);

        cStream.Write(sourceData,0,sourceData.Length);
        cStream.FlushFinalBlock();

        byte[] resultData = mStream.ToArray();

        mStream.Close();
        cStream.Close();

        Array.Reverse(resultData);

        return Convert.ToBase64String(resultData);
    }

    public static string DecryptData(string _data,string _key)
    {
        PasswordDeriveBytes secretKey = new PasswordDeriveBytes(_key,Encoding.UTF8.GetBytes(_key.Length.ToString()));
        RijndaelManaged aes = new RijndaelManaged
        {
            BlockSize = 128,
            KeySize = 256,
            Mode = CipherMode.CBC,
            Padding = PaddingMode.PKCS7,
            Key = secretKey.GetBytes(32),
            IV = secretKey.GetBytes(16)
        };

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key,aes.IV);
        byte[] sourceData = Convert.FromBase64String(_data);

        Array.Reverse(sourceData);

        MemoryStream mStream = new MemoryStream(sourceData);
        CryptoStream cStream = new CryptoStream(mStream,decryptor,CryptoStreamMode.Read);

        int resultLength = cStream.Read(sourceData,0,sourceData.Length);

        mStream.Close();
        cStream.Close();

        return Encoding.UTF8.GetString(sourceData,0,resultLength);
    }
}
