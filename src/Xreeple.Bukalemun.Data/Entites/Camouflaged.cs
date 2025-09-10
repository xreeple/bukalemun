namespace Xreeple.Bukalemun.Data.Entites;

internal class Camouflaged
{
    public string Store { get; set; } = null!;
    public string Table { get; set; } = null!;
    public string Key { get; set; } = null!;
    public string Column { get; set; } = null!;
    public byte[]? Encrypted { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
