using System;
using System.Collections.Generic;
using UnityAuxiliaryTools.UnityExecutor;

namespace UnityAuxiliaryTools.Promises
{
    public abstract class BaseControllablePromise : IBaseControllablePromise
    {
        private readonly IUnityExecutor _unityExecutor;

        private readonly IList<Action<Exception>> _failCallbacks = new List<Action<Exception>>();
        private readonly IList<Action> _finallyCallbacks = new List<Action>();

        private Exception _failingError;
        protected bool IsCompleted { get; private set; }
        
        protected BaseControllablePromise(IUnityExecutor unityExecutor)
        {
            _unityExecutor = unityExecutor;
        }
        
        public IBasePromise OnFail(Action<Exception> callback)
        {
            lock (this)
            {
                if (_failingError != null)
                {
                    callback?.Invoke(_failingError);
                }
                else
                {
                    _failCallbacks.Add(callback);
                }
            }

            return this;
            
        }

        public IBasePromise Finally(Action callback)
        {
            lock (this)
            {
                if (IsCompleted)
                {
                    callback?.Invoke();
                }
                else
                {
                    _finallyCallbacks.Add(callback);
                }
            }

            return this;
        }

        public void Fail(Exception error)
        {
            lock (this)
            {
                if (IsCompleted)
                    throw new InvalidOperationException("Promise is already completed!");
                _failingError = error ?? new Exception("The null was passed to the promise as an exception");
                foreach (var callback in _failCallbacks)
                {
                    _unityExecutor.ExecuteOnFixedUpdate(() => callback?.Invoke(_failingError));
                }

                DoFinally();
            }
        }

        /// <summary>
        /// Should be called when the promise is completed.
        /// </summary>
        protected void DoFinally()
        {
            IsCompleted = true;
            foreach (var callback in _finallyCallbacks)
            {
                _unityExecutor.ExecuteOnFixedUpdate(() => callback?.Invoke());
            }
        }
    }
}