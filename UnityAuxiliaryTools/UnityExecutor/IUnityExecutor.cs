using System;

namespace UnityAuxiliaryTools.UnityExecutor
{
    public interface IUnityExecutor
    {
        void ExecuteOnFixedUpdate(Action callback);
        void ExecuteOnUpdate(Action callback);
        void ExecuteOnGui(Action callback);
    }
}