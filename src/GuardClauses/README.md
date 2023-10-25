# Georgia EPD-IT Guard Clauses Library

This package was created by Georgia EPD-IT to provide simple guard clause methods for our web applications.

This package was inspired by the great [GuardClauses](https://github.com/ardalis/GuardClauses/tree/main) package by
Steve Smith, which has a lot more options and extensibility.

## How to install

[![Nuget](https://img.shields.io/nuget/v/GaEpd.GuardClauses)](https://www.nuget.org/packages/GaEpd.GuardClauses)

To install, search for "GaEpd.GuardClauses" in the NuGet package manager or run the following command:

`dotnet add package GaEpd.GuardClauses`

## How to use

Guard clauses simplify checking for invalid input parameters.

Example usage:

```csharp
public class SomeClass
{
    private readonly string _name;

    public SomeClass(string name)
    {
        _name = Guard.NotNullOrWhiteSpace(name);
    }
}
```

Each clause returns the original value if the conditions are met; otherwise, it throws an exception.

- **NotNull** – ensures a value is not null.
- **NotNullOrWhiteSpace** – ensures a string is not null, empty, or whitespace.
- **ValidLength** – ensures a string has a length between the specified minimum and maximum (inclusive).
- **NotNegative** – ensures an integer is not negative.
- **Positive** – ensures an integer is not zero or negative.
- **RegexMatch** – ensures a string matches the provided regex pattern.
