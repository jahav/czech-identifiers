# Structure

The library consists from two basic types:

* Identifiers
* Patterns

## Identifiers
Identifiers are a `structs` that contain an idenfifier number. Just because you have an identifier doesn't mean that idenfifier is 
valid, the identifier might be invalid. Every identifier implements `IIdentifier` with a `IsValid` getter property that indicates whether 
the idenfier is valid or not.

The identifiers also support `IFormattable` interface that allows you to format the idenfier using a specific pattern, depending on a passed format parameter.

## Patterns
Pattern is a string representation of an idenfier. Because basically all idenfiers are all written as strings, you need an object that parses string into the identifier and also some object that formats the idenfier object back into the string. Since both of there operations (parsing/formatting) are connected (the string pattern of the input/output), I use only one that does both. Every pattern implements `IPattern`.

Pattern class itself is sealed and not extendable, its main job is to provide static properties with one or more standard patterns that are commonly using for writing the idenfier. Most of the time, there is a `StandardPattern` property on the pattern type for an identifer. If you need to add a new pattern, just implement the `IPattern` interface and use an instance of your pattern.