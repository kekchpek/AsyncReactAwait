using System;
using System.Collections.Generic;

namespace AsyncReactAwait.Bindable
{
    /// <summary>
    /// Aggregates four bindable values into a new bindable value.
    /// </summary>
    public sealed class BindableAggregator<T1, T2, T3, T4, TRes> : IBindable<TRes>, IBindableRaw
    {
        private readonly IBindable<T1> _b1;
        private readonly IBindable<T2> _b2;
        private readonly IBindable<T3> _b3;
        private readonly IBindable<T4> _b4;
        private readonly Func<T1, T2, T3, T4, TRes> _aggregator;

        private readonly Dictionary<Action<TRes>, int> _handlers = new();
        private readonly Dictionary<Action<object?>, int> _rawHandlers = new();
        private readonly Dictionary<Action, int> _blindHandlers = new();
        private readonly Dictionary<Action<TRes, TRes>, int> _fullHandlers = new();
        private readonly Dictionary<Action<object?, object?>, int> _rawFullHandlers = new();

        private bool _subscribed;
        private TRes? _prevValue;
        private TRes? _currentValue;
        private bool _hasValue;

        /// <inheritdoc cref="IBindable.OnAnySubscription"/>
        public event Action? OnAnySubscription;

        /// <inheritdoc cref="IBindable.OnSubscriptionsCleared"/>
        public event Action? OnSubscriptionsCleared;

        /// <summary>
        /// Aggregates four bindable values into a new bindable value.
        /// </summary>
        /// <param name="b1">First source bindable value.</param>
        /// <param name="b2">Second source bindable value.</param>
        /// <param name="b3">Third source bindable value.</param>
        /// <param name="b4">Fourth source bindable value.</param>
        /// <param name="aggregator">Aggregator function.</param>
        /// <exception cref="ArgumentNullException">Any of the arguments is null.</exception>
        public BindableAggregator(
            IBindable<T1> b1,
            IBindable<T2> b2,
            IBindable<T3> b3,
            IBindable<T4> b4,
            Func<T1, T2, T3, T4, TRes> aggregator)
        {
            _b1 = b1 ?? throw new ArgumentNullException(nameof(b1));
            _b2 = b2 ?? throw new ArgumentNullException(nameof(b2));
            _b3 = b3 ?? throw new ArgumentNullException(nameof(b3));
            _b4 = b4 ?? throw new ArgumentNullException(nameof(b4));
            _aggregator = aggregator ?? throw new ArgumentNullException(nameof(aggregator));
        }

        #region Value & compute

        /// <summary>
        /// Current aggregated value.
        /// </summary>
        public TRes Value
        {
            get
            {
                if (!_hasValue)
                {
                    Refresh();
                }

                return _currentValue!;
            }
        }

        object? IBindableRaw.Value => Value;

        private TRes Compute() => _aggregator(_b1.Value, _b2.Value, _b3.Value, _b4.Value);

        private void Refresh()
        {
            _currentValue = Compute();
            _hasValue = true;
        }

        #endregion

        #region Binding API

        /// <inheritdoc cref="IBindable{T}.Bind(Action{T}, bool)"/>
        public void Bind(Action<TRes> handler, bool callImmediately = true)
        {
            if (!_handlers.TryAdd(handler, 1))
            {
                _handlers[handler]++;
            }

            OnAnySubscription?.Invoke();
            UpdateBinding();

            if (callImmediately)
            {
                handler(Value);
            }
        }

        /// <inheritdoc cref="IBindable.Bind(Action, bool)"/>
        public void Bind(Action handler, bool callImmediately = true)
        {
            if (!_blindHandlers.TryAdd(handler, 1))
            {
                _blindHandlers[handler]++;
            }

            OnAnySubscription?.Invoke();
            UpdateBinding();

            if (callImmediately)
            {
                handler();
            }
        }

        /// <inheritdoc cref="IBindable{T}.Bind(Action{T, T})"/>
        public void Bind(Action<TRes, TRes> handler)
        {
            if (!_fullHandlers.TryAdd(handler, 1))
            {
                _fullHandlers[handler]++;
            }

            OnAnySubscription?.Invoke();
            UpdateBinding();
        }

        /// <inheritdoc cref="IBindable{T}.Unbind(Action{T})"/>
        public void Unbind(Action<TRes> handler)
        {
            if (!_handlers.ContainsKey(handler))
            {
                return;
            }

            if (--_handlers[handler] <= 0)
            {
                _handlers.Remove(handler);
            }

            UpdateBinding();
        }

        /// <inheritdoc cref="IBindable.Unbind(Action)"/>
        public void Unbind(Action handler)
        {
            if (!_blindHandlers.ContainsKey(handler))
            {
                return;
            }

            if (--_blindHandlers[handler] <= 0)
            {
                _blindHandlers.Remove(handler);
            }

            UpdateBinding();
        }

