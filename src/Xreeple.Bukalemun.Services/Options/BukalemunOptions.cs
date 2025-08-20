namespace Xreeple.Bukalemun.Services.Options;

internal sealed class BukalemunOptions
{
    public Dictionary<string, Store> Stores { get; set; } = [];

    public class Store
    {
        public string EncryptKey { get; set; } = null!;
    }
}
