using CryptoNet;
using Xreeple.Bukalemun.Providers.Abstractions;

namespace Xreeple.Bukalemun.Providers;
public class CryptoProvider : ICryptoProvider
{
    public byte[]? Encrypt(string key, string content)
    {
        var encryptClient = new CryptoNetAes(key);

        var encrypt = encryptClient.EncryptFromString(content);

        return encrypt;
    }
}
