using System;
using System.Collections.Generic;

namespace OptionRecords
{
    public abstract record Option<T>();
    public record Some<T>(T Value) : Option<T>;
    public record None<T>() : Option<T>;
    public static class Option
    {
        public static Some<T> Some<T>(T value) => new Some<T>(value);

        public static None<T> None<T>() => new None<T>();

        public static Option<U> Bind<T, U>(Option<T> option, Func<T, Option<U>> binder) => option switch
        {
            Some<T> some => binder(some.Value),
            _ => new None<U>()
        };

        public static bool Contains<T>(Option<T> option, T value) => option switch
        {
            Some<T> some => some.Value.Equals(value),
            _ => false
        };

        public static int Count<T>(Option<T> option) => option switch
        {
            Some<T> some => 1,
            _ => 0
        };

        public static T DefaultValue<T>(Option<T> option, T value) => option switch
        {
            Some<T> some => some.Value,
            _ => value
        };

        public static T DefaultWith<T>(Option<T> option, Func<T> defThunk) => option switch
        {
            Some<T> some => some.Value,
            _ => defThunk()
        };

        public static bool Exists<T>(Option<T> option, Predicate<T> predicate) => option switch
        {
            Some<T> some => predicate(some.Value),
            _ => false
        };

        public static Option<T> Filter<T>(Option<T> option, Predicate<T> predicate) => option switch
        {
            Some<T> some => predicate(some.Value) switch
            {
                true => some,
                false => new None<T>()
            },
            _ => new None<T>()
        };

        public static Option<T> Flatten<T>(Option<Option<T>> option) => option switch
        {
            Some<T> outter => outter switch
            {
                Some<T> inner => inner,
                _ => new None<T>()
            },
            _ => new None<T>()
        };

        public static Option<TState> Fold<T, TState>(Option<T> option, Func<TState, T, TState> fold, TState state) => option switch
        {
            Some<T> some => new Some<TState>(fold(state, some.Value)),
            _ => new None<TState>()
        };

        public static Option<TState> FoldBack<T, TState>(Option<T> option, Func<TState, T, TState> fold, TState state)
            => Option.Fold(option, fold, state);

        public static bool ForAll<T>(Option<T> option, Predicate<T> predicate) => option switch
        {
            Some<T> some => predicate(some.Value),
            _ => true
        };

        public static T Get<T>(Option<T> option) => option switch
        {
            Some<T> some => some.Value,
            _ => throw new ArgumentException("The option value was None")
        };

        public static T Get<T, TException>(Option<T> option, Func<TException> exception) where TException : Exception, new()
            => option switch
            {
                Some<T> some => some.Value,
                _ => throw exception()
            };

        public static bool IsNone<T>(Option<T> option) => option switch
        {
            None<T> none => true,
            _ => false
        };

        public static bool IsSome<T>(Option<T> option) => option switch
        {
            Some<T> some => true,
            _ => false
        };

        public static void Iter<T>(Option<T> option, Action<T> action)
        {
            if (option is Some<T> some)
                action(some.Value);
        }

        public static Option<U> Map<T, U>(Option<T> option, Func<T, U> mapping) => option switch
        {
            Some<T> some => new Some<U>(mapping(some.Value)),
            _ => new None<U>()
        };

        public static Option<U> Map2<T1, T2, U>((Option<T1>, Option<T2>) options, Func<T1, T2, U> mapping) => options switch
        {
            (Some<T1> one, Some<T2> two) some => new Some<U>(mapping(one.Value, two.Value)),
            _ => new None<U>()
        };

        public static Option<U> Map2<T1, T2, U>(Option<T1> option1, Option<T2> option2, Func<T1, T2, U> mapping) => Option.Map2((option1, option2), mapping);

        public static Option<U> Map3<T1, T2, T3, U>((Option<T1>, Option<T2>, Option<T3>) options, Func<T1, T2, T3, U> mapping) => options switch
        {
            (Some<T1> one, Some<T2> two, Some<T3> three) some => new Some<U>(mapping(one.Value, two.Value, three.Value)),
            _ => new None<U>()
        };

        public static Option<U> Map3<T1, T2, T3, U>(Option<T1> option1, Option<T2> option2, Option<T3> option3, Func<T1, T2, T3, U> mapping) => Option.Map3((option1, option2, option3), mapping);

        public static Option<T> ofNullable<T>(Nullable<T> value) where T : struct => value.HasValue switch
        {
            true => new Some<T>(value.Value),
            false => new None<T>()
        };

