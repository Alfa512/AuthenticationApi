namespace AuthenticationApi.Common.Contracts.Services
{
    public interface ICryptoService
    {
        string Hash(string data);
        string EncryptStringAes(string plainText, string sharedSecret);
        string DecryptStringAes(string cipherText, string sharedSecret);
    }
}