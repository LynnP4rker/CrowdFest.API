using System.Security.Cryptography;

///Jabbarzadeh, J. (2023). Cyber Security with C# and AES | TutorialsEU. [online]
public interface IEncryptionService
{
    string Encrypt(string input);
    string Decrypt(string input);
}

public class EncryptionService: IEncryptionService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public EncryptionService(IConfiguration configuration)
    {
        _key = Convert.FromBase64String(configuration["Encryption:Key"]);
        _iv = Convert.FromBase64String(configuration["Encryption:IV"]);
    }

    public string Decrypt(string cipheredText)
    {
        string decryptedText = String.Empty;
        byte[] cipherTextBytes = Convert.FromBase64String(cipheredText);
        using Aes aes = Aes.Create();
        ICryptoTransform decryptor = aes.CreateDecryptor(_key, _iv);
        MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        StreamReader streamReader = new StreamReader(cryptoStream);
        decryptedText = streamReader.ReadToEnd();
                    

        return decryptedText;
    }

    public string Encrypt(string inputText)
    {
        byte[] cipheredText;
        using (Aes aes = Aes.Create())
        {
            ICryptoTransform encryptor = aes.CreateEncryptor(_key, _iv);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(inputText);
                    }

                    cipheredText = memoryStream.ToArray();
                }
            }
        }

        return Convert.ToBase64String(cipheredText);
    }
}