using System;
using System.Threading;

namespace AsyncReactAwait.Bindable.Awaiter
{
    internal class BindableAwaiter<T> : IBindableAwaiter<T>
    {

        private readonly IBindable<T> _bindable;
        private readonly Func<T, bool> _predicate;
        private readonly SynchronizationContext _syncContext;

        private event Action _completed;

        private bool _captureContext = true;

        private bool _isCompleted;
        private T _awaitedValue;

        public bool IsCompleted { get; private set; }

        public BindableAwaiter(IBindable<T> bindable, SynchronizationContext context, Func<T, bool> predicate,
            bool checkCurrentValue = true)
        {
            _bindable = bindable;
            _predicate = predicate;
            _syncContext = context;

            _bindable.Bind(OnValueChanged, checkCurrentValue);
        }

        private void OnValueChanged(T val)
        {
            if (_predicate == null || _predicate.Invoke(val))
            {
                _bindable.Unbind(OnValueChanged);
                _isCompleted = true;
                _awaitedValue = val;
                if (_captureContext && _syncContext != null)
                {
                    _syncContext.Send(_ => _completed?.Invoke(), null);
                }
                else if (SynchronizationContext.Current != null)
                {
                    SynchronizationContext.Current.Send(_ => _completed?.Invoke(), null);
                }
                else
                {
                    _completed?.Invoke();
                }
            }
        }

        public T GetResult()
        {
            if (!_isCompleted)
            {
                throw new Exception("Operation is not completed!");
            }
            return _awaitedValue;
        }

        public IBindableAwaiter<T> ConfigureAwaiter(bool captureContext)
        {
            _captureContext = captureContext;
            return this;
        }

        public IBindableAwaiter<T> GetAwaiter()
        {
            return this;
        }

        public void OnCompleted(Action continuation)
        {
            _completed += continuation;
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            _completed += continuation;
        }
    }
}
