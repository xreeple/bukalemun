namespace Xreeple.Bukalemun.Providers.Abstractions;
public interface ICryptoProvider
{
    byte[]? Encrypt(string key, string content);
}
