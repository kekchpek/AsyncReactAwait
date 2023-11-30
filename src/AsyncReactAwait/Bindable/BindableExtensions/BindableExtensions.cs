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
            return bindable.WillBe(v => (v != null && v.Equals(value)) ||
                                        (v == null && value == null));
        }

        /// <summary>
        /// Awaits for specific value.
        /// </summary>
        /// <param name="bindable">The bindable value to operate with.</param>
        /// <param name="predicate">The specific value awaiter.</param>
        /// <param name="checkCurrentValue">False if you don't want to check current value</param>
        // ReSharper disable once MemberCanBePrivate.Global
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
        
        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="mutable">Mutable to extend.</param>
        /// <param name="value">New value.</param>
        public static void Set<T>(this IMutable<T> mutable, T value)
        {
            mutable.Value = value;
        }


        /// <summary>
        /// Proxies a value and it's changes from certain bindable object.
        /// Setting a value explicitly breaks proxying.
        /// </summary>
        /// <param name="proxy">The proxying object.</param>
        /// <param name="valueSource">The object to proxy.</param>
        /// <param name="converter">The converter from proxy type.</param>
        /// <typeparam name="TSource">The proxy type.</typeparam>
        /// <typeparam name="T">The proxying type.</typeparam>
        public static void Proxy<T, TSource>(this IMutable<T> proxy, IBindable<TSource> valueSource, Func<TSource, T> converter)
        {
            proxy.Proxy(valueSource.ConvertTo(converter));
        }
        
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="bindable">Bindable to extend.</param>
        /// <param name="value">New value.</param>
        public static T Get<T>(this IBindable<T> bindable, T value)
        {
            return bindable.Value;
        }

        /// <summary>
        /// Aggregates bindable with other bindable value
        /// </summary>
        /// <param name="bindable1">The first bindable to aggregate.</param>
        /// <param name="bindable2">The second bindable to aggregate.</param>
        /// <param name="aggregator">An aggregation function.</param>
        /// <typeparam name="T1">A type of the first bindable value.</typeparam>
        /// <typeparam name="T2">A type of the second bindable value.</typeparam>
        /// <typeparam name="TRes">The type of the result of the aggregation.</typeparam>
        /// <returns>An aggregated bindable.</returns>
        public static IBindable<TRes> AggregateWith<T1, T2, TRes>(this IBindable<T1> bindable1, IBindable<T2> bindable2,
            Func<T1, T2, TRes> aggregator)
        {
            return Bindable.Aggregate(bindable1, bindable2, aggregator);
        }

    }
}
