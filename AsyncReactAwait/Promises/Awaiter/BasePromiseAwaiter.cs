using System;
using System.Threading;

namespace AsyncReactAwait.Promises.Awaiter
{
    internal abstract class BasePromiseAwaiter<T> : IBasePromiseAwaiter<T>
        where T : IBasePromiseAwaiter<T>
    {

        private readonly IBasePromise _sourcePromise;
        private readonly SynchronizationContext _syncContext;

        private bool _captureContext = true;

        protected BasePromiseAwaiter(IBasePromise sourcePromise, SynchronizationContext syncContext)
        {
            _sourcePromise = sourcePromise;
            _syncContext = syncContext;
        }

        public bool IsCompleted => _sourcePromise.IsCompleted;

        public T ConfigureAwaiter(bool captureContext)
        {
            _captureContext = captureContext;
            return GetAwaiter();
        }

        public abstract T GetAwaiter();

        public void OnCompleted(Action continuation)
        {
            if (continuation == null)
            {
                return;
            }
            _sourcePromise.Finally(() =>
            {
                if (_syncContext != null && _captureContext)
                {
                    _syncContext.Send(_ => continuation?.Invoke(), null);
                }
                else
                {
                    if (SynchronizationContext.Current != null)
                    {
                        SynchronizationContext.Current.Send(_ => continuation?.Invoke(), null);
                    }
                    else
                    {
                        continuation?.Invoke();
                    }
                }
            });
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            OnCompleted(continuation);
        }
    }
}
