using System;
using System.Collections;

namespace UnityAuxiliaryTools.UnityExecutor
{
    public interface IUnityExecutor
    {
        void ExcuteCoroutine(IEnumerator coroutine);
        void ExecuteOnFixedUpdate(Action callback);
        void ExecuteOnUpdate(Action callback);
        void ExecuteOnGui(Action callback);
    }
}