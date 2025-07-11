﻿using Xreeple.Bukalemun.Services.Models;

namespace Xreeple.Bukalemun.Services.Abstractions;

public interface ICamouflageService
{
    void Create(string store, string tableName, string primaryKey, string columnName, string value);
    Uncamouflaged? Get(string store, string tableName, string primaryKey, string columnName);
}
