using System;
using System.Collections.Generic;

namespace OptionRecords
{
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
}