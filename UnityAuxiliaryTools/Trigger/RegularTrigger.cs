using System;
using UnityAuxiliaryTools.Trigger.Factory;

namespace UnityAuxiliaryTools.Trigger
{
    public class RegularTrigger : IRegularTrigger
    {
        public event Action OnTriggered;
        public void Trigger()
        {
            OnTriggered?.Invoke();   
        }
    }
}