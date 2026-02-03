using System.Security.Cryptography;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Xreeple.Bukalemun.Providers.Tests;

[TestFixture()]
public class CryptoProviderTests
{
    private CryptoProvider _cryptoProvider = null!;
    private string _key = null!;

    [SetUp]
    public void Setup()
    {
        _cryptoProvider = new CryptoProvider();

        byte[] keyBytes = new byte[32];
        new Random(42).NextBytes(keyBytes);
        _key = Convert.ToBase64String(keyBytes);
    }

    [Test]
    public void Encrypt_And_Decrypt_Should_Return_Original_Content()
    {
        // Arrange
        string originalContent = "Hello World!";

        // Act
        var encrypted = _cryptoProvider.Encrypt(_key, originalContent);
        var decrypted = _cryptoProvider.Decrypt(_key, encrypted);

        // Assert
        Assert.AreEqual(originalContent, decrypted);
    }

    [Test]
    public void Encrypt_And_Decrypt_Turkish_Characters_Should_Return_Original_Content()
    {
        // Arrange
        string originalContent = "Şifreleme İçin Ğüzel Çözüm Önerisi";

        // Act
        var encrypted = _cryptoProvider.Encrypt(_key, originalContent);
        var decrypted = _cryptoProvider.Decrypt(_key, encrypted);

        // Assert
        Assert.AreEqual(originalContent, decrypted);
    }

    [Test]
    public void Encrypted_Content_Should_Contain_IV()
    {
        // Arrange
        string content = "Hello World!";

        // Act
        var encrypted = _cryptoProvider.Encrypt(_key, content);

        // Assert: İlk 16 byte IV olmalı
        Assert.AreEqual(16, encrypted[..16].Length);
        Assert.Greater(encrypted.Length, 16);
    }

    [Test]
    public void Encrypt_Should_Produce_Different_Results_For_Same_Input()
    {
        // Arrange
        string content = "Hello World!";

        // Act
        var encrypted1 = _cryptoProvider.Encrypt(_key, content);
        var encrypted2 = _cryptoProvider.Encrypt(_key, content);

        // Assert: IV farklı olacağı için sonuçlar da farklı olmalı
        Assert.AreNotEqual(Convert.ToBase64String(encrypted1), Convert.ToBase64String(encrypted2));
    }

    [Test]
    public void Decrypt_With_Wrong_Key_Should_Fail()
    {
        // Arrange
        string content = "Hello World!";
        var encrypted = _cryptoProvider.Encrypt(_key, content);

        byte[] wrongKeyBytes = new byte[32];
        new Random(100).NextBytes(wrongKeyBytes);
        string wrongKey = Convert.ToBase64String(wrongKeyBytes);

        // Act & Assert
        Assert.Throws<CryptographicException>(() => _cryptoProvider.Decrypt(wrongKey, encrypted));
    }
}
