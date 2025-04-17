using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

public interface IPasswordService
{
    string Hash(string password);
    bool Verfiy(string passwordHash, string password);
}
public class PasswordService: IPasswordService
{
    private const int SaltSize = 128 / 8;
    private const int KeySize = 256 / 8;
    private const int Iterations = 10000;
    private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
    private const char Delimiter = ';';

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);
        return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    public bool Verfiy(string passwordHash, string password)
    {
        var elements = password.Split(Delimiter);
        var salt = Convert.FromBase64String(elements[0]);
        var hash = Convert.FromBase64String(elements[1]);

        var hashInput = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);
        return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    }
}