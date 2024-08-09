using System;
using System.Collections.Generic;
using System.Threading;
using AsyncReactAwait.Promises.Awaiter;

namespace AsyncReactAwait.Promises
{

    /// <inheritdoc cref="IControllablePromise"/>
    [Obsolete]
    public class ControllablePromise : BaseControllablePromise, IControllablePromise
    {

        private readonly IList<Action> _successCallbacks = new List<Action>();

        /// <inheritdoc cref="IControllablePromise.Success"/>
        public void Success()
        {
            lock (this)
            {
                if (IsCompleted)
                {
                    throw new InvalidOperationException("Promise is already completed!");
                }

                foreach (var callback in _successCallbacks)
                {
                    callback?.Invoke();
                }

                DoFinally();
            }
        }

        /// <inheritdoc cref="IPromise.OnSuccess(Action)"/>
        public IPromise OnSuccess(Action callback)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            lock (this)
            {
                if (IsCompleted)
                {
                    callback.Invoke();
                }
                else
                {
                    _successCallbacks.Add(callback);
                }
            }

            return this;
        }

        /// <inheritdoc cref="IPromise.GetAwaiter"/>
        public IPromiseAwaiter GetAwaiter()
        {
            return new PromiseAwaiter(this, SynchronizationContext.Current);
        }

        /// <inheritdoc cref="IPromise.ConfigureAwait"/>
        public IPromiseAwaiter ConfigureAwait(bool continueOnCapturedContext)
        {
            return GetAwaiter().ConfigureAwaiter(continueOnCapturedContext);
        }

        /// <inheritdoc cref="IPromise.ThrowIfFailed"/>
        public void ThrowIfFailed()
        {
            if (FailException != null)
                throw new Exception("Promise failed", FailException);
        }
    }

    /// <inheritdoc cref="IControllablePromise{T}"/>
    [Obsolete]
    public class ControllablePromise<T> : BaseControllablePromise, IControllablePromise<T>
    {
        private readonly IList<Action<T>> _successCallbacks = new List<Action<T>>();

        private T? _result;
        private bool _resultSet;

        /// <inheritdoc cref="IControllablePromise{T}.Success(T)"/>
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

        /// <inheritdoc cref="IPromise{T}.OnSuccess(Action{T})"/>
        public IPromise<T> OnSuccess(Action<T> callback)
        {
            lock (this)
            {
                if (_resultSet)
                {
                    callback.Invoke(_result!);
                }
                else
                {
                    _successCallbacks.Add(callback);
                }
            }

            return this;
        }

        /// <inheritdoc cref="IPromise{T}.TryGetResult(out T)"/>
        public bool TryGetResult(out T? result)
        {
            if (FailException != null)
                throw new Exception("Promise failed!", FailException);
            result = _result;
            return _resultSet;
        }

        /// <inheritdoc cref="IPromise{T}.GetAwaiter"/>
        public IPromiseAwaiter<T> GetAwaiter()
        {
            return new PromiseAwaiter<T>(this, SynchronizationContext.Current);
        }

        /// <inheritdoc cref="IPromise{T}.ConfigureAwait(bool)"/>
        public IPromiseAwaiter<T> ConfigureAwait(bool continueOnCapturedContext)
        {
            return GetAwaiter().ConfigureAwaiter(continueOnCapturedContext);
        }
    }
}