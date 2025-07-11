using CryptoNet;
using System.Diagnostics;
using Xreeple.Bukalemun.Providers.Abstractions;

namespace Xreeple.Bukalemun.Providers;
public class CryptoProvider : ICryptoProvider
{
    private byte[] GetByteArray(int sizeInKb)
    {
        Random rnd = new Random();
        byte[] b = new byte[sizeInKb]; // convert kb to byte
        rnd.NextBytes(b);
        return b;

    }
    public byte[]? Encrypt(string key, string content)
    {
        string _key = "V8vl8wrpMAjjKqq02wsjYVctgUC5GnPVfuEGZ/d3VZA=";
        string _iv = "fUmz8IYi+0tGtRpBvxCX1g==";

        byte[] key_ba = Convert.FromBase64String(_key);
        byte[] iv_ba = Convert.FromBase64String(_iv);

        List<string> items = new List<string>();

        for (int i = 0; i < 10; i++)
        {
            iv_ba = GetByteArray(16);

            var encryptClient = new CryptoNetAes(key_ba, iv_ba);

            var encrypt = encryptClient.EncryptFromString(content);

            encrypt = [.. iv_ba, .. encrypt];

            var base64 = System.Convert.ToBase64String(encrypt);

            items.Add(base64);

            Debug.WriteLine("Encrypt: " + base64);
        }

        foreach (var item in items)
        {
            var itemContent = Convert.FromBase64String(item);

            var iv_ba2 = itemContent[..16];
            var content_ba = itemContent[16..];

            var encryptClient = new CryptoNetAes(key_ba, iv_ba2);

            var decrypt = encryptClient.DecryptToString(content_ba);


            Debug.WriteLine("Decrypt: " + decrypt);
        }

        return null;
    }
}
