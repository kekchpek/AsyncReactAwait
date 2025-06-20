using System;
using System.Collections.Generic;

namespace AsyncReactAwait.Bindable
{
    /// <summary>
    /// Aggregates two bindable values into a new bindable value.
    /// </summary>
    /// <typeparam name="T1">Type of first source value.</typeparam>
    /// <typeparam name="T2">Type of second source value.</typeparam>
    /// <typeparam name="TRes">Resulting aggregated type.</typeparam>
    public sealed class BindableAggregator<T1, T2, TRes> : IBindable<TRes>, IBindableRaw
    {
        private readonly IBindable<T1> _b1;
        private readonly IBindable<T2> _b2;
        private readonly Func<T1, T2, TRes> _aggregator;

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
        /// Aggregates two bindable values into a new bindable value.
        /// </summary>
        /// <param name="b1">First source bindable value.</param>
        /// <param name="b2">Second source bindable value.</param>
        /// <param name="aggregator">Aggregator function.</param>
        /// <exception cref="ArgumentNullException">Any of the arguments is null.</exception>
        public BindableAggregator(IBindable<T1> b1, IBindable<T2> b2, Func<T1, T2, TRes> aggregator)
        {
            _b1 = b1 ?? throw new ArgumentNullException(nameof(b1));
            _b2 = b2 ?? throw new ArgumentNullException(nameof(b2));
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
                    RefreshCachedValue();
                }
                return _currentValue!;
            }
        }

        object? IBindableRaw.Value => Value;

        private TRes ComputeCurrent() => _aggregator(_b1.Value, _b2.Value);

        private void RefreshCachedValue()
        {
            _currentValue = ComputeCurrent();
            _hasValue = true;
        }
        #endregion

        #region Binding API

        /// <inheritdoc cref="IBindable{T}.Bind(Action{T}, bool)"/>
        public void Bind(Action<TRes> handler, bool callImmediately = true)
        {
            if (!_handlers.TryAdd(handler, 1))
                _handlers[handler]++;
            OnAnySubscription?.Invoke();
            UpdateSourceBinding();
            if (callImmediately) handler(Value);
        }

        /// <inheritdoc cref="IBindable.Bind(Action, bool)"/>
        public void Bind(Action handler, bool callImmediately = true)
        {
            if (!_blindHandlers.TryAdd(handler, 1))
                _blindHandlers[handler]++;
            OnAnySubscription?.Invoke();
            UpdateSourceBinding();
            if (callImmediately) handler();
        }

        /// <inheritdoc cref="IBindable{T}.Bind(Action{T, T})"/>
        public void Bind(Action<TRes, TRes> handler)
        {
            if (!_fullHandlers.TryAdd(handler, 1))
                _fullHandlers[handler]++;
            OnAnySubscription?.Invoke();
            UpdateSourceBinding();
        }

        /// <inheritdoc cref="IBindable{T}.Unbind(Action{T})"/>
        public void Unbind(Action<TRes> handler)
        {
            if (!_handlers.ContainsKey(handler)) return;
            if (--_handlers[handler] <= 0) _handlers.Remove(handler);
            UpdateSourceBinding();
        }

        /// <inheritdoc cref="IBindable.Unbind(Action)"/>
        public void Unbind(Action handler)
        {
            if (!_blindHandlers.ContainsKey(handler)) return;
            if (--_blindHandlers[handler] <= 0) _blindHandlers.Remove(handler);
            UpdateSourceBinding();
        }

        /// <inheritdoc cref="IBindable{T}.Unbind(Action{T, T})"/>
        public void Unbind(Action<TRes, TRes> handler)
        {
            if (!_fullHandlers.ContainsKey(handler)) return;
            if (--_fullHandlers[handler] <= 0) _fullHandlers.Remove(handler);
            UpdateSourceBinding();
        }

        void IBindableRaw.Bind(Action<object?> handler, bool callImmediately)
        {
            if (!_rawHandlers.TryAdd(handler, 1))
                _rawHandlers[handler]++;
            OnAnySubscription?.Invoke();
            UpdateSourceBinding();
            if (callImmediately) handler(Value);
        }

        void IBindableRaw.Bind(Action<object?, object?> handler)
        {
            if (!_rawFullHandlers.TryAdd(handler, 1))
                _rawFullHandlers[handler]++;
            OnAnySubscription?.Invoke();
            UpdateSourceBinding();
        }

        void IBindableRaw.Unbind(Action<object?> handler)
        {
            if (!_rawHandlers.ContainsKey(handler)) return;
            if (--_rawHandlers[handler] <= 0) _rawHandlers.Remove(handler);
            UpdateSourceBinding();
        }

        void IBindableRaw.Unbind(Action<object?, object?> handler)
        {
            if (!_rawFullHandlers.ContainsKey(handler)) return;
            if (--_rawFullHandlers[handler] <= 0) _rawFullHandlers.Remove(handler);
            UpdateSourceBinding();
        }
        #endregion

        #region Internal subscription logic
        private void UpdateSourceBinding()
        {
            if (HasAnyListeners())
            {
                if (!_subscribed)
                {
                    Subscribe();
                }
            }
            else
            {
                if (_subscribed)
                {
                    Unsubscribe();
                    OnSubscriptionsCleared?.Invoke();
                }
            }
        }

        private bool HasAnyListeners() => _handlers.Count > 0 || _rawHandlers.Count > 0 || _blindHandlers.Count > 0 || _fullHandlers.Count > 0 || _rawFullHandlers.Count > 0;

        private void Subscribe()
        {
            RefreshCachedValue();
            _prevValue = _currentValue;
            _b1.Bind(OnSourceUpdated1, false);
            _b1.Bind(OnSourceUpdatedFull1);
            _b2.Bind(OnSourceUpdated2, false);
            _b2.Bind(OnSourceUpdatedFull2);
            _subscribed = true;
        }

        private void Unsubscribe()
        {
            _b1.Unbind(OnSourceUpdated1);
            _b1.Unbind(OnSourceUpdatedFull1);
            _b2.Unbind(OnSourceUpdated2);
            _b2.Unbind(OnSourceUpdatedFull2);
            _subscribed = false;
        }

        private void OnSourceUpdated1(T1 _) => OnSourceUpdated();
        private void OnSourceUpdated2(T2 _) => OnSourceUpdated();
        private void OnSourceUpdatedFull1(T1 __, T1 ___) => OnSourceUpdatedFull();
        private void OnSourceUpdatedFull2(T2 __, T2 ___) => OnSourceUpdatedFull();

        private void OnSourceUpdated()
        {
            RefreshCachedValue();
            var current = _currentValue!;
            foreach (var kv in _handlers) for (int i=0;i<kv.Value;i++) kv.Key(current);
            foreach (var kv in _rawHandlers) for (int i=0;i<kv.Value;i++) kv.Key(current);
            foreach (var kv in _blindHandlers) for (int i=0;i<kv.Value;i++) kv.Key();
            _prevValue = current;
        }

        private void OnSourceUpdatedFull()
        {
            var previous = _prevValue!;
            RefreshCachedValue();
            var current = _currentValue!;
            foreach (var kv in _fullHandlers) for (int i=0;i<kv.Value;i++) kv.Key(previous, current);
            foreach (var kv in _rawFullHandlers) for (int i=0;i<kv.Value;i++) kv.Key(previous, current);
            _prevValue = current;
        }
        #endregion
    }
} 