﻿using System;

namespace AsyncReactAwait.Bindable
{

    /// <summary>
    /// Non-generic bindable value. 
    /// </summary>
    public interface IBindable
    {
        /// <summary>
        /// Bind a handler for value changing.
        /// </summary>
        /// <param name="handler">Value changing handler.</param>
        /// <param name="callImmediately">Calls handler instantly when it is set.</param>
        /// <exception cref="ArgumentNullException">Handler is null.</exception>
        void Bind(Action handler, bool callImmediately = true);
        
        /// <summary>
        /// Unbinds the value changing handler.
        /// </summary>
        /// <param name="handler">Value changing handler.</param>
        void Unbind(Action handler);
    }

    /// <summary>
    /// Value, changes of which could be handled with bound handlers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBindable<out T> : IBindable
    {

        /// <summary>
        /// Current value.
        /// </summary>
        T Value { get; }

        /// <inheritdoc cref="IBindable.Bind(Action, bool)"/>
        void Bind(Action<T> handler, bool callImmediately = true);

        /// <summary>
        /// <inheritdoc cref="Bind(System.Action{T},bool)"/>
        /// </summary>
        /// <param name="handler"><inheritdoc cref="Bind(System.Action{T},bool)"/> It handles previous and new value.</param>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="Bind(System.Action{T},bool)"/></exception>
        void Bind(Action<T, T> handler);

        /// <inheritdoc cref="IBindable.Unbind(Action)"/>
        void Unbind(Action<T> handler);


        /// <inheritdoc cref="Unbind(Action{T})"/>
        void Unbind(Action<T, T> handler);

    }
}
