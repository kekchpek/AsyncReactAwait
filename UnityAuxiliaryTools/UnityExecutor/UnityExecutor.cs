using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityAuxiliaryTools.UnityExecutor
{
    public class UnityExecutor : MonoBehaviour, IUnityExecutor
    {

        private readonly Queue<Action> _fixedUpdateCallbacks = new Queue<Action>();
        private readonly Queue<Action> _updateCallbacks = new Queue<Action>();
        private readonly Queue<Action> _guiCallbacks = new Queue<Action>();

        public void ExecuteOnFixedUpdate(Action callback)
        {
            CheckCallbackForNull(callback);
            _fixedUpdateCallbacks.Enqueue(callback);
        }

        public void ExecuteOnUpdate(Action callback)
        {
            CheckCallbackForNull(callback);
            _updateCallbacks.Enqueue(callback);
        }

        public void ExecuteOnGui(Action callback)
        {
            CheckCallbackForNull(callback);
            _guiCallbacks.Enqueue(callback);
        }

        private void FixedUpdate()
        {
            while (_fixedUpdateCallbacks.Any())
            {
                _fixedUpdateCallbacks.Dequeue()?.Invoke();
            }
        }

        private void Update()
        {
            while (_updateCallbacks.Any())
            {
                _updateCallbacks.Dequeue()?.Invoke();
            }
        }

        private void OnGUI()
        {
            while (_guiCallbacks.Any())
            {
                _guiCallbacks.Dequeue()?.Invoke();
            }
        }

        private void CheckCallbackForNull(Action callback)
        {
            if (callback == null)
                Debug.LogWarning("Null callback was submitted to execute");
        }
    }
}