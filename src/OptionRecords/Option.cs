using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OptionRecords
{
    /// <summary>
    /// An Optional value.
    /// Can be either Some or None
    /// </summary>
    public abstract record Option<T>();

    /// <summary>
    /// The representation of "Value of type 'T"
    /// </summary>
    /// <param name="Value">The input value</param>
    public record Some<T>(T Value) : Option<T>;

    /// <summary>
    /// The representation of "No value"
    /// </summary>
    public record None<T>() : Option<T>;

    /// <summary>
    /// Contains operations for working with options
    /// </summary>
    public static class Option
    {
        /// <summary>
        /// The representation of "Value of type 'T'"
        /// </summary>
        /// <param name="value">The input value</param>
        /// <returns> Some </returns>
        public static Some<T> Some<T>(T value) => new Some<T>(value);

        /// <summary>
        /// The representation of "No value"
        /// </summary>
        /// <returns> None </returns>
        public static None<T> None<T>() => new None<T>();

        /// <summary>
        /// Applies a binder over an option
        /// </summary>
        /// <param name="option"> The input option </param>
        /// <param name="binder"> A function that takes the value of type T from an option and transforms it into an option containing a value of type U. </param>
        /// <returns> An option of the output type of the binder </returns>
        public static Option<U> Bind<T, U>(Option<T> option, Func<T, Option<U>> binder) => option switch
        {
            Some<T> some => binder(some.Value),
            _ => new None<U>()
        };

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

        /// <summary>
        /// Evaluates to true if option is Some and its value is equal to value
        /// </summary>
        /// <param name="option"> The input option </param>
        /// <param name="value"> The value to test for equality </param>
        /// <returns> True if the option is Some and contains a value equal to value, otherwise false </returns>
        public static bool Contains<T>(Option<T> option, T value) => option switch
        {
            Some<T> some => some.Value.Equals(value),
            _ => false
        };

        /// <inheritdoc cref="OptionRecords.Option.Contains{T}(Option{T}, T)"/>
        public static async Task<bool> Contains<T>(Task<Option<T>> option, T value) => await option switch
        {
            Some<T> some => some.Value.Equals(value),
            _ => false
        };

        /// <param name="option"> The input option </param>
        /// <returns> A zero if the option is None, a one otherwise. </returns>
        public static int Count<T>(Option<T> option) => option switch
        {
            Some<T> some => 1,
            _ => 0
        };

        /// <inheritdoc cref="OptionRecords.Option.Count{T}(Option{T})"/>
        public static async Task<int> Count<T>(Task<Option<T>> option) => await option switch
        {
            Some<T> some => 1,
            _ => 0
        };

        /// <summary>
        /// Gets the value of the option if the option is Some, otherwise returns the specified default value
        /// </summary>
        /// <param name="option"> The input option </param>
        /// <param name="value"> The specified default value </param>
        /// <returns> The option if the option is Some, else the default value </returns>
        public static T DefaultValue<T>(Option<T> option, T value) => option switch
        {
            Some<T> some => some.Value,
            _ => value
        };

        /// <inheritdoc cref="OptionRecords.Option.DefaultValue{T}(Option{T}, T)"/>
        public static async Task<T> DefaultValue<T>(Task<Option<T>> option, T value) => await option switch
        {
            Some<T> some => some.Value,
            _ => value
        };

        /// <summary>
        /// Gets the value of the option if the option is Some, otherwise evaluates defThunk and returns the result
        /// </summary>
        /// <param name="option"> The input option </param>
        /// <param name="defThunk"> A thunk that provides a default value when evaluated </param>
        /// <returns> The option if the option is Some, else the result of evaluating defThunk </returns>
        public static T DefaultWith<T>(Option<T> option, Func<T> defThunk) => option switch
        {
            Some<T> some => some.Value,
            _ => defThunk()
        };

        /// <inheritdoc cref="OptionRecords.Option.DefaultValue{T}(Option{T}, T)"/>
        public static async Task<T> DefaultWith<T>(Task<Option<T>> option, Func<T> defThunk) => await option switch
        {
            Some<T> some => some.Value,
            _ => defThunk()
        };

        /// <param name="option"> The input option </param>
        /// <param name="predicate"> A function that evaluates to a boolean when given a value from the option type </param>
        /// <returns> False if the option is None, otherwise it returns the result of applying the predicate to the option value </returns>
        public static bool Exists<T>(Option<T> option, Predicate<T> predicate) => option switch
        {
            Some<T> some => predicate(some.Value),
            _ => false
        };

        /// <inheritdoc cref="OptionRecords.Option.Exists{T}(Option{T}, Predicate{T})"/>
        public static async Task<bool> Exists<T>(Task<Option<T>> option, Predicate<T> predicate) => await option switch
        {
            Some<T> some => predicate(some.Value),
            _ => false
        };

        /// <param name="option"> The input option </param>
        /// <param name="predicate"> A function that evaluates whether the value contained in the option should remain, or be filtered out </param>
        /// <returns> The input if the predicate evaluates to true; otherwise, None </returns>
        public static Option<T> Filter<T>(Option<T> option, Predicate<T> predicate) => option switch
        {
            Some<T> some => predicate(some.Value) switch
            {
                true => some,
                false => new None<T>()
            },
            _ => new None<T>()
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

        /// <param name="option"> The input option </param>
        /// <returns> The input value if the value is Some; otherwise, None </returns>
        public static Option<T> Flatten<T>(Option<Option<T>> option) => option switch
        {
            Some<Option<T>> outter => outter.Value switch
            {
                Some<T> inner => inner,
                _ => new None<T>()
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

        /// <param name="option"> The input option </param>
        /// <param name="folder"> A function to update the state data when given a value from an option </param>
        /// <param name="state"> The initial state </param>
        /// <returns> The original state if the option is None, otherwise it returns the updated state with the folder and the option value </returns>
        public static TState Fold<T, TState>(Option<T> option, Func<TState, T, TState> folder, TState state) => option switch
        {
            Some<T> some => folder(state, some.Value),
            _ => state
        };

        /// <inheritdoc cref="OptionRecords.Option.Fold{T, TState}(Option{T}, Func{TState, T, TState}, TState)"/>
        public static async Task<TState> Fold<T, TState>(Task<Option<T>> option, Func<TState, T, TState> folder, TState state) => await option switch
        {
            Some<T> some => folder(state, some.Value),
            _ => state
        };

        /// <param name="option"> The input option </param>
        /// <param name="folder"> A function to update the state data when given a value from an option </param>
        /// <param name="state"> The initial state </param>
        /// <returns> The original state if the option is None, otherwise it returns the updated state with the folder and the option value </returns>
        public static TState FoldBack<T, TState>(Option<T> option, Func<TState, T, TState> folder, TState state)
            => Option.Fold(option, folder, state);

        /// <inheritdoc cref="OptionRecords.Option.Fold{T, TState}(Option{T}, Func{TState, T, TState}, TState)"/>
        public static async Task<TState> FoldBack<T, TState>(Task<Option<T>> option, Func<TState, T, TState> folder, TState state)
            => await Option.Fold(option, folder, state);

        /// <param name="option"> The input option </param>
        /// <param name="predicate"> A function that evaluates to a boolean when given a value from the option type </param>
        /// <returns> True if the option is None, otherwise it returns the result of applying the predicate to the option value </returns>
        public static bool ForAll<T>(Option<T> option, Predicate<T> predicate) => option switch
        {
            Some<T> some => predicate(some.Value),
            _ => true
        };

        /// <inheritdoc cref="OptionRecords.Option.ForAll{T}(Option{T}, Predicate{T})"/>
        public static async Task<bool> ForAll<T>(Task<Option<T>> option, Predicate<T> predicate) => await option switch
        {
            Some<T> some => predicate(some.Value),
            _ => true
        };

        /// <summary>
        /// Gets the value associated with the option
        /// </summary>
        /// <param name="option"> The input option </param>
        /// <returns> The value within the option </returns>
        /// <exception cref="System.ArgumentException"> Throw when the option is None </exception>
        public static T Get<T>(Option<T> option) => option switch
        {
            Some<T> some => some.Value,
            _ => throw new ArgumentException("The option value was None")
        };

        /// <inheritdoc cref="OptionRecords.Option.Get{T}(Option{T})"/>
        public static async Task<T> Get<T>(Task<Option<T>> option) => await option switch
        {
            Some<T> some => some.Value,
            _ => throw new ArgumentException("The option value was None")
        };

        /// <summary>
        /// Gets the value associated with the option
        /// </summary>
        /// <param name="option"> The input option </param>
        /// <param name="exception"> The exception to be thrown when the input option is None </param>
        /// <returns> The value within the option </returns>
        /// <exception cref="Exception"> Throw when the option is None </exception>
        public static T Get<T, TException>(Option<T> option, Func<TException> exception) where TException : Exception, new()
            => option switch
            {
                Some<T> some => some.Value,
                _ => throw exception()
            };

        /// <inheritdoc cref="OptionRecords.Option.Get{T, TException}(Option{T}, Func{TException})"/>
        public static async Task<T> Get<T, TException>(Task<Option<T>> option, Func<TException> exception) where TException : Exception, new()
            => await option switch
            {
                Some<T> some => some.Value,
                _ => throw exception()
            };

        /// <summary>
        /// Returns true if the option is None
        /// </summary>
        /// <param name="option"> The input option </param>
        /// <returns> True if the option is None </returns>
        public static bool IsNone<T>(Option<T> option) => option switch
        {
            None<T> none => true,
            _ => false
        };

        /// <inheritdoc cref="OptionRecords.Option.IsNone{T}(Option{T})"/>
        public static async Task<bool> IsNone<T>(Task<Option<T>> option) => await option switch
        {
            None<T> none => true,
            _ => false
        };

        /// <summary>
        /// Returns true if the option is not None
        /// </summary>
        /// <param name="option"> The input option </param>
        /// <returns> True if the option is not None </returns>
        public static bool IsSome<T>(Option<T> option) => option switch
        {
            Some<T> some => true,
            _ => false
        };

        /// <inheritdoc cref="OptionRecords.Option.IsSome{T}(Option{T})"/>
        public static async Task<bool> IsSome<T>(Task<Option<T>> option) => await option switch
        {
            Some<T> some => true,
            _ => false
        };

        /// <summary>
        /// Applies a function over the option value
        /// </summary>
        /// <param name="option"> The input option </param>
        /// <param name="action"> A function to apply to the option value </param>
        public static void Iter<T>(Option<T> option, Action<T> action)
        {
            if (option is Some<T> some)
                action(some.Value);
        }

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

        /// <param name="mapping"> A function to apply to the option value </param>
        /// <param name="option"> The input option </param>
        /// <returns> An option of the input value after applying the mapping function, or None if the input is None </returns>
        public static Option<U> Map<T, U>(Option<T> option, Func<T, U> mapping) => option switch
        {
            Some<T> some => new Some<U>(mapping(some.Value)),
            _ => new None<U>()
        };

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
        public static Option<U> Map2<T1, T2, U>((Option<T1>, Option<T2>) options, Func<T1, T2, U> mapping) => options switch
        {
            (Some<T1> one, Some<T2> two) some => new Some<U>(mapping(one.Value, two.Value)),
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
        public static Option<U> Map2<T1, T2, U>(Option<T1> option1, Option<T2> option2, Func<T1, T2, U> mapping) => Option.Map2((option1, option2), mapping);

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
        public static Option<U> Map3<T1, T2, T3, U>((Option<T1>, Option<T2>, Option<T3>) options, Func<T1, T2, T3, U> mapping) => options switch
        {
            (Some<T1> one, Some<T2> two, Some<T3> three) some => new Some<U>(mapping(one.Value, two.Value, three.Value)),
            _ => new None<U>()
        };

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
        public static Option<U> Map3<T1, T2, T3, U>(Option<T1> option1, Option<T2> option2, Option<T3> option3, Func<T1, T2, T3, U> mapping) => Option.Map3((option1, option2, option3), mapping);

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

        /// <summary>
        /// Convert a Nullable value to an option
        /// </summary>
        /// <param name="value"> The input nullable value </param>
        /// <returns> The result option </returns>
        public static Option<T> ofNullable<T>(Nullable<T> value) where T : struct => value.HasValue switch
        {
            true => new Some<T>(value.Value),
            false => new None<T>()
        };

        /// <inheritdoc cref="OptionRecords.Option.ofNullable{T}(T?)"/>
        public static async Task<Option<T>> ofNullable<T>(Task<Nullable<T>> value) where T : struct => (await value).HasValue switch
        {
            true => new Some<T>((await value).Value),
            false => new None<T>()
        };

        /// <summary>
        /// Convert a potentially null value to an option
        /// </summary>
        /// <param name="value"> The input value </param>
        /// <returns> The result option </returns>
        public static Option<T> OfObj<T>(T value) => (value != null) switch
        {
            true => new Some<T>(value),
            false => new None<T>()
        };

        /// <inheritdoc cref="OptionRecords.Option.OfObj{T}(T)"/>
        public static async Task<Option<T>> OfObj<T>(Task<T> value) => (await value != null) switch
        {
            true => new Some<T>(await value),
            false => new None<T>()
        };

        /// <summary>
        /// Returns option if it is Some, otherwise returns ifNone
        /// </summary>
        /// <param name="option"> The input option </param>
        /// <param name="ifNone"> The value to use if option is None </param>
        /// <returns> The option if the option is Some, else the alternate option </returns>
        public static Option<T> OrElse<T>(Option<T> option, Option<T> ifNone) => option switch
        {
            Some<T> some => some,
            _ => ifNone
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

        /// <summary>
        /// Returns option if it is Some, otherwise evaluates ifNoneThunk and returns the result. 
        /// ifNoneThunk is not evaluated unless option is None
        /// </summary>
        /// <param name="option"> The input option </param>
        /// <param name="ifNoneThunk"> A thunk that provides an alternate option when evaluated </param>
        /// <returns> The option if the option is Some, else the result of evaluating ifNoneThunk </returns>
        public static Option<T> OrElseWith<T>(Option<T> option, Func<Option<T>> ifNoneThunk) => option switch
        {
            Some<T> some => some,
            _ => ifNoneThunk()
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

        /// <summary>
        /// Convert the option to an array of length 0 or 1
        /// </summary>
        /// <param name="option"> The input option </param>
        /// <returns> The result array </returns>
        public static T[] ToArray<T>(Option<T> option) => option switch
        {
            Some<T> some => new T[] { some.Value },
            _ => new T[0]
        };

        /// <inheritdoc cref="OptionRecords.Option.ToArray{T}(Option{T})"/>
        public static async Task<T[]> ToArray<T>(Task<Option<T>> option) => await option switch
        {
            Some<T> some => new T[] { some.Value },
            _ => new T[0]
        };

        /// <summary>
        /// Convert the option to an IEnumerable of length 0 or 1
        /// </summary>
        /// <param name="option"> The input option </param>
        /// <returns> The result enumerable </returns>
        public static IEnumerable<T> ToEnumerable<T>(Option<T> option)
        {
            if (option is Some<T> some)
                yield return some.Value;
        }

        /// <inheritdoc cref="OptionRecords.Option.ToEnumerable{T}(Option{T})"/>
        public static async IAsyncEnumerable<T> ToEnumerable<T>(Task<Option<T>> option)
        {
            if (await option is Some<T> some)
                yield return some.Value;
        }

        /// <summary>
        /// Convert the option to a list of length 0 or 1
        /// </summary>
        /// <param name="option"> The input option </param>
        /// <returns> The result list </returns>        
        public static List<T> ToList<T>(Option<T> option) => option switch
        {
            Some<T> some => new List<T> { some.Value },
            _ => new List<T>()
        };

        /// <inheritdoc cref="OptionRecords.Option.ToList{T}(Option{T})"/>
        public static async Task<List<T>> ToList<T>(Task<Option<T>> option) => await option switch
        {
            Some<T> some => new List<T> { some.Value },
            _ => new List<T>()
        };

        /// <summary>
        /// Convert the option to a Nullable value
        /// </summary>
        /// <param name="option"> The input option </param>
        /// <returns> The result value </returns>
        public static Nullable<T> ToNullable<T>(Option<T> option) where T : struct => option switch
        {
            Some<T> some => new Nullable<T>(some.Value),
            _ => new Nullable<T>()
        };

        /// <inheritdoc cref="OptionRecords.Option.ToNullable{T}(Option{T})"/>
        public static async Task<Nullable<T>> ToNullable<T>(Task<Option<T>> option) where T : struct => await option switch
        {
            Some<T> some => new Nullable<T>(some.Value),
            _ => new Nullable<T>()
        };

        /// <summary>
        /// Convert an option to a potentially null value
        /// </summary>
        /// <param name="option"> The input option </param>
        /// <returns> The result value, which is null if the input was None </returns>
        public static T ToObj<T>(Option<T> option) where T : class => option switch
        {
            Some<T> some => some.Value,
            _ => null
        };

        /// <inheritdoc cref="OptionRecords.Option.ToObj{T}(Option{T})"/>
        public static async Task<T> ToObj<T>(Task<Option<T>> option) where T : class => await option switch
        {
            Some<T> some => some.Value,
            _ => null
        };
    }

    /// <summary>
    /// Contains extension methods for working with options
    /// </summary>
    public static class OptionExt
    {
        /// <inheritdoc cref="OptionRecords.Option.Bind{T, U}(Option{T}, Func{T, Option{U}})"/>
        public static Option<U> Bind<T, U>(this Option<T> option, Func<T, Option<U>> binder) => Option.Bind(option, binder);

        /// <inheritdoc cref="OptionRecords.Option.Contains{T}(Option{T}, T)"/>
        public static bool Contains<T>(this Option<T> option, T value) => Option.Contains(option, value);

        /// <inheritdoc cref="OptionRecords.Option.Count{T}(Option{T})"/>
        public static int Count<T>(this Option<T> option) => Option.Count(option);

        /// <inheritdoc cref="OptionRecords.Option.DefaultValue{T}(Option{T}, T)"/>
        public static T DefaultValue<T>(this Option<T> option, T value) => Option.DefaultValue(option, value);

        /// <inheritdoc cref="OptionRecords.Option.DefaultValue{T}(Option{T}, T)"/>
        public static T DefaultWith<T>(this Option<T> option, Func<T> defThunk) => Option.DefaultWith(option, defThunk);

        /// <inheritdoc cref="OptionRecords.Option.Exists{T}(Option{T}, Predicate{T})"/>
        public static bool Exists<T>(this Option<T> option, Predicate<T> predicate) => Option.Exists(option, predicate);

        /// <inheritdoc cref="OptionRecords.Option.Filter{T}(Option{T}, Predicate{T})"/>
        public static Option<T> Filter<T>(this Option<T> option, Predicate<T> predicate) => Option.Filter(option, predicate);

        /// <inheritdoc cref="OptionRecords.Option.Flatten{T}(Option{Option{T}})"/>
        public static Option<T> Flatten<T>(this Option<Option<T>> option) => Option.Flatten(option);

        /// <inheritdoc cref="OptionRecords.Option.Fold{T, TState}(Option{T}, Func{TState, T, TState}, TState)"/>
        public static TState Fold<T, TState>(this Option<T> option, Func<TState, T, TState> fold, TState state) => Option.Fold(option, fold, state);

        /// <inheritdoc cref="OptionRecords.Option.FoldBack{T, TState}(Option{T}, Func{TState, T, TState}, TState)"/>
        public static TState FoldBack<T, TState>(this Option<T> option, Func<TState, T, TState> fold, TState state) => Option.FoldBack(option, fold, state);

        /// <inheritdoc cref="OptionRecords.Option.ForAll{T}(Option{T}, Predicate{T})"/>
        public static bool ForAll<T>(this Option<T> option, Predicate<T> predicate) => Option.ForAll(option, predicate);

        /// <inheritdoc cref="OptionRecords.Option.Get{T}(Option{T})"/>
        public static T Get<T>(this Option<T> option) => Option.Get(option);

        /// <inheritdoc cref="OptionRecords.Option.Get{T, TException}(Option{T}, Func{TException})"/>
        public static T Get<T, TException>(this Option<T> option, Func<TException> exception) where TException : Exception, new() => Option.Get(option, exception);

        /// <inheritdoc cref="OptionRecords.Option.IsNone{T}(Option{T})"/>
        public static bool IsNone<T>(this Option<T> option) => Option.IsNone(option);

        /// <inheritdoc cref="OptionRecords.Option.IsSome{T}(Option{T})"/>
        public static bool IsSome<T>(this Option<T> option) => Option.IsSome(option);

        /// <inheritdoc cref="OptionRecords.Option.Iter{T}(Option{T}, Action{T})"/>
        public static void Iter<T>(this Option<T> option, Action<T> action) => Option.Iter(option, action);

        /// <inheritdoc cref="OptionRecords.Option.Map{T, U}(Option{T}, Func{T, U})"/>
        public static Option<U> Map<T, U>(this Option<T> option, Func<T, U> mapping) => Option.Map(option, mapping);

        /// <param name="options"> The input options </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if either input is None </returns>
        public static Option<U> Map2<T1, T2, U>(this (Option<T1>, Option<T2>) options, Func<T1, T2, U> mapping) => Option.Map2(options, mapping);

        /// <param name="options"> The input options </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if any input is None </returns>
        public static Option<U> Map3<T1, T2, T3, U>(this (Option<T1>, Option<T2>, Option<T3>) options, Func<T1, T2, T3, U> mapping) => Option.Map3(options, mapping);

        /// <inheritdoc cref="OptionRecords.Option.OrElse{T}(Option{T}, Option{T})"/>
        public static Option<T> OrElse<T>(this Option<T> option, Option<T> ifNone) => Option.OrElse(option, ifNone);

        /// <inheritdoc cref="OptionRecords.Option.OrElseWith{T}(Option{T}, Func{Option{T}})"/>
        public static Option<T> OrElseWith<T>(this Option<T> option, Func<Option<T>> ifNoneThunk) => Option.OrElseWith(option, ifNoneThunk);

        /// <inheritdoc cref="OptionRecords.Option.ToArray{T}(Option{T})"/>
        public static T[] ToArray<T>(this Option<T> option) => Option.ToArray(option);

        /// <inheritdoc cref="OptionRecords.Option.ToEnumerable{T}(Option{T})"/>
        public static IEnumerable<T> ToEnumerable<T>(Option<T> option) => Option.ToEnumerable(option);

        /// <inheritdoc cref="OptionRecords.Option.ToList{T}(Option{T})"/>
        public static List<T> ToList<T>(this Option<T> option) => Option.ToList(option);

        /// <inheritdoc cref="OptionRecords.Option.ToNullable{T}(Option{T})"/>
        public static Nullable<T> ToNullable<T>(this Option<T> option) where T : struct => Option.ToNullable(option);

        /// <inheritdoc cref="OptionRecords.Option.ToObj{T}(Option{T})"/>
        public static T ToObj<T>(this Option<T> option) where T : class => Option.ToObj(option);
    }

    /// <summary>
    /// Contains extension methods for working with async options
    /// </summary>
    public static class AsyncOptionEx
    {
        /// <inheritdoc cref="OptionRecords.Option.Bind{T, U}(Option{T}, Func{T, Option{U}})"/>
        public static async Task<Option<U>> Bind<T, U>(this Option<T> option, Func<T, Task<Option<U>>> binder) => await Option.Bind(option, binder);

        /// <inheritdoc cref="OptionRecords.Option.Bind{T, U}(Option{T}, Func{T, Option{U}})"/>
        public static async Task<Option<U>> Bind<T, U>(this Task<Option<T>> option, Func<T, Task<Option<U>>> binder) => await Option.Bind(option, binder);

        /// <inheritdoc cref="OptionRecords.Option.Contains{T}(Option{T}, T)"/>
        public static async Task<bool> Contains<T>(this Task<Option<T>> option, T value) => await Option.Contains(option, value);

        /// <inheritdoc cref="OptionRecords.Option.Count{T}(Option{T})"/>
        public static async Task<int> Count<T>(this Task<Option<T>> option) => await Option.Count(option);

        /// <inheritdoc cref="OptionRecords.Option.DefaultValue{T}(Option{T}, T)"/>
        public static async Task<T> DefaultValue<T>(this Task<Option<T>> option, T value) => await Option.DefaultValue(option, value);

        /// <inheritdoc cref="OptionRecords.Option.DefaultValue{T}(Option{T}, T)"/>
        public static async Task<T> DefaultWith<T>(this Task<Option<T>> option, Func<T> defThunk) => await Option.DefaultWith(option, defThunk);

        /// <inheritdoc cref="OptionRecords.Option.Iter{T}(Option{T}, Action{T})"/>
        public static async Task Iter<T>(this Task<Option<T>> option, Action<T> action) => await Option.Iter(option, action);

        /// <inheritdoc cref="OptionRecords.Option.Map{T, U}(Option{T}, Func{T, U})"/>
        public static async Task<Option<U>> Map<T, U>(this Option<T> option, Func<T, Task<U>> mapping) => await Option.Map(option, mapping);

        /// <inheritdoc cref="OptionRecords.Option.Map{T, U}(Option{T}, Func{T, U})"/>
        public static async Task<Option<U>> Map<T, U>(this Task<Option<T>> option, Func<T, Task<U>> mapping) => await Option.Map(option, mapping);

        /// <param name="options"> The input options </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if either input is None </returns>
        public static async Task<Option<U>> Map2<T1, T2, U>(this (Task<Option<T1>>, Task<Option<T2>>) options, Func<T1, T2, U> mapping) => await Option.Map2(options, mapping);

        /// <param name="options"> The input options </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if either input is None </returns>
        public static async Task<Option<U>> Map2<T1, T2, U>(this (Option<T1>, Option<T2>) options, Func<T1, T2, Task<U>> mapping) => await Option.Map2(options, mapping);

        /// <param name="options"> The input options </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if either input is None </returns>
        public static async Task<Option<U>> Map2<T1, T2, U>(this (Task<Option<T1>>, Task<Option<T2>>) options, Func<T1, T2, Task<U>> mapping) => await Option.Map2(options, mapping);

        /// <param name="options"> The input options </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if any input is None </returns>
        public static async Task<Option<U>> Map3<T1, T2, T3, U>(this (Task<Option<T1>>, Task<Option<T2>>, Task<Option<T3>>) options, Func<T1, T2, T3, U> mapping) => await Option.Map3(options, mapping);

        /// <param name="options"> The input options </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if any input is None </returns>
        public static async Task<Option<U>> Map3<T1, T2, T3, U>((Option<T1>, Option<T2>, Option<T3>) options, Func<T1, T2, T3, Task<U>> mapping) => await Option.Map3(options, mapping);

        /// <param name="options"> The input options </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if any input is None </returns>
        public static async Task<Option<U>> Map3<T1, T2, T3, U>((Task<Option<T1>>, Task<Option<T2>>, Task<Option<T3>>) options, Func<T1, T2, T3, Task<U>> mapping) => await Option.Map3(options, mapping);

        /// <inheritdoc cref="OptionRecords.Option.OrElse{T}(Option{T}, Option{T})"/>
        public static async Task<Option<T>> OrElse<T>(this Task<Option<T>> option, Option<T> ifNone) => await Option.OrElse(option, ifNone);

        /// <inheritdoc cref="OptionRecords.Option.OrElseWith{T}(Option{T}, Func{Option{T}})"/>
        public static async Task<Option<T>> OrElseWith<T>(this Task<Option<T>> option, Func<Option<T>> ifNoneThunk) => await Option.OrElseWith(option, ifNoneThunk);

        /// <inheritdoc cref="OptionRecords.Option.ToArray{T}(Option{T})"/>
        public static async Task<T[]> ToArray<T>(this Task<Option<T>> option) => await Option.ToArray(option);

        /// <inheritdoc cref="OptionRecords.Option.ToEnumerable{T}(Option{T})"/>
        public static IAsyncEnumerable<T> ToEnumerable<T>(Task<Option<T>> option) => Option.ToEnumerable(option);

        /// <inheritdoc cref="OptionRecords.Option.ToList{T}(Option{T})"/>
        public static async Task<List<T>> ToList<T>(this Task<Option<T>> option) => await Option.ToList(option);

        /// <inheritdoc cref="OptionRecords.Option.ToNullable{T}(Option{T})"/>
        public static async Task<Nullable<T>> ToNullable<T>(this Task<Option<T>> option) where T : struct => await Option.ToNullable(option);

        /// <inheritdoc cref="OptionRecords.Option.ToObj{T}(Option{T})"/>
        public static async Task<T> ToObj<T>(this Task<Option<T>> option) where T : class => await Option.ToObj(option);
    }
}
