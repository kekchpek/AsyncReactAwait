using AsyncReactAwait.Bindable.Awaiter;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace AsyncReactAwait.Bindable
{

    /// <inheritdoc cref="IMutable{T}"/>
    public class Mutable<T> : IMutable<T>
    {

        [AllowNull]
        private T _value;

        private event Action<T> _onChange;
        private event Action _onChangeBlind;

        /// <inheritdoc cref="IMutable{T}.Value"/>
        public T Value
        {
            get => _value;
            set
            {
                if (_value.Equals(value))
                    return;
                _value = value;
                _onChangeBlind?.Invoke();
                _onChange?.Invoke(_value);
            }
        }

        T IBindable<T>.Value => _value;

        /// <summary>
        /// Default constructor to create changable mutable value.
        /// </summary>
        /// <param name="initialValue">Initial value.</param>
        public Mutable([AllowNull] T initialValue = default)
        {
            _value = initialValue;
        }

        /// <inheritdoc cref="IBindable{T}.Bind(Action{T}, bool)"/>
        /// <exception cref="ArgumentNullException"></exception>
        public void Bind(Action<T> handler, bool callImmediately = true)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            if (callImmediately)
            {
                handler.Invoke(_value);
            }
            _onChange += handler;
        }

        /// <inheritdoc cref="IBindable{T}.Bind(Action, bool)"/>
        /// <exception cref="ArgumentNullException"></exception>
        public void Bind(Action handler, bool callImmediately = true)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _onChangeBlind += handler;

            if (callImmediately)
            {
                handler.Invoke();
            }
        }

        /// <inheritdoc cref="IBindable{T}.Unbind(Action{T})"/>
        public void Unbind(Action<T> handler)
        {
            _onChange -= handler;
        }

        /// <inheritdoc cref="IBindable{T}.Unbind(Action)"/>
        public void Unbind(Action handler)
        {
            _onChangeBlind -= handler;
        }

        /// <inheritdoc cref="IBindable{T}.WillBe(Func{T, bool}, bool)"/>
        public IBindableAwaiter<T> WillBe(Func<T, bool> predicate, bool checkCurrentValue = true)
        {
            return new BindableAwaiter<T>(this, SynchronizationContext.Current, predicate, checkCurrentValue);
        }
    }
}
