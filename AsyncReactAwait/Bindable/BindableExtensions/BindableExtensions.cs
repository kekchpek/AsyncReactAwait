using System;
using System.Threading;
using AsyncReactAwait.Bindable.Awaiter;

namespace AsyncReactAwait.Bindable.BindableExtensions
{

    /// <summary>
    /// Extensions for bindable values.
    /// </summary>
    public static class BindableExtensions
    {

        /// <summary>
        /// Awaits the bindable value becomes specific value.
        /// </summary>
        /// <typeparam name="T">The type of bindable value.</typeparam>
        /// <param name="bindable">The bindable value.</param>
        /// <param name="value">The value to await.</param>
        /// <returns>The awaiter for specified value.</returns>
        public static IBindableAwaiter<T> WillBeEqual<T>(this IBindable<T> bindable, T value)
        {
            return bindable.WillBe(v => v.Equals(value));
        }

        /// <summary>
        /// Awaits for specific value.
        /// </summary>
        /// <param name="bindable">The bindable value to operate with.</param>
        /// <param name="predicate">The specific value awaiter.</param>
        /// <param name="checkCurrentValue">False if you don't want to check current value</param>
        public static IBindableAwaiter<T> WillBe<T>(this IBindable<T> bindable, 
            Func<T, bool> predicate, bool checkCurrentValue = true)
        {
            return new BindableAwaiter<T>(bindable, SynchronizationContext.Current, predicate, checkCurrentValue);
        }

        /// <summary>
        /// Converts bindable value to some other value, that depends on source bindable.
        /// </summary>
        /// <param name="bindable">The bindable to convert.</param>
        /// <param name="predicate">The predicate to apply to bindable value.</param>
        /// <typeparam name="T">The type of converted bindable value.</typeparam>
        /// <typeparam name="TSource">The source bindable value type.</typeparam>
        /// <returns></returns>
        public static IBindable<T> ConvertTo<T, TSource>(this IBindable<TSource> bindable, Func<TSource, T> predicate)
        {
            return new BindableDecorator<T, TSource>(bindable, predicate);
        }

    }
}
