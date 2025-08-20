namespace Xreeple.Bukalemun.Providers.Abstractions;

internal interface ICryptoProvider
{
    byte[] Encrypt(string key, string content);
    string Decrypt(string key, byte[] content);
}
