using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class AesHelper
{
    private readonly byte[] Key;
    private readonly byte[] IV;

    public AesHelper()
    {
        // Simular exactamente lo que hacía .NET Framework viejo
        var rawKey = Encoding.ASCII.GetBytes("Lami Nair"); // 9 bytes
        Key = new byte[16]; // Rijndael espera 16
        Array.Copy(rawKey, Key, rawKey.Length); // copia los 9 primeros
        // los 7 restantes quedan en 0 → igual que legacy

        IV = Encoding.ASCII.GetBytes("Devjoker7.37hAES"); // 16 bytes
    }

    public string Encrypt(string plainText)
    {
        using var rij = new RijndaelManaged
        {
            Key = Key,
            IV = IV,
            Mode = CipherMode.CBC,
            Padding = PaddingMode.PKCS7
        };

        var encryptor = rij.CreateEncryptor(rij.Key, rij.IV);

        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs, Encoding.ASCII)) // 🔑 ASCII igual al legacy
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public string Decrypt(string cipherText)
    {
        var buffer = Convert.FromBase64String(cipherText);

        using var rij = new RijndaelManaged
        {
            Key = Key,
            IV = IV,
            Mode = CipherMode.CBC,
            Padding = PaddingMode.PKCS7
        };

        var decryptor = rij.CreateDecryptor(rij.Key, rij.IV);

        using var ms = new MemoryStream(buffer);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs, Encoding.ASCII); // 🔑 ASCII igual al legacy
        return sr.ReadToEnd();
    }
}
