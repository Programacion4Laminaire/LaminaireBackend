using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using SharedKernel.Abstractions.Encript;


namespace SharedKernel.Helpers;
public class AesEncryptor:IDataEncryptor
{

    private readonly byte[] _key;


    public AesEncryptor(string keyBase64)
    {


        if (string.IsNullOrEmpty(keyBase64))
            throw new InvalidOperationException("La clave AES no puede ser nula o vacía.");

        try
        {
            // Solo se encarga de convertir y validar la clave recibida
            _key = Convert.FromBase64String(keyBase64);
        }
        catch (FormatException)
        {
            throw new ArgumentException("La clave debe ser una cadena Base64 válida.");
        }

        if (_key.Length != 32)
            throw new ArgumentException("La Key debe ser de 32 bytes (256 bits) para AES-256.");
    }
    



    public string Encrypt(string plainText)
    {

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = _key;

            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;


            aesAlg.GenerateIV();
            byte[] iv = aesAlg.IV;

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, iv);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] encryptedContent;
            using (var msEncrypt = new System.IO.MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                }
                encryptedContent = msEncrypt.ToArray();
            }


            byte[] combined = new byte[iv.Length + encryptedContent.Length];
            Buffer.BlockCopy(iv, 0, combined, 0, iv.Length);
            Buffer.BlockCopy(encryptedContent, 0, combined, iv.Length, encryptedContent.Length);


            return Convert.ToBase64String(combined);
        }
    }

    public string Decrypt(string cipherTextBase64)
    {

        byte[] combinedBytes = Convert.FromBase64String(cipherTextBase64);

        using (Aes aesAlg = Aes.Create())
        {

            aesAlg.Key = _key;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;


            int ivLength = aesAlg.BlockSize / 8; // 16 bytes para AES
            if (combinedBytes.Length < ivLength)
                throw new CryptographicException("El texto cifrado es demasiado corto para contener un IV válido.");

            byte[] iv = new byte[ivLength];
            Buffer.BlockCopy(combinedBytes, 0, iv, 0, ivLength);

            int cipherLength = combinedBytes.Length - ivLength;
            byte[] cipherBytes = new byte[cipherLength];
            Buffer.BlockCopy(combinedBytes, ivLength, cipherBytes, 0, cipherLength);

            // 4. Crear el desencriptador con la clave y el IV extraído
            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, iv);

            // 5. Desencriptar
            using (var msDecrypt = new System.IO.MemoryStream(cipherBytes))
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (var srDecrypt = new System.IO.StreamReader(csDecrypt, Encoding.UTF8))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }
}