        /// <inheritdoc cref="IBindable{T}.Unbind(Action{T, T})"/>
        public void Unbind(Action<TRes, TRes> handler)
        {
            if (!_fullHandlers.ContainsKey(handler))
            {
                return;
            }

            if (--_fullHandlers[handler] <= 0)
            {
                _fullHandlers.Remove(handler);
            }

            UpdateBinding();
        }

        void IBindableRaw.Bind(Action<object?> handler, bool callImmediately)
        {
            if (!_rawHandlers.TryAdd(handler, 1))
            {
                _rawHandlers[handler]++;
            }

            OnAnySubscription?.Invoke();
            UpdateBinding();

            if (callImmediately)
            {
                handler(Value);
            }
        }

        void IBindableRaw.Bind(Action<object?, object?> handler)
        {
            if (!_rawFullHandlers.TryAdd(handler, 1))
            {
                _rawFullHandlers[handler]++;
            }

            OnAnySubscription?.Invoke();
            UpdateBinding();
        }

        void IBindableRaw.Unbind(Action<object?> handler)
        {
            if (!_rawHandlers.ContainsKey(handler))
            {
                return;
            }

            if (--_rawHandlers[handler] <= 0)
            {
                _rawHandlers.Remove(handler);
            }

            UpdateBinding();
        }

        void IBindableRaw.Unbind(Action<object?, object?> handler)
        {
            if (!_rawFullHandlers.ContainsKey(handler))
            {
                return;
            }

            if (--_rawFullHandlers[handler] <= 0)
            {
                _rawFullHandlers.Remove(handler);
            }

            UpdateBinding();
        }

        #endregion

        #region Internal subscription logic

        private bool AnyListeners()
        {
            return _handlers.Count > 0 ||
                   _rawHandlers.Count > 0 ||
                   _blindHandlers.Count > 0 ||
                   _fullHandlers.Count > 0 ||
                   _rawFullHandlers.Count > 0;
        }

        private void UpdateBinding()
        {
            if (AnyListeners())
            {
                if (!_subscribed)
                {
                    Subscribe();
                }
            }
            else if (_subscribed)
            {
                Unsubscribe();
                OnSubscriptionsCleared?.Invoke();
            }
        }

        private void Subscribe()
        {
            Refresh();
            _prevValue = _currentValue;

            _b1.Bind(Bridge1, false);
            _b1.Bind(FullBridge1);
            _b2.Bind(Bridge2, false);
            _b2.Bind(FullBridge2);
            _b3.Bind(Bridge3, false);
            _b3.Bind(FullBridge3);
            _b4.Bind(Bridge4, false);
            _b4.Bind(FullBridge4);

            _subscribed = true;
        }

        private void Unsubscribe()
        {
            _b1.Unbind(Bridge1);
            _b1.Unbind(FullBridge1);
            _b2.Unbind(Bridge2);
            _b2.Unbind(FullBridge2);
            _b3.Unbind(Bridge3);
            _b3.Unbind(FullBridge3);
            _b4.Unbind(Bridge4);
            _b4.Unbind(FullBridge4);

            _subscribed = false;
        }

        private void Bridge1(T1 _)
        {
            OnUpdate();
        }

        private void Bridge2(T2 _)
        {
            OnUpdate();
        }

        private void Bridge3(T3 _)
        {
            OnUpdate();
        }

        private void Bridge4(T4 _)
        {
            OnUpdate();
        }

        private void FullBridge1(T1 __, T1 ___)
        {
            OnUpdateFull();
        }

        private void FullBridge2(T2 __, T2 ___)
        {
            OnUpdateFull();
        }

        private void FullBridge3(T3 __, T3 ___)
        {
            OnUpdateFull();
        }

        private void FullBridge4(T4 __, T4 ___)
        {
            OnUpdateFull();
        }

        private void OnUpdate()
        {
            Refresh();
            var current = _currentValue!;

            foreach (var kv in _handlers)
            {
                for (var i = 0; i < kv.Value; i++)
                {
                    kv.Key(current);
                }
            }

            foreach (var kv in _rawHandlers)
            {
                for (var i = 0; i < kv.Value; i++)
                {
                    kv.Key(current);
                }
            }

            foreach (var kv in _blindHandlers)
            {
                for (var i = 0; i < kv.Value; i++)
                {
                    kv.Key();
                }
            }

            _prevValue = current;
        }

        private void OnUpdateFull()
        {
            var previous = _prevValue!;
            Refresh();
            var current = _currentValue!;

            foreach (var kv in _fullHandlers)
            {
                for (var i = 0; i < kv.Value; i++)
                {
                    kv.Key(previous, current);
                }
            }

            foreach (var kv in _rawFullHandlers)
            {
                for (var i = 0; i < kv.Value; i++)
                {
                    kv.Key(previous, current);
                }
            }

            _prevValue = current;
        }

        #endregion
    }
} 