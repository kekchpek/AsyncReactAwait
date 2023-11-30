using System;
using System.Threading;

namespace AsyncReactAwait.Trigger.Awaiter
{
    internal abstract class BaseTriggerAwaiter<TConcrete> : IBaseTriggerAwaiter<TConcrete>
        where TConcrete : IBaseTriggerAwaiter<TConcrete>
    {

        private readonly SynchronizationContext? _synchronizationContext;

        private bool _isCompleted;

        private bool _captureContext = true;

        private event Action? OnTriggerCompleted;

        public bool IsCompleted => _isCompleted;

        protected BaseTriggerAwaiter(SynchronizationContext? context) 
        {
            _synchronizationContext = context;
        }

        public void OnCompleted(Action continuation)
        {
            OnTriggerCompleted += continuation;
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            OnCompleted(continuation);
        }

        protected void Complete()
        {
            _isCompleted = true;
            if (_captureContext && _synchronizationContext != null) {
                _synchronizationContext.Send(_ => OnTriggerCompleted?.Invoke(), null);
            }
            else if (SynchronizationContext.Current != null)
            {
                SynchronizationContext.Current.Send(_ => OnTriggerCompleted?.Invoke(), null);
            }
            else
            {
                OnTriggerCompleted?.Invoke();
            }
        }

        public TConcrete GetAwaiter()
        {
            return GetThis();
        }

        protected abstract TConcrete GetThis();

        public TConcrete ConfigureAwaiter(bool captureContext)
        {
            _captureContext = captureContext;
            return GetThis();
        }
    }
}
