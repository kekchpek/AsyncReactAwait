using System;
using System.Collections.Generic;
using System.Linq;

namespace AsyncReactAwait.Bindable
{
    /// <summary>
    /// Tools for bindable and mutable objects
    /// </summary>
    public static class Bindable
    {
        
        /// <summary>
        /// Aggregate any count of bindable values of same type
        /// </summary>
        public static IBindable<TRes> Aggregate<T, TRes>(IReadOnlyCollection<IBindable<T>> bindables, Func<IReadOnlyList<IBindable<T>>, TRes> aggregator)
        {
            return new BindableAggregator<T, TRes>(
                bindables, 
                aggregator);
        }

        /// <summary>
        /// Aggregate two bindable values
        /// </summary>
        public static IBindable<TRes> Aggregate<T1, T2, TRes>(IBindable<T1> b1, IBindable<T2> b2, Func<T1, T2, TRes> aggregator)
        {
            return new BindableAggregator<T1, T2, TRes>(
                b1, b2,
                aggregator);
        }
        
        /// <summary>
        /// Aggregate three bindable values
        /// </summary>
        public static IBindable<TRes> Aggregate<T1, T2, T3, TRes>(IBindable<T1> b1, IBindable<T2> b2, IBindable<T3> b3, Func<T1, T2, T3, TRes> aggregator)
        {
            return new BindableAggregator<T1, T2, T3, TRes>(
                b1, b2, b3,
                aggregator);
        }
        
        /// <summary>
        /// Aggregate four bindable values
        /// </summary>
        public static IBindable<TRes> Aggregate<T1, T2, T3, T4, TRes>(IBindable<T1> b1, IBindable<T2> b2, IBindable<T3> b3, IBindable<T4> b4, Func<T1, T2, T3, T4, TRes> aggregator)
        {
            return new BindableAggregator<T1, T2, T3, T4, TRes>(
                b1, b2, b3, b4,
                aggregator);
        }
        
        /// <summary>
        /// Aggregate any count of bindable values. Before it should be able to be casted to IBindableRaw.
        /// This method is slow, because of value type boxing. Use it only if you have no other choice.
        /// </summary>
        public static IBindable<TRes> Aggregate<TRes>(IReadOnlyList<IBindable> bindableValues, Func<IReadOnlyList<IBindableRaw>, TRes> aggregator)
        {
            if (bindableValues == null) throw new ArgumentNullException(nameof(bindableValues));
            if (bindableValues.Any(x => x is not IBindableRaw   ))
                throw new InvalidOperationException("All bindable values should implement IBindableRaw to be aggregated!");
            var rawBindables = bindableValues.Cast<IBindableRaw>().ToArray();
            return new BindableAggregator<TRes>(rawBindables, aggregator);
        }
    }
}