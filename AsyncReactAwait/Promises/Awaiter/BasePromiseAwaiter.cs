using System;
using System.Diagnostics;
using System.Threading;
using AsyncReactAwait.Logging;

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
            Logger.Log($"BasePromiseAwaiter constructor sourcePromise = {sourcePromise?.GetHashCode()}");
            Logger.Log($"BasePromiseAwaiter constructor syncContext = {syncContext?.GetHashCode()}");
            _sourcePromise = sourcePromise;
            _syncContext = syncContext;
        }

        public bool IsCompleted => _sourcePromise.IsCompleted;

        public T ConfigureAwaiter(bool captureContext)
        {
            Logger.Log($"ConfigureAwaiter captureContext = {captureContext}");
            _captureContext = captureContext;
            return GetAwaiter();
        }

        public abstract T GetAwaiter();

        public void OnCompleted(Action continuation)
        {
            Logger.Log($"OnCompleted continuation = {continuation?.GetHashCode()}");
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
            Logger.Log($"UnsafeOnCompleted continuation = {continuation?.GetHashCode()}");
            OnCompleted(continuation);
        }
    }
}
