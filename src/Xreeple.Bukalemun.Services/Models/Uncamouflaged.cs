namespace Xreeple.Bukalemun.Services.Models;

public class Uncamouflaged
{
    public required string Store { get; set; }
    public required string TableName { get; set; }
    public required string PrimaryKey { get; set; }
    public required string ColumnName { get; set; }
    public string? Value { get; set; }
}
