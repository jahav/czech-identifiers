# Identifiers.Czech

Identifiers.Czech is a C# library for creating, parsing and validating various identifiers used in Czech Republic. As of this time, it supports
following identifiers:
* Birth number - this identifier is assigned to ever person born in Czech Republic. It is used extensively in a medical care. The number itself contains a flag indicating whether the person is a man or a woman and also a date of birth.
* Business identifying number - It is a number used for identification of a business, either a company or a person doing business. In czech, it is called IČO - Identifikační číslo osoby.
* Bank account number - account number in the bank. IBAN is not in widespread use, instead we still use a system from 90s. It is possible to transform the account number to an IBAN.

## Quick Start

There are three identifier so far:

* ``BirthNumber`` - birth number.
* ``IdentificationNumber`` - Business identifying number.
* ``AccountNumber`` - bank account number.

All of these classes follow this use pattern:

```
// First choose a pattern from [identifier]Pattern class. Generally, there is always a standard pattern, possibly others.
ParseResult<BirthNumber> parseResult = BirthNumberPattern.StandardPattern.Parse("780123/3540");
if (!parseResult.Success)
{
    Console.WriteLine("Birth number couldn't be parsed.");
    return;
}

Console.WriteLine("Birth number was successfully parsed, but we don't know yet if it is valid or not.");

// Get a parsed value
BirthNumber birthNumber = parseResult.Value;

// check if it is valid, it might be or it might not.
if (birthNumber.IsValid)
{
    Console.WriteLine("Birth number is is valid for a {0} born at {1:d}.", birthNumber.BelongsToWoman ? "woman" : "man", birthNumber.DateOfBirth);
}
else
{
    Console.WriteLine("Birth number is invalid, expected check digit to be {0}.", birthNumber.ExpectedCheckDigit);
}
```
