using System;
using System.Collections.Generic;

namespace UnityAuxiliaryTools.Promises
{
    public class ControllablePromise : BaseControllablePromise, IControllablePromise
    {

        private readonly IList<Action> _successCallbacks = new List<Action>();

        public void Success()
        {
            lock (this)
            {
                if (IsCompleted)
                    throw new InvalidOperationException("Promise is already completed!");
                foreach (var callback in _successCallbacks)
                {
                    callback?.Invoke();
                }

                DoFinally();
            }
        }

        public IPromise OnSuccess(Action callback)
        {
            lock (this)
            {
                if (IsCompleted)
                {
                    callback?.Invoke();
                }
                else
                {
                    _successCallbacks.Add(callback);
                }
            }

            return this;
        }
    }
    
    public class ControllablePromise<T> : BaseControllablePromise, IControllablePromise<T>
    {
        private readonly IList<Action<T>> _successCallbacks = new List<Action<T>>();

        private T _result;
        private bool _resultSet;
        
        public void Success(T result)
        {
            lock (this)
            {
                if (IsCompleted)
                    throw new InvalidOperationException("Promise is already completed!");
                _result = result;
                _resultSet = true;
                foreach (var callback in _successCallbacks)
                {
                    callback?.Invoke(_result);
                }

                DoFinally();
            }
        }

        public IPromise<T> OnSuccess(Action<T> callback)
        {
            lock (this)
            {
                if (_resultSet)
                {
                    callback?.Invoke(_result);
                }
                else
                {
                    _successCallbacks.Add(callback);
                }
            }

            return this;
        }
    }
}