using CryptoNet;
using Xreeple.Bukalemun.Providers.Abstractions;

namespace Xreeple.Bukalemun.Providers;

public class CryptoProvider : ICryptoProvider
{
    public byte[] Encrypt(string key, string content)
    {
        byte[] _key = Convert.FromBase64String(key);
        byte[] _iv = RandomByteArray(16);

        var encrypt = new CryptoNetAes(_key, _iv).EncryptFromString(content);

        encrypt = [.. _iv, .. encrypt];

        return encrypt;
    }

    public string Decrypt(string key, byte[] content)
    {
        byte[] _key = Convert.FromBase64String(key);
        byte[] _iv = content[..16];
        byte[] _content = content[16..];

        var decrypt = new CryptoNetAes(_key, _iv).DecryptToString(_content);

        return decrypt;
    }

    private static byte[] RandomByteArray(int size)
    {
        byte[] b = new byte[size];
        new Random().NextBytes(b);

        return b;
    }
}
