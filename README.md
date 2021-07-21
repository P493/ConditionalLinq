# ConditionalLinq
Conditional add/use/apply query condition by chaining.

This repository is not for a library, it's more like a code style may or may not help us to write cleaner code.

# Why
Did we feel that the syntax of using query condition in the back end is redundant and fragmented, like this linq:

```csharp
if (!string.IsNullOrWhiteSpace(name))
{
    query = query.Where(customerAndOrder => customerAndOrder.CustomerName.Contains(name));
}
if (int.TryParse(stateStr, out int state))
{
    query = query.Where(customerAndOrder => customerAndOrder.State == state);
}
```

Most times we check if user submitted the form field and then use it as query condition (field validation is another topic, we won't discuss it here),
we could try to chain it together for more readable and simplification.

# How
Method chain style for **linq to sql**:

```csharp
query
  .MayWhereString(name, v => t => t.CustomerName.Contains(v))
  .MayWhereInt(stateStr, v => t => t.State == v)
```

A csharp extension method example (detailed in [ConditionalQueryable.cs](./ConditionalQueryable.cs)):

```csharp
public static IQueryable<T> MayWhereString<T>(this IQueryable<T> source, string value, Func<string, Expression<Func<T, bool>>> predicateBuilder)
{
    if (string.IsNullOrWhiteSpace(value))
    {
        return source;
    }
    return source.Where(predicateBuilder(value));
}
```

There are several questions.

## Naming convention
Simplified:

`v => t => t.CustomerName.Contains(v)`

Or detailed:

`name => customerAndOrder => customerAndOrder.CustomerName.Contains(name)`

`v` means the `value` we'll used as query condition and `t` means some `Anonymous Type`, if we get used to this style and understand the underlying method usage,
then the simplified one is simpler.

## Two arrow
Why not use one lambda method as predicate:

`(v, t) => t.CustomerName.Contains(v)`

First, look at a normal linq style extension method example (detailed in [ConditionalEnumerable.cs](./ConditionalEnumerable.cs)):

```csharp
public static IEnumerable<T> MayWhereString<T>(this IEnumerable<T> source, string value, Func<string, T, bool> predicate)
{
    if (string.IsNullOrWhiteSpace(value))
    {
        return source;
    }
    return source.Where(t => predicate(value, t));
}
```

As mentioned before, the two arrow one is **linq to sql** style, which is a lambda method return a `predicate` lambda method (so the parameter name is `predicateBuilder`).
It's the easiest way I can think of to make the linq translated right, try to run [ConditionalQueryable.cs](./ConditionalQueryable.cs) and [ConditionalEnumerable.cs](./ConditionalEnumerable.cs) in some code and observe the generated sql to find the difference.

## Common extension library
No, can't write a library to match all cases, it's a style here.

We could use this style everywhere with enumeration function if we like.

`Enumerable.Where` for `csharp`

`Stream.filter` for `java`

`Array.prototype.filter` for `javascript`

Even for raw sql concatenation like pseudo code below, and a implementation [ConditionalSqlBuilder.cs](./ConditionalSqlBuilder.cs)

```
if (shouldUseThis(field)) {
    sql = sql + someRawSql;
}
```

Finally, see [Program.cs](./Program.cs) for demo
