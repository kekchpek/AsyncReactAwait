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
        /// Aggregate two bindable values
        /// </summary>
        public static IBindable<TRes> Aggregate<T1, T2, TRes>(IBindable<T1> b1, IBindable<T2> b2, Func<T1, T2, TRes> aggregator)
        {
            if (!(b1 is IBindableRaw br1))
                throw new InvalidOperationException(
                    $"Bindable value should implement {nameof(IBindableRaw)} to be aggregated!");
            if (!(b2 is IBindableRaw br2))
                throw new InvalidOperationException(
                    $"Bindable value should implement {nameof(IBindableRaw)} to be aggregated!");
            
            return new BindableAggregator<TRes>(
                new[] { br1, br2 }, 
                values => aggregator((T1)values[0], (T2)values[1]));
        }
        
        /// <summary>
        /// Aggregate three bindable values
        /// </summary>
        public static IBindable<TRes> Aggregate<T1, T2, T3, TRes>(IBindable<T1> b1, IBindable<T2> b2, IBindable<T3> b3, Func<T1, T2, T3, TRes> aggregator)
        {
            if (!(b1 is IBindableRaw br1))
                throw new InvalidOperationException(
                    $"Bindable value should implement {nameof(IBindableRaw)} to be aggregated!");
            if (!(b2 is IBindableRaw br2))
                throw new InvalidOperationException(
                    $"Bindable value should implement {nameof(IBindableRaw)} to be aggregated!");
            if (!(b3 is IBindableRaw br3))
                throw new InvalidOperationException(
                    $"Bindable value should implement {nameof(IBindableRaw)} to be aggregated!");
            
            return new BindableAggregator<TRes>(
                new[] { br1, br2, br3 }, 
                values => aggregator((T1)values[0], (T2)values[1], (T3)values[2]));
        }
        
        /// <summary>
        /// Aggregate four bindable values
        /// </summary>
        public static IBindable<TRes> Aggregate<T1, T2, T3, T4, TRes>(IBindable<T1> b1, IBindable<T2> b2, IBindable<T3> b3, IBindable<T4> b4, Func<T1, T2, T3, T4, TRes> aggregator)
        {
            if (!(b1 is IBindableRaw br1))
                throw new InvalidOperationException(
                    $"Bindable value should implement {nameof(IBindableRaw)} to be aggregated!");
            if (!(b2 is IBindableRaw br2))
                throw new InvalidOperationException(
                    $"Bindable value should implement {nameof(IBindableRaw)} to be aggregated!");
            if (!(b3 is IBindableRaw br3))
                throw new InvalidOperationException(
                    $"Bindable value should implement {nameof(IBindableRaw)} to be aggregated!");
            if (!(b4 is IBindableRaw br4))
                throw new InvalidOperationException(
                    $"Bindable value should implement {nameof(IBindableRaw)} to be aggregated!");
            
            return new BindableAggregator<TRes>(
                new[] { br1, br2, br3, br4 }, 
                values => aggregator((T1)values[0], (T2)values[1], (T3)values[2], (T4)values[3]));
        }
        
        /// <summary>
        /// Aggregate any count of bindable values of same type
        /// </summary>
        public static IBindable<TRes> Aggregate<T, TRes>(IEnumerable<IBindable<T>> bindableValues, Func<T[], TRes> aggregator)
        {

            var rawBindables = bindableValues.Select(x =>
            {
                if (!(x is IBindableRaw br))
                    throw new InvalidOperationException(
                        $"Bindable value should implement {nameof(IBindableRaw)} to be aggregated!");
                return br;
            });
            return new BindableAggregator<TRes>(rawBindables, 
                values => aggregator(values.Cast<T>().ToArray()));
        }
    }
}