using System;
using System.Threading;

namespace AsyncReactAwait.Trigger.Awaiter
{
    internal abstract class BaseTriggerAwaiter<TConcrete> : IBaseTriggerAwaiter<TConcrete>
        where TConcrete : IBaseTriggerAwaiter<TConcrete>
    {

        private readonly ITriggerHandler _trigger;
        private readonly SynchronizationContext _synchronizationContext;

        private bool _isCompleted;

        private bool _captureContext;

        private event Action _onTriggerCompleted;

        public bool IsCompleted => _isCompleted;

        public BaseTriggerAwaiter(ITriggerHandler trigger, SynchronizationContext context) 
        {
            _trigger = trigger;
            _synchronizationContext = context;
            _trigger.Triggered += Complete;
        }

        public void OnCompleted(Action continuation)
        {
            _onTriggerCompleted += continuation;
        }

        private void Complete()
        {
            _trigger.Triggered -= Complete;
            _isCompleted = true;
            if (_captureContext) {
                _synchronizationContext.Send(_ => _onTriggerCompleted?.Invoke(), null);
            }
            else if (SynchronizationContext.Current != null)
            {
                SynchronizationContext.Current.Send(_ => _onTriggerCompleted?.Invoke(), null);
            }
            else
            {
                _onTriggerCompleted?.Invoke();
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
