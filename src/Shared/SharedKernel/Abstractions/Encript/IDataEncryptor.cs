namespace SharedKernel.Abstractions.Encript
{

    public interface IDataEncryptor
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherTextBase64);
    }
}