# czech-identifiers
A project to parse and validate various identifiers used in Czech Republic (e.g. birth number, IČO..).

## Design principles
Basically if you ever worked in a bank, you know you often get a garbage as an input. You get an IBAN where you should get BIC, you get wrong invalid data all the time. 

* Principle 0: **Each identifier must be able to say whether it is a valid or invalid one**
* Principle 1: **No matter what kind of garbage you get, you can't reject it. Invalid identifier can be constructed as easily as a valid identifier.**
  * When I get '1954' as an account number, I must keep it. Use all important properties `IsValid` (possibly `HasStandardForm`) to check if you should ignore.
  * When you limit yourself to only valid identifiers, there is trouble what to do with invalid ones. Use nulls? Use null object? I find both forms unappealing, because in many cases, I must do some processing later, e.g. I have a czech account number where I expect IBAN. Such conversion is possible.
* Principle 2: **Don't just say valid/invalid. Offer properties to get data from the identifier**
  * Example: Birth number has a date of birth, IČO has a number + checksum. These pieces of identifier can be retrieved using properties, as long as the identifier `HasStandardForm`.
* Principle 3: **Identifier class can be created using a standard form of an identifier**
  * While identifiers can be written in many forms, it is responsibility of parser. The identifier class itself will have a constructor/factory method that accepts a single standard form of an identifier. For example account number prefix and account number must be "clearly divided" according to [decree 169/2011 §5 (2)](https://www.zakonyprolidi.cz/cs/2011-169#p5), but everyone writes it with a dash and standard form is a dash, although some parser could accept a smiley face character ☺ (it't a distinct).
  * This way, I can create both valid identifiers (from the input sources) and valid. Though I am wondering whether a factory method wouldn't be a better option.
* Principle 4: **Offer parsers**
  * Offer parsers that convert user input to standard format used by identifiers as best as it can. This is low priority.

# IČO (IdentificationNumber)
IČO is an 8 digit identifier of a legal person in a czech republic. It is used for companies and for self-employed persons. The IČO consists of 7 number digits and eigth check digit.

You can fing companies at https://justce.cz and self-employed persons at http://www.rzp.cz.

It seems that official sources describing validation algorithm are sparse/nonexistent when it comes to the validation of IČO, so I ended up with
https://phpfashion.com/jak-overit-platne-ic-a-rodne-cislo.
