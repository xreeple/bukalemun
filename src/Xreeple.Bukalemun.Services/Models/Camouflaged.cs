﻿namespace Xreeple.Bukalemun.Services.Models;

public class Camouflaged
{
    public string Store { get; set; } = null!;
    public string TableName { get; set; } = null!;
    public string PrimaryKey { get; set; } = null!;
    public string ColumnName { get; set; } = null!;
    public byte[]? Encrypted { get; set; }
    public string? Hashed { get; set; }
}
