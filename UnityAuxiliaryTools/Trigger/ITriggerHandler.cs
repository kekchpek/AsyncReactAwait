using System;

namespace UnityAuxiliaryTools.Trigger
{
    public interface ITriggerHandler
    {
        event Action OnTriggered;
    }
}