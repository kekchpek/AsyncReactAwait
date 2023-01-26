using System;
using System.Threading;
using UnityEngine;

namespace UnityAuxiliaryTools.Trigger.Awaiter
{
    internal class BaseTriggerAwaiter : IBaseTriggerAwaiter
    {

        private readonly ITriggerHandler _trigger;
        private readonly SynchronizationContext _synchronizationContext;

        private bool _isCompleted;

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
            if (_synchronizationContext != null) {
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
    }
}
