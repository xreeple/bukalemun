# Xreeple.Bukalemun.Masking

A flexible and extensible **string masking library** for C#.  
Supports different masking scenarios, customizable rules, and advanced options like compact masking or removing masked characters entirely.

---

## 📦 Installation

Install via NuGet:

```bash
dotnet add package Xreeple.Bukalemun.Masking
```

---

## 🚀 Usage

```csharp
using System;
using Xreeple.Bukalemun.Masking;

class Program
{
    static void Main()
    {
        Console.WriteLine(Mask.Build("helloworld").RevealLast(3).ToString()); 
        // *******rld
    }
}
```

---

## ⚙️ API and Methods

All methods are **chainable**.

---

### General Settings

- **`MaskChar(char c)`**  
  Changes the mask character.  
  ```csharp
  Mask.Build("mehmet").RevealLast(2).MaskChar('#').ToString(); 
  // ######et
  ```

- **`CompactMask(int length)`**  
  Replaces any hidden section with a compact block of the specified mask length.  
  ```csharp
  Mask.Build("mehmet").RevealFirst(2).CompactMask(3).ToString(); 
  // me***
  ```

- **`RemoveMasked(bool remove = true)`**  
  Removes masked characters completely instead of replacing them.  
  ```csharp
  Mask.Build("mehmet").RevealLast(3).RemoveMasked().ToString(); 
  // met
  ```

---

### Masking Rules

- **`RevealFirst(int count)`**  
  Reveals the first `count` characters.  
  ```csharp
  Mask.Build("helloworld").RevealFirst(2).ToString(); 
  // he********
  ```

- **`RevealLast(int count)`**  
  Reveals the last `count` characters.  
  ```csharp
  Mask.Build("helloworld").RevealLast(3).ToString(); 
  // *******rld
  ```

- **`RevealRange(int start, int length)`**  
  Reveals characters in the given range.  
  ```csharp
  Mask.Build("helloworld").RevealRange(2, 4).ToString(); 
  // **llow*****
  ```

- **`RevealInitialsPerWord()`**  
  Reveals only the first letter of each word.  
  ```csharp
  Mask.Build("mehmet emin eker").RevealInitialsPerWord().ToString(); 
  // m***** e*** e***
  ```

- **`RevealIf(Func<char, int, bool> predicate)`**  
  Reveals characters that match a given condition.  
  Example: reveal only digits.  
  ```csharp
  Mask.Build("abc123xyz").RevealIf((ch, i) => char.IsDigit(ch)).ToString();
  // ***123***
  ```

---

### Special Rules

- **`PreserveChars(params char[] chars)`**  
  Ensures the given characters are always preserved (never masked).  
  ```csharp
  Mask.Build("555-123-4567").RevealLast(2).PreserveChars('-').ToString(); 
  // ***-***-**67
  ```

---

## 🧩 Combination Examples

```csharp
// Initials per word + compact mask
Console.WriteLine(
    Mask.Build("mehmet emin eker")
        .RevealInitialsPerWord()
        .CompactMask(3)
        .ToString()
);
// m*** e*** e***


// Removing masked characters instead of replacing
Console.WriteLine(
    Mask.Build("mehmet")
        .RevealLast(2)
        .RemoveMasked()
        .ToString()
);
// et


// Range reveal + preserved characters
Console.WriteLine(
    Mask.Build("TR-2025-ABC")
        .RevealRange(3, 4)
        .PreserveChars('-')
        .ToString()
);
// **-2025-***


// Change mask character + reveal first letters
Console.WriteLine(
    Mask.Build("hello world")
        .RevealInitialsPerWord()
        .MaskChar('#')
        .CompactMask(2)
        .ToString()
);
// h## w##


// Conditional reveal (digits only)
Console.WriteLine(
    Mask.Build("user123data456")
        .RevealIf((ch, i) => char.IsDigit(ch))
        .CompactMask(3)
        .ToString()
);
// u***123***456
```

---

## 📄 License

MIT
