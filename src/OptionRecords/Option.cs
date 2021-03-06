using System;
using System.Collections.Generic;

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
    public static partial class Option
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

        /// <param name="option"> The input option </param>
        /// <returns> A zero if the option is None, a one otherwise. </returns>
        public static int Count<T>(Option<T> option) => option switch
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

        /// <param name="option"> The input option </param>
        /// <param name="predicate"> A function that evaluates to a boolean when given a value from the option type </param>
        /// <returns> False if the option is None, otherwise it returns the result of applying the predicate to the option value </returns>
        public static bool Exists<T>(Option<T> option, Predicate<T> predicate) => option switch
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

        /// <param name="option"> The input option </param>
        /// <param name="folder"> A function to update the state data when given a value from an option </param>
        /// <param name="state"> The initial state </param>
        /// <returns> The original state if the option is None, otherwise it returns the updated state with the folder and the option value </returns>
        public static TState Fold<T, TState>(Option<T> option, Func<TState, T, TState> folder, TState state) => option switch
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

        /// <param name="option"> The input option </param>
        /// <param name="predicate"> A function that evaluates to a boolean when given a value from the option type </param>
        /// <returns> True if the option is None, otherwise it returns the result of applying the predicate to the option value </returns>
        public static bool ForAll<T>(Option<T> option, Predicate<T> predicate) => option switch
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

        /// <param name="mapping"> A function to apply to the option value </param>
        /// <param name="option"> The input option </param>
        /// <returns> An option of the input value after applying the mapping function, or None if the input is None </returns>
        public static Option<U> Map<T, U>(Option<T> option, Func<T, U> mapping) => option switch
        {
            Some<T> some => new Some<U>(mapping(some.Value)),
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

        /// <param name="option1"> The first input option </param>
        /// <param name="option2"> The second input option </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if either input is None </returns>
        public static Option<U> Map2<T1, T2, U>(Option<T1> option1, Option<T2> option2, Func<T1, T2, U> mapping) => Option.Map2((option1, option2), mapping);

        /// <param name="options"> The input options </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if any input is None </returns>
        public static Option<U> Map3<T1, T2, T3, U>((Option<T1>, Option<T2>, Option<T3>) options, Func<T1, T2, T3, U> mapping) => options switch
        {
            (Some<T1> one, Some<T2> two, Some<T3> three) some => new Some<U>(mapping(one.Value, two.Value, three.Value)),
            _ => new None<U>()
        };

        /// <param name="option1"> The first input option </param>
        /// <param name="option2"> The second input option </param>
        /// <param name="option3"> The third input option </param>
        /// <param name="mapping"> A function to apply to the option values </param>
        /// <returns> An option of the input values after applying the mapping function, or None if any input is None </returns>
        public static Option<U> Map3<T1, T2, T3, U>(Option<T1> option1, Option<T2> option2, Option<T3> option3, Func<T1, T2, T3, U> mapping) => Option.Map3((option1, option2, option3), mapping);

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
    }
}
