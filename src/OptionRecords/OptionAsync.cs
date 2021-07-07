using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OptionRecords
{
    /// <summary>
    /// Contains operations for working with options
    /// </summary>
    public static partial class Option
    {
        /// <inheritdoc cref="OptionRecords.Option.Bind{T, U}(Option{T}, Func{T, Option{U}})"/>
        public static async Task<Option<U>> Bind<T, U>(Option<T> option, Func<T, Task<Option<U>>> binder) => option switch
        {
            Some<T> some => await binder(some.Value),
            _ => new None<U>()
        };

        /// <inheritdoc cref="OptionRecords.Option.Bind{T, U}(Option{T}, Func{T, Option{U}})"/>
        public static async Task<Option<U>> Bind<T, U>(Task<Option<T>> option, Func<T, Task<Option<U>>> binder) => await option switch
        {
            Some<T> some => await binder(some.Value),
            _ => new None<U>()
        };

        /// <inheritdoc cref="OptionRecords.Option.Contains{T}(Option{T}, T)"/>
        public static async Task<bool> Contains<T>(Task<Option<T>> option, T value) => await option switch
        {
            Some<T> some => some.Value.Equals(value),
            _ => false
        };

        /// <inheritdoc cref="OptionRecords.Option.Count{T}(Option{T})"/>
        public static async Task<int> Count<T>(Task<Option<T>> option) => await option switch
        {
            Some<T> some => 1,
            _ => 0
        };

        /// <inheritdoc cref="OptionRecords.Option.DefaultValue{T}(Option{T}, T)"/>
        public static async Task<T> DefaultValue<T>(Task<Option<T>> option, T value) => await option switch
        {
            Some<T> some => some.Value,
            _ => value
        };

        /// <inheritdoc cref="OptionRecords.Option.DefaultValue{T}(Option{T}, T)"/>
        public static async Task<T> DefaultWith<T>(Task<Option<T>> option, Func<T> defThunk) => await option switch
        {
            Some<T> some => some.Value,
            _ => defThunk()
        };

        /// <inheritdoc cref="OptionRecords.Option.Exists{T}(Option{T}, Predicate{T})"/>
        public static async Task<bool> Exists<T>(Task<Option<T>> option, Predicate<T> predicate) => await option switch
        {
            Some<T> some => predicate(some.Value),
            _ => false
        };

        /// <inheritdoc cref="OptionRecords.Option.Filter{T}(Option{T}, Predicate{T})"/>
        public static async Task<Option<T>> Filter<T>(Task<Option<T>> option, Predicate<T> predicate) => await option switch
        {
            Some<T> some => predicate(some.Value) switch
            {
                true => some,
                false => new None<T>()
            },
            _ => new None<T>()
        };

        /// <inheritdoc cref="OptionRecords.Option.Flatten{T}(Option{Option{T}})"/>
        public static async Task<Option<T>> Flatten<T>(Task<Option<Option<T>>> option) => await option switch
        {
            Some<Option<T>> outter => outter.Value switch
            {
                Some<T> inner => inner,
                _ => new None<T>()
            },
            _ => new None<T>()
        };

        /// <inheritdoc cref="OptionRecords.Option.Fold{T, TState}(Option{T}, Func{TState, T, TState}, TState)"/>
        public static async Task<TState> Fold<T, TState>(Task<Option<T>> option, Func<TState, T, TState> folder, TState state) => await option switch
        {
            Some<T> some => folder(state, some.Value),
            _ => state
        };

        /// <inheritdoc cref="OptionRecords.Option.Fold{T, TState}(Option{T}, Func{TState, T, TState}, TState)"/>
        public static async Task<TState> FoldBack<T, TState>(Task<Option<T>> option, Func<TState, T, TState> folder, TState state)
            => await Option.Fold(option, folder, state);

        /// <inheritdoc cref="OptionRecords.Option.ForAll{T}(Option{T}, Predicate{T})"/>
        public static async Task<bool> ForAll<T>(Task<Option<T>> option, Predicate<T> predicate) => await option switch
        {
            Some<T> some => predicate(some.Value),
            _ => true
        };

        /// <inheritdoc cref="OptionRecords.Option.Get{T}(Option{T})"/>
        public static async Task<T> Get<T>(Task<Option<T>> option) => await option switch
        {
            Some<T> some => some.Value,
            _ => throw new ArgumentException("The option value was None")
        };

        /// <inheritdoc cref="OptionRecords.Option.Get{T, TException}(Option{T}, Func{TException})"/>
        public static async Task<T> Get<T, TException>(Task<Option<T>> option, Func<TException> exception) where TException : Exception, new()
            => await option switch
            {
                Some<T> some => some.Value,
                _ => throw exception()
            };

        /// <inheritdoc cref="OptionRecords.Option.IsNone{T}(Option{T})"/>
        public static async Task<bool> IsNone<T>(Task<Option<T>> option) => await option switch
        {
            None<T> none => true,
            _ => false
        };

        /// <inheritdoc cref="OptionRecords.Option.IsSome{T}(Option{T})"/>
        public static async Task<bool> IsSome<T>(Task<Option<T>> option) => await option switch
        {
            Some<T> some => true,
            _ => false
        };

        /// <inheritdoc cref="OptionRecords.Option.Iter{T}(Option{T}, Action{T})"/>
        public static async Task Iter<T>(Option<T> option, Func<T, Task> action)
        {
            if (option is Some<T> some)
                await action(some.Value);
        }

        /// <inheritdoc cref="OptionRecords.Option.Iter{T}(Option{T}, Action{T})"/>
        public static async Task Iter<T>(Task<Option<T>> option, Action<T> action)
        {
            if (await option is Some<T> some)
                action(some.Value);
        }

        /// <inheritdoc cref="OptionRecords.Option.Iter{T}(Option{T}, Action{T})"/>
        public static async Task Iter<T>(Task<Option<T>> option, Func<T, Task> action)
        {
            if (await option is Some<T> some)
                await action(some.Value);
        }

        /// <inheritdoc cref="OptionRecords.Option.Map{T, U}(Option{T}, Func{T, U})"/>
        public static async Task<Option<U>> Map<T, U>(Option<T> option, Func<T, Task<U>> mapping) => option switch
        {
            Some<T> some => new Some<U>(await mapping(some.Value)),
            _ => new None<U>()
        };

        /// <inheritdoc cref="OptionRecords.Option.Map{T, U}(Option{T}, Func{T, U})"/>
        public static async Task<Option<U>> Map<T, U>(Task<Option<T>> option, Func<T, U> mapping) => await option switch
        {
            Some<T> some => new Some<U>(mapping(some.Value)),
            _ => new None<U>()
        };

        /// <inheritdoc cref="OptionRecords.Option.Map{T, U}(Option{T}, Func{T, U})"/>
        public static async Task<Option<U>> Map<T, U>(Task<Option<T>> option, Func<T, Task<U>> mapping) => await option switch
        {
            Some<T> some => new Some<U>(await mapping(some.Value)),
            _ => new None<U>()
        };

        /// <param name="options"> The input options </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if either input is None </returns>
        public static async Task<Option<U>> Map2<T1, T2, U>((Option<T1>, Option<T2>) options, Func<T1, T2, Task<U>> mapping) => options switch
        {
            (Some<T1> one, Some<T2> two) some => new Some<U>(await mapping(one.Value, two.Value)),
            _ => new None<U>()
        };

        /// <param name="options"> The input options </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if either input is None </returns>
        public static async Task<Option<U>> Map2<T1, T2, U>((Task<Option<T1>>, Task<Option<T2>>) options, Func<T1, T2, U> mapping) => (await options.Item1, await options.Item2) switch
        {
            (Some<T1> one, Some<T2> two) some => new Some<U>(mapping(one.Value, two.Value)),
            _ => new None<U>()
        };

        /// <param name="options"> The input options </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if either input is None </returns>
        public static async Task<Option<U>> Map2<T1, T2, U>((Task<Option<T1>>, Task<Option<T2>>) options, Func<T1, T2, Task<U>> mapping) => (await options.Item1, await options.Item2) switch
        {
            (Some<T1> one, Some<T2> two) some => new Some<U>(await mapping(one.Value, two.Value)),
            _ => new None<U>()
        };
        /// <param name="option1"> The first input option </param>
        /// <param name="option2"> The second input option </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if either input is None </returns>
        public static async Task<Option<U>> Map2<T1, T2, U>(Option<T1> option1, Option<T2> option2, Func<T1, T2, Task<U>> mapping) => await Option.Map2((option1, option2), mapping);

        /// <param name="option1"> The first input option </param>
        /// <param name="option2"> The second input option </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if either input is None </returns>
        public static async Task<Option<U>> Map2<T1, T2, U>(Task<Option<T1>> option1, Task<Option<T2>> option2, Func<T1, T2, U> mapping) => await Option.Map2((option1, option2), mapping);

        /// <param name="option1"> The first input option </param>
        /// <param name="option2"> The second input option </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if either input is None </returns>
        public static async Task<Option<U>> Map2<T1, T2, U>(Task<Option<T1>> option1, Task<Option<T2>> option2, Func<T1, T2, Task<U>> mapping) => await Option.Map2((option1, option2), mapping);

        /// <param name="options"> The input options </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if any input is None </returns>
        public static async Task<Option<U>> Map3<T1, T2, T3, U>((Option<T1>, Option<T2>, Option<T3>) options, Func<T1, T2, T3, Task<U>> mapping) => options switch
        {
            (Some<T1> one, Some<T2> two, Some<T3> three) some => new Some<U>(await mapping(one.Value, two.Value, three.Value)),
            _ => new None<U>()
        };

        /// <param name="options"> The input options </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if any input is None </returns>
        public static async Task<Option<U>> Map3<T1, T2, T3, U>((Task<Option<T1>>, Task<Option<T2>>, Task<Option<T3>>) options, Func<T1, T2, T3, U> mapping) => (await options.Item1, await options.Item2, await options.Item3) switch
        {
            (Some<T1> one, Some<T2> two, Some<T3> three) some => new Some<U>(mapping(one.Value, two.Value, three.Value)),
            _ => new None<U>()
        };

        /// <param name="options"> The input options </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if any input is None </returns>
        public static async Task<Option<U>> Map3<T1, T2, T3, U>((Task<Option<T1>>, Task<Option<T2>>, Task<Option<T3>>) options, Func<T1, T2, T3, Task<U>> mapping) => (await options.Item1, await options.Item2, await options.Item3) switch
        {
            (Some<T1> one, Some<T2> two, Some<T3> three) some => new Some<U>(await mapping(one.Value, two.Value, three.Value)),
            _ => new None<U>()
        };

        /// <param name="option1"> The first input option </param>
        /// <param name="option2"> The second input option </param>
        /// <param name="option3"> The third input option </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if any input is None </returns>
        public static async Task<Option<U>> Map3<T1, T2, T3, U>(Option<T1> option1, Option<T2> option2, Option<T3> option3, Func<T1, T2, T3, Task<U>> mapping) => await Option.Map3((option1, option2, option3), mapping);

        /// <param name="option1"> The first input option </param>
        /// <param name="option2"> The second input option </param>
        /// <param name="option3"> The third input option </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if any input is None </returns>
        public static async Task<Option<U>> Map3<T1, T2, T3, U>(Task<Option<T1>> option1, Task<Option<T2>> option2, Task<Option<T3>> option3, Func<T1, T2, T3, U> mapping) => await Option.Map3((option1, option2, option3), mapping);

        /// <param name="option1"> The first input option </param>
        /// <param name="option2"> The second input option </param>
        /// <param name="option3"> The third input option </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if any input is None </returns>
        public static async Task<Option<U>> Map3<T1, T2, T3, U>(Task<Option<T1>> option1, Task<Option<T2>> option2, Task<Option<T3>> option3, Func<T1, T2, T3, Task<U>> mapping) => await Option.Map3((option1, option2, option3), mapping);

        /// <inheritdoc cref="OptionRecords.Option.ofNullable{T}(T?)"/>
        public static async Task<Option<T>> ofNullable<T>(Task<Nullable<T>> value) where T : struct => (await value).HasValue switch
        {
            true => new Some<T>((await value).Value),
            false => new None<T>()
        };

        /// <inheritdoc cref="OptionRecords.Option.OfObj{T}(T)"/>
        public static async Task<Option<T>> OfObj<T>(Task<T> value) => (await value != null) switch
        {
            true => new Some<T>(await value),
            false => new None<T>()
        };

        /// <inheritdoc cref="OptionRecords.Option.OrElse{T}(Option{T}, Option{T})"/>
        public static async Task<Option<T>> OrElse<T>(Task<Option<T>> option, Option<T> ifNone) => await option switch
        {
            Some<T> some => some,
            _ => ifNone
        };

        /// <inheritdoc cref="OptionRecords.Option.OrElse{T}(Option{T}, Option{T})"/>
        public static async Task<Option<T>> OrElse<T>(Option<T> option, Task<Option<T>> ifNone) => option switch
        {
            Some<T> some => some,
            _ => await ifNone
        };

        /// <inheritdoc cref="OptionRecords.Option.OrElse{T}(Option{T}, Option{T})"/>
        public static async Task<Option<T>> OrElse<T>(Task<Option<T>> option, Task<Option<T>> ifNone) => await option switch
        {
            Some<T> some => some,
            _ => await ifNone
        };

        /// <inheritdoc cref="OptionRecords.Option.OrElseWith{T}(Option{T}, Func{Option{T}})"/>
        public static async Task<Option<T>> OrElseWith<T>(Task<Option<T>> option, Func<Option<T>> ifNoneThunk) => await option switch
        {
            Some<T> some => some,
            _ => ifNoneThunk()
        };

        /// <inheritdoc cref="OptionRecords.Option.OrElseWith{T}(Option{T}, Func{Option{T}})"/>
        public static async Task<Option<T>> OrElseWith<T>(Option<T> option, Func<Task<Option<T>>> ifNoneThunk) => option switch
        {
            Some<T> some => some,
            _ => await ifNoneThunk()
        };

        /// <inheritdoc cref="OptionRecords.Option.OrElseWith{T}(Option{T}, Func{Option{T}})"/>
        public static async Task<Option<T>> OrElseWith<T>(Task<Option<T>> option, Func<Task<Option<T>>> ifNoneThunk) => await option switch
        {
            Some<T> some => some,
            _ => await ifNoneThunk()
        };

        /// <inheritdoc cref="OptionRecords.Option.ToArray{T}(Option{T})"/>
        public static async Task<T[]> ToArray<T>(Task<Option<T>> option) => await option switch
        {
            Some<T> some => new T[] { some.Value },
            _ => new T[0]
        };

        /// <inheritdoc cref="OptionRecords.Option.ToEnumerable{T}(Option{T})"/>
        public static async IAsyncEnumerable<T> ToEnumerable<T>(Task<Option<T>> option)
        {
            if (await option is Some<T> some)
                yield return some.Value;
        }

        /// <inheritdoc cref="OptionRecords.Option.ToList{T}(Option{T})"/>
        public static async Task<List<T>> ToList<T>(Task<Option<T>> option) => await option switch
        {
            Some<T> some => new List<T> { some.Value },
            _ => new List<T>()
        };

        /// <inheritdoc cref="OptionRecords.Option.ToNullable{T}(Option{T})"/>
        public static async Task<Nullable<T>> ToNullable<T>(Task<Option<T>> option) where T : struct => await option switch
        {
            Some<T> some => new Nullable<T>(some.Value),
            _ => new Nullable<T>()
        };

        /// <inheritdoc cref="OptionRecords.Option.ToObj{T}(Option{T})"/>
        public static async Task<T> ToObj<T>(Task<Option<T>> option) where T : class => await option switch
        {
            Some<T> some => some.Value,
            _ => null
        };

    }
}