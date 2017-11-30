# czech-identifiers 
A project to parse and validate various identifiers used in Czech Republic (e.g. birth number, IČO..).

[![Build status](https://ci.appveyor.com/api/projects/status/aqrjgivpxk33w71k?svg=true)](https://ci.appveyor.com/project/jahav/czech-identifiers)  [![codecov](https://codecov.io/gh/jahav/czech-identifiers/branch/master/graph/badge.svg)](https://codecov.io/gh/jahav/czech-identifiers)

## Usage
There are three basic classes so far:
* `BirthNumber` - identifier assigned to every natural person born in Czech Republic. If you are foreigner, you can ask for it. It is key identifier for medical care.
* `IdentificationNumber` - IČO - identifier of legal persons.
* `AccountNumber` - Czech bank account number

```
    ParseResult<AccountNumber> parseResult = AccountNumberPattern.StandardPattern.Parse("00012-2548/0300");
    if (!parseResult.Success)
    {
        Console.WriteLine("Account number couldn't be parsed.");
        return;
    }

    Console.WriteLine("Account was successfully parsed, but we don't know yet if it is valid or not.");

    AccountNumber accountNumber = parseResult.Value;
    if (accountNumber.IsValid)
    {
        Console.WriteLine("Account number is valid.");
    }
    else
    {
        Console.WriteLine("Account number is invalid.");
    }
```

## Design principles
Basically if you ever worked in a bank, you know you often get a garbage as an input. You get an IBAN where you should get BIC, you get wrong invalid data all the time.

* Principle 0: **Identifiers are immutable structs**
* Principle 1: **Each identifier must be able to say whether it is a valid or invalid one**
* Principle 2: **Don't just say valid/invalid. Offer properties to get data from the identifier**
  * Example: Birth number has a date of birth, IČO has a number + checksum. These pieces of identifier can be retrieved using properties.
* Principle 4: **Offer parsers for various formats**
  * Offer parsers that convert user input to standard format used by identifiers as best as it can. This is low priority.
  * I am using pattern similar to NodaTime - there are classes [identifier]Pattern that contain patterns used for parsing and formatting.

# IČO (IdentificationNumber)
IČO is an 8 digit identifier of a legal person in a czech republic. It is used for companies and for self-employed persons. The IČO consists of 7 number digits and eigth check digit.

You can fing companies at https://justce.cz and self-employed persons at http://www.rzp.cz.

It seems that official sources describing validation algorithm are sparse/nonexistent when it comes to the validation of IČO, so I ended up with
https://phpfashion.com/jak-overit-platne-ic-a-rodne-cislo.

# Building NuGet package
* Open VS dev console `Program Files (x86)/Microsoft Visual Studio/2017/Community/Common7/Tools/VsDevCmd.bat`
* Go to `Identitifiers` directory (there is csproj there)
* run `msbuild /t:pack /p:Configuration=Release`
* in `bin/Release` will be the `Identifiers.Czech.*.nupkg` package
