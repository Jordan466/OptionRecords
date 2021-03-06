using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OptionRecords
{
    /// <summary>
    /// Contains extension methods for working with async options
    /// </summary>
    public static class AsyncOptionExt
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