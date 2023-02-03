using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AsyncReactAwait.Promises
{

    /// <inheritdoc cref="IBaseControllablePromise"/>
    public abstract class BaseControllablePromise : IBaseControllablePromise
    {

        private readonly IList<Action<Exception>> _failCallbacks = new List<Action<Exception>>();
        private readonly IList<Action> _finallyCallbacks = new List<Action>();

        private Exception _failingError;

        /// <summary>
        /// An exception that cause promise fail. 
        /// </summary>
        [AllowNull]
        protected Exception FailException => _failingError;

        /// <inheritdoc cref="IBasePromise.IsCompleted"/>
        public bool IsCompleted { get; private set; }

        /// <inheritdoc cref="IBasePromise.OnFail(Action{Exception})"/>
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

        /// <inheritdoc cref="IBasePromise.Finally(Action)"/>
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

        /// <inheritdoc cref="IBaseControllablePromise.Fail(Exception)"/>
        public void Fail(Exception error)
        {
            lock (this)
            {
                if (IsCompleted)
                    throw new InvalidOperationException("Promise is already completed!");
                _failingError = error ?? new Exception("The null was passed to the promise as an exception");
                foreach (var callback in _failCallbacks)
                {
                    callback?.Invoke(_failingError);
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
                callback?.Invoke();
            }
        }
    }
}