        public static Option<T> OfObj<T>(T value) => (value != null) switch
        {
            true => new Some<T>(value),
            false => new None<T>()
        };

        public static Option<T> OrElse<T>(Option<T> option, Option<T> ifNone) => option switch
        {
            Some<T> some => some,
            _ => ifNone
        };

        public static Option<T> OrElseWith<T>(Option<T> option, Func<Option<T>> ifNoneThunk) => option switch
        {
            Some<T> some => some,
            _ => ifNoneThunk()
        };

        public static T[] ToArray<T>(Option<T> option) => option switch
        {
            Some<T> some => new T[] { some.Value },
            _ => new T[0]
        };

        public static IEnumerable<T> ToEnumerable<T>(Option<T> option)
        {
            if (option is Some<T> some)
                yield return some.Value;
        }

        public static List<T> ToList<T>(Option<T> option) => option switch
        {
            Some<T> some => new List<T> { some.Value },
            _ => new List<T>()
        };

        public static Nullable<T> ToNullable<T>(Option<T> option) where T : struct => option switch
        {
            Some<T> some => new Nullable<T>(some.Value),
            _ => new Nullable<T>()
        };

        public static T ToObj<T>(Option<T> option) where T : class => option switch
        {
            Some<T> some => some.Value,
            _ => null
        };
    }

    public static class OptionExt
    {
        public static Option<U> Bind<T, U>(this Option<T> option, Func<T, Option<U>> binder) => Option.Bind(option, binder);

        public static bool Contains<T>(this Option<T> option, T value) => Option.Contains(option, value);

        public static int Count<T>(this Option<T> option) => Option.Count(option);

        public static T DefaultValue<T>(this Option<T> option, T value) => Option.DefaultValue(option, value);

        public static T DefaultWith<T>(this Option<T> option, Func<T> defThunk) => Option.DefaultWith(option, defThunk);

        public static bool Exists<T>(this Option<T> option, Predicate<T> predicate) => Option.Exists(option, predicate);

        public static Option<T> Filter<T>(this Option<T> option, Predicate<T> predicate) => Option.Filter(option, predicate);

        public static Option<T> Flatten<T>(this Option<Option<T>> option) => Option.Flatten(option);

        public static Option<TState> Fold<T, TState>(this Option<T> option, Func<TState, T, TState> fold, TState state) => Option.Fold(option, fold, state);

        public static Option<TState> FoldBack<T, TState>(this Option<T> option, Func<TState, T, TState> fold, TState state) => Option.FoldBack(option, fold, state);

        public static bool ForAll<T>(this Option<T> option, Predicate<T> predicate) => Option.ForAll(option, predicate);

        public static T Get<T>(this Option<T> option) => Option.Get(option);

        public static T Get<T, TException>(this Option<T> option, Func<TException> exception) where TException : Exception, new() => Option.Get(option, exception);

        public static bool IsNone<T>(this Option<T> option) => Option.IsNone(option);

        public static bool IsSome<T>(this Option<T> option) => Option.IsSome(option);

        public static void Iter<T>(this Option<T> option, Action<T> action) => Option.Iter(option, action);

        public static Option<U> Map<T, U>(this Option<T> option, Func<T, U> mapping) => Option.Map(option, mapping);

        public static Option<U> Map2<T1, T2, U>(this (Option<T1>, Option<T2>) options, Func<T1, T2, U> mapping) => Option.Map2(options, mapping);

        public static Option<U> Map3<T1, T2, T3, U>(this (Option<T1>, Option<T2>, Option<T3>) options, Func<T1, T2, T3, U> mapping) => Option.Map3(options, mapping);

        public static Option<T> OrElse<T>(this Option<T> option, Option<T> ifNone) => Option.OrElse(option, ifNone);

        public static Option<T> OrElseWith<T>(this Option<T> option, Func<Option<T>> ifNoneThunk) => Option.OrElseWith(option, ifNoneThunk);

        public static T[] ToArray<T>(this Option<T> option) => Option.ToArray(option);

        public static IEnumerable<T> ToEnumerable<T>(Option<T> option) => Option.ToEnumerable(option);

        public static List<T> ToList<T>(this Option<T> option) => Option.ToList(option);

        public static Nullable<T> ToNullable<T>(this Option<T> option) where T : struct => Option.ToNullable(option);

        public static T ToObj<T>(this Option<T> option) where T : class => Option.ToObj(option);
    }
}
