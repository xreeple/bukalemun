using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Xreeple.Bukalemun.Data.Abstractions;
using Xreeple.Bukalemun.Providers.Abstractions;
using Xreeple.Bukalemun.Services.Abstractions;
using Xreeple.Bukalemun.Services.Options;
using Assert = NUnit.Framework.Assert;
using CamouflagedEntity = Xreeple.Bukalemun.Data.Entites.Camouflaged;

namespace Xreeple.Bukalemun.Services.Tests
{
    [TestFixture]
    public class CamouflageServiceTests
    {
        private Mock<ICamouflageRepository>? _camouflageRepoMock = null!;
        private Mock<ICryptoProvider>? _cryptoProviderMock = null!;
        private IOptions<BukalemunOptions>? _options = null!;
        private ICamouflageService _camouflageService = null!;

        [SetUp]
        public void Setup()
        {
            _camouflageRepoMock = new Mock<ICamouflageRepository>();
            _cryptoProviderMock = new Mock<ICryptoProvider>();
            _options = Microsoft.Extensions.Options.Options.Create(
                new BukalemunOptions
                {
                    Stores = new Dictionary<string, BukalemunOptions.Store>
                    {
                        {
                            "store1",
                            new BukalemunOptions.Store { EncryptKey = "dummyKey" }
                        },
                    },
                }
            );

            _camouflageService = new CamouflageService(
                _options,
                _camouflageRepoMock.Object,
                _cryptoProviderMock.Object
            );
        }

        [Test]
        public async Task CreateAsync_ShouldCallRepositoryWithEncryptedValue()
        {
            // Arrange
            string store = "store1";
            string table = "table1";
            string key = "key1";
            string column = "column1";
            string value = "secret";

            var encryptedValue = new byte[] { 1, 2, 3 };
            var storeKey = _options!.Value.Stores[store].EncryptKey;

            _cryptoProviderMock!.Setup(c => c.Encrypt(storeKey, value)).Returns(encryptedValue);
            _camouflageRepoMock!
                .Setup(r => r.UpsertAsync(It.IsAny<CamouflagedEntity>()))
                .ReturnsAsync(true);

            // Act
            await _camouflageService!.CreateAsync(store, table, key, column, value);

            // Assert
            _camouflageRepoMock.Verify(
                r =>
                    r.UpsertAsync(
                        It.Is<CamouflagedEntity>(c =>
                            c.Store == store
                            && c.Table == table
                            && c.Key == key
                            && c.Column == column
                            && c.Encrypted != null
                            && c.Encrypted.SequenceEqual(encryptedValue)
                        )
                    ),
                Times.Once
            );
        }

        [Test]
        public async Task GetAsync_SingleKey_SingleColumn_ShouldReturnValidResult()
        {
            // Arrange
            string store = "store1";
            string table = "table1";
            string key = "key1";
            string column = "column1";
            string expectedDecryptedValue = "decryptedValue";

            var camouflaged = new CamouflagedEntity
            {
                Store = store,
                Table = table,
                Key = key,
                Column = column,
                Encrypted = [1, 2, 3],
            };

            _camouflageRepoMock!
                .Setup(r =>
                    r.GetAsync(
                        store,
                        table,
                        It.Is<string[]>(arr => arr.SequenceEqual(new[] { key })),
                        It.Is<string[]>(arr => arr.SequenceEqual(new[] { column }))
                    )
                )
                .ReturnsAsync([camouflaged]);

            var storeKey = _options!.Value.Stores[store].EncryptKey;
            _cryptoProviderMock!
                .Setup(c => c.Decrypt(storeKey, It.IsAny<byte[]>()))
                .Returns(expectedDecryptedValue);

            // Act
            var result = await _camouflageService!.GetAsync(store, table, key, column);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(key, result!.Key);
            Assert.AreEqual(column, result.Name);
            Assert.AreEqual(expectedDecryptedValue, result.Value);
        }

        [Test]
        public async Task GetAsync_MultipleKeys_SingleColumn_ShouldReturnMultipleResults()
        {
            // Arrange
            string store = "store1";
            string table = "table1";
            string[] keys = ["key1", "key2"];
            string column = "column1";

            var camouflaged1 = new CamouflagedEntity
            {
                Store = store,
                Table = table,
                Key = "key1",
                Column = column,
                Encrypted = [1, 2, 3],
            };
            var camouflaged2 = new CamouflagedEntity
            {
                Store = store,
                Table = table,
                Key = "key2",
                Column = column,
                Encrypted = [4, 5, 6],
            };

            _camouflageRepoMock!
                .Setup(r =>
                    r.GetAsync(
                        store,
                        table,
                        It.Is<string[]>(arr => arr.SequenceEqual(keys)),
                        It.Is<string[]>(arr => arr.SequenceEqual(new[] { column }))
                    )
                )
                .ReturnsAsync([camouflaged1, camouflaged2]);

            _cryptoProviderMock!
                .Setup(c => c.Decrypt(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Returns("decrypted");

            // Act
            var results = await _camouflageService!.GetAsync(store, table, keys, column);

            // Assert
            Assert.AreEqual(2, results.Count());
            Assert.IsTrue(results.All(r => r.Name == column));
        }

        [Test]
        public async Task GetAsync_SingleKey_MultipleColumns_ShouldReturnMultipleResults()
        {
            // Arrange
            string store = "store1";
            string table = "table1";
            string key = "key1";
            string[] columns = ["column1", "column2"];

            var camouflaged1 = new CamouflagedEntity
            {
                Store = store,
                Table = table,
                Key = key,
                Column = "column1",
                Encrypted = [1, 2, 3],
            };
            var camouflaged2 = new CamouflagedEntity
            {
                Store = store,
                Table = table,
                Key = key,
                Column = "column2",
                Encrypted = [4, 5, 6],
            };

            _camouflageRepoMock!
                .Setup(r =>
                    r.GetAsync(
                        store,
                        table,
                        It.Is<string[]>(arr => arr.SequenceEqual(new[] { key })),
                        It.Is<string[]>(arr => arr.SequenceEqual(columns))
                    )
                )
                .ReturnsAsync([camouflaged1, camouflaged2]);

            _cryptoProviderMock!
                .Setup(c => c.Decrypt(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Returns("decrypted");

            // Act
            var results = await _camouflageService!.GetAsync(store, table, key, columns);

            // Assert
            Assert.AreEqual(2, results.Count());
            Assert.IsTrue(results.All(r => r.Key == key));
        }

        [Test]
        public async Task GetAsync_WithNullEncrypted_ShouldReturnNullValue()
        {
            // Arrange
            string store = "store1";
            string table = "table1";
            string[] keys = ["key1"];
            string[] columns = ["column1"];

            var camouflaged = new CamouflagedEntity
            {
                Store = store,
                Table = table,
                Key = "key1",
                Column = "column1",
                Encrypted = null,
            };

            _camouflageRepoMock!
                .Setup(r => r.GetAsync(store, table, It.IsAny<string[]>(), It.IsAny<string[]>()))
                .ReturnsAsync([camouflaged]);

            // Act
            var results = await _camouflageService!.GetAsync(store, table, keys, columns);
            var result = results.First();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Value);
            _cryptoProviderMock!.Verify(
                c => c.Decrypt(It.IsAny<string>(), It.IsAny<byte[]>()),
                Times.Never
            );
        }
    }
}
