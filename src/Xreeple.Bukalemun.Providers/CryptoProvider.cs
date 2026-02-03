using System.Text;
using CryptoNet;
using Xreeple.Bukalemun.Providers.Abstractions;

namespace Xreeple.Bukalemun.Providers;

internal class CryptoProvider : ICryptoProvider
{
    public byte[] Encrypt(string key, string content)
    {
        byte[] _key = Convert.FromBase64String(key);
        byte[] _iv = RandomByteArray(16);

        byte[] contentBytes = Encoding.UTF8.GetBytes(content);
        var encrypt = new CryptoNetAes(_key, _iv).EncryptFromBytes(contentBytes);

        encrypt = [.. _iv, .. encrypt];

        return encrypt;
    }

    public string Decrypt(string key, byte[] content)
    {
        byte[] _key = Convert.FromBase64String(key);
        byte[] _iv = content[..16];
        byte[] _content = content[16..];

        var decryptedBytes = new CryptoNetAes(_key, _iv).DecryptToBytes(_content);
        var decrypt = Encoding.UTF8.GetString(decryptedBytes);

        return decrypt;
    }

    private static byte[] RandomByteArray(int size)
    {
        byte[] b = new byte[size];
        new Random().NextBytes(b);

        return b;
    }
}
