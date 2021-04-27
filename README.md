# OptionRecords

This is an an Option type implementaion using C# 9 records which closely follows the [Option<'T>](https://fsharp.github.io/fsharp-core-docs/reference/fsharp-core-fsharpoption-1.html) API as found in FSharp.Core 

Operations against Options can be accessed through static functions or extension methods

Examples of the entire API can be found in the [tests](https://github.com/Jordan466/OptionRecords/blob/main/src/OptionRecords.Tests/OptionTest.cs)

## Some
Construct a new Some
```csharp
new Some<int>(42);

Option.Some(42);
```

## None
Construct a new None
```csharp
new None<int>();

Option.None<int>();
```

## Map
Applies a map over the value of an Option
```csharp
Option.Map(new Some<int>(42), x => x * 2); //Some 84

new Some<int>(42).Map(x => x * 2); //Some 84
```

## Bind
Applies a binder over an Option
```csharp
Option.Bind(new Some<int>(42), x => new Some<int>(x * 2)); //Some 84

new Some<int>(42).Bind(x => new Some<int>(x * 2)); //Some 84
```

## Get
Gets the value on an Option
```csharp
Option.Get(new Some<int>(42)); //42

new Some<int>(42).Get(); //42

new None<int>().Get(); //ArgumentException
```

## Default Value
Gets the value on an Option, or returns the default if None
```csharp
Option.DefaultValue(new Some<int>(42), 0); //42

new Some<int>(42).DefaultValue(0); //42

new None<int>().DefaultValue(0).Dump(); //0
```

## IsSome
Checks if the Option is Some
```csharp
Option.IsSome(new Some<int>(42)); //True

new Some<int>(42).IsSome(); //True

new None<int>().IsSome(); //False
```

## IsNone
Checks if the Option is None
```csharp
Option.IsNone(new Some<int>(42)); //False

new Some<int>(42).IsNone(); //False

new None<int>().IsNone(); //True
```

## OfObj
Converts the object into an Option, retuns None if the object is null
```csharp
Option.OfObj("not a null string"); // Some("not a null string")

Option.OfObj<string>(null); // None<string>()
```

## ToObj
Returns the object from an Option, returns null if the Option is null
```csharp
Option.ToObj(new Some<string>("not a null string")); // "not a null string"

new Some<string>("not a null string").ToObj(); // "not a null string"

Option.ToObj(new None<string>()); // null

new None<string>().ToObj(); // null
```

## Equality
```csharp
new None<int>() == new None<int>() //True

new None<int>() == new Some<int>(42) //False

new Some<int>(42) == new Some<int>(42) //True

new Some<int>(42) == new Some<int>(99) //False
```
