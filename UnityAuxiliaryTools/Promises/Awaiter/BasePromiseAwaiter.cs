using System;
using System.Threading;

namespace UnityAuxiliaryTools.Promises.Awaiter
{
    internal abstract class BasePromiseAwaiter : IBasePromiseAwaiter
    {

        private readonly IBasePromise _sourcePromise;
        private readonly SynchronizationContext _syncContext;

        protected BasePromiseAwaiter(IBasePromise sourcePromise, SynchronizationContext sincConnext)
        {
            _sourcePromise = sourcePromise;
            _syncContext = sincConnext;
        }

        public bool IsCompleted => _sourcePromise.IsCompleted;


        public void OnCompleted(Action continuation)
        {
            if (continuation == null)
            {
                return;
            }
            _sourcePromise.Finally(() =>
            {
                if (_syncContext != null)
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

    }
}
