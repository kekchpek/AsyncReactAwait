﻿namespace AsyncReactAwait.Bindable
{

    /// <summary>
    /// Class for representing changeable bindable value.
    /// </summary>
    /// <typeparam name="T">Bindable value type.</typeparam>
    public interface IMutable<T> : IBindable<T>
    {

        /// <inheritdoc cref="IBindable{T}.Value"/>
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
        /// Sets the value to new value event if they are equal.
        /// </summary>
        /// <param name="value">New value.</param>
        void ForceSet(T value);
    }
}
