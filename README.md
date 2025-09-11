# Bukalemun

![Unit Tests](https://github.com/xreeple/bukalemun/actions/workflows/dotnet-tests.yml/badge.svg)

Bukalemun 🦎 – Flexible and Secure Data Encryption Library for .NET
Bukalemun is a lightweight and flexible encryption library for .NET applications, designed to securely store sensitive and personal data in databases.

It was built to enhance data privacy and help comply with regulations such as GDPR and KVKK. With real-time encryption/decryption operations, it offers an ideal balance between performance and security.

## 📦 Packages
| Package | Release | Preview | Downloads |
|---------|--------|---------|-----------|
|Xreeple.Bukalemun|[![NuGet](https://img.shields.io/nuget/v/Xreeple.Bukalemun)](https://www.nuget.org/packages/Xreeple.Bukalemun/)|[![NuGet](https://img.shields.io/nuget/vpre/Xreeple.Bukalemun)](https://www.nuget.org/packages/Xreeple.Bukalemun/)|[![NuGet](https://img.shields.io/nuget/dt/Xreeple.Bukalemun)](https://www.nuget.org/packages/Xreeple.Bukalemun/)|
|Xreeple.Bukalemun.Data|[![NuGet](https://img.shields.io/nuget/v/Xreeple.Bukalemun.Data)](https://www.nuget.org/packages/Xreeple.Bukalemun.Data/)|[![NuGet](https://img.shields.io/nuget/vpre/Xreeple.Bukalemun.Data)](https://www.nuget.org/packages/Xreeple.Bukalemun.Data/)|[![NuGet](https://img.shields.io/nuget/dt/Xreeple.Bukalemun.Data)](https://www.nuget.org/packages/Xreeple.Bukalemun.Data/)|
|Xreeple.Bukalemun.Postgresql|[![NuGet](https://img.shields.io/nuget/v/Xreeple.Bukalemun.Postgresql)](https://www.nuget.org/packages/Xreeple.Bukalemun.Postgresql/)|[![NuGet](https://img.shields.io/nuget/vpre/Xreeple.Bukalemun.Postgresql)](https://www.nuget.org/packages/Xreeple.Bukalemun.Postgresql/)|[![NuGet](https://img.shields.io/nuget/dt/Xreeple.Bukalemun.Postgresql)](https://www.nuget.org/packages/Xreeple.Bukalemun.Postgresql/)|
|Xreeple.Bukalemun.Providers|[![NuGet](https://img.shields.io/nuget/v/Xreeple.Bukalemun.Providers)](https://www.nuget.org/packages/Xreeple.Bukalemun.Providers/)|[![NuGet](https://img.shields.io/nuget/vpre/Xreeple.Bukalemun.Providers)](https://www.nuget.org/packages/Xreeple.Bukalemun.Providers/)|[![NuGet](https://img.shields.io/nuget/dt/Xreeple.Bukalemun.Providers)](https://www.nuget.org/packages/Xreeple.Bukalemun.Providers/)|
|Xreeple.Bukalemun.Services|[![NuGet](https://img.shields.io/nuget/v/Xreeple.Bukalemun.Services)](https://www.nuget.org/packages/Xreeple.Bukalemun.Services/)|[![NuGet](https://img.shields.io/nuget/vpre/Xreeple.Bukalemun.Services)](https://www.nuget.org/packages/Xreeple.Bukalemun.Services/)|[![NuGet](https://img.shields.io/nuget/dt/Xreeple.Bukalemun.Services)](https://www.nuget.org/packages/Xreeple.Bukalemun.Services/)|
|Xreeple.Bukalemun.DependencyInjectionExtensions|[![NuGet](https://img.shields.io/nuget/v/Xreeple.Bukalemun.DependencyInjectionExtensions)](https://www.nuget.org/packages/Xreeple.Bukalemun.DependencyInjectionExtensions/)|[![NuGet](https://img.shields.io/nuget/vpre/Xreeple.Bukalemun.DependencyInjectionExtensions)](https://www.nuget.org/packages/Xreeple.Bukalemun.DependencyInjectionExtensions/)|[![NuGet](https://img.shields.io/nuget/dt/Xreeple.Bukalemun.DependencyInjectionExtensions)](https://www.nuget.org/packages/Xreeple.Bukalemun.DependencyInjectionExtensions/)|
|Xreeple.Bukalemun.Masking|[![NuGet](https://img.shields.io/nuget/v/Xreeple.Bukalemun.Masking)](https://www.nuget.org/packages/Xreeple.Bukalemun.Masking/)|[![NuGet](https://img.shields.io/nuget/vpre/Xreeple.Bukalemun.Masking)](https://www.nuget.org/packages/Xreeple.Bukalemun.Masking/)|[![NuGet](https://img.shields.io/nuget/dt/Xreeple.Bukalemun.Masking)](https://www.nuget.org/packages/Xreeple.Bukalemun.Masking/)|

## 🚀 Features
- AES-based symmetric encryption support
- Customizable encryption algorithms
- Column-level encryption (e.g., name, surname, email, etc.)
- Masked data presentation support (e.g., ****4567)
- Minimal integration via attribute-based usage
- Extendable key management system

## 🎯 Target Use Cases

- Encrypting personal data (e.g., name, national ID, phone number, email, etc.)
- Enhancing data security at the application layer
- Providing masked data to minimize data leakage risk

## 💾 Install

The Xreeple.Bukalemun.DependencyInjectionExtensions library also provides other packages. Therefore, to get started, simply add the Xreeple.Bukalemun.DependencyInjectionExtensions library to your project.

```bash
dotnet add package Xreeple.Bukalemun.DependencyInjectionExtensions
```

### Use PostgreSQL

Currently, only PostgreSQL databases are supported. If you are using PostgreSQL, you should install this package.

```bash
dotnet add package Xreeple.Bukalemun.Postgresql
```

## Quick Start

The IServiceCollection extension should be used for dependency management.

```csharp
using Xreeple.Bukalemun.DependencyInjectionExtensions.Extensions;

builder.Services.AddBukalemun(builder.Configuration);
```

### With PostgreSQL

The UseNpgsql extension must be used for PostgreSQL. The default schema is the "public" schema. The default Connection String key is the "DefaultConnection" key. You can specify the schema using the "schema" parameter.

```csharp
using Xreeple.Bukalemun.DependencyInjectionExtensions.Extensions;

builder.Services.AddBukalemun(builder.Configuration).UseNpgsql();
```

### Configurations

The "Store" definition is mandatory in the configuration section. At least one store must be defined. Store definitions can be made as needed.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "Bukalemun": {
    "Stores": {
      "Default": {
        "EncryptKey": ""
      }
    }
  }
}
```

### Simple usage

IBukalemun is available through DI.

```csharp
private readonly IBukalemun _bukalemun;

await _bukalemun.CamouflageAsync("Store", "Table", "Key", "Column", "Value");
```

### Transactional usage

TransactionScope can be used for transactional use.

```csharp
using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
{
    await bukalemun.CamouflageAsync("Store", "Table", "Key", "Column", "Value");

    // Other transactions
    // ...

    scope.Complete();
}
```

## 📄 License
MIT License
