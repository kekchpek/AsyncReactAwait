using System;

namespace AsyncReactAwait.Bindable
{

    /// <summary>
    /// Class for representing changeable bindable value.
    /// </summary>
    /// <typeparam name="T">Bindable value type.</typeparam>
    public interface IMutable<T> : IBindable<T>
    {

        /// <summary>
        /// Fired on any listener is bound for this value.
        /// </summary>
        event Action OnAnySubscription;

        /// <summary>
        /// Fired on any listener is unbound from this value, and no other listeners left.
        /// </summary>
        event Action OnSubscriptionsCleared;

        /// <inheritdoc cref="IBindable{T}.Value"/>
        /// Automatically resets proxying value.
        new T Value { get; set; }

        /// <summary>
        /// Proxies a value and it's changes from certain bindable object.
        /// Setting a value explicitly breaks proxying.
        /// </summary>
        /// <param name="valueSource">The object to proxy.</param>
        void Proxy(IBindable<T> valueSource);

        /// <summary>
        /// Breaks proxying other bindable. If it was.
        /// </summary>
        void StopProxying();

        /// <summary>
        /// Sets the value to new value even if they are equal.
        /// </summary>
        /// <param name="value">New value.</param>
        void ForceSet(T value);
    }
}
