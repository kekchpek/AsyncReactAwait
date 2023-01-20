using System;

namespace UnityAuxiliaryTools.Trigger
{
    public interface ITriggerHandler
    {
        event Action Triggered;
    }
    public interface ITriggerHandler<T>
    {
        event Action<T> Triggered;
    }
}