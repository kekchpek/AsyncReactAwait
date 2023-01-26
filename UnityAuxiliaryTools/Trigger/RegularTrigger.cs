using System;

namespace UnityAuxiliaryTools.Trigger
{
    public class RegularTrigger : IRegularTrigger
    {
        public event Action Triggered;
        public void Trigger()
        {
            Triggered?.Invoke();   
        }
    }
    public class RegularTrigger<T> : IRegularTrigger<T>
    {
        public event Action<T> Triggered;
        public void Trigger(T obj)
        {
            Triggered?.Invoke(obj);
        }
    }
}