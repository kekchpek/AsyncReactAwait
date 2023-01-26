using System;
using System.Collections.Generic;
using System.Threading;
using UnityAuxiliaryTools.Promises.Awaiter;

namespace UnityAuxiliaryTools.Promises
{

    /// <inheritdoc cref="IControllablePromise"/>
    public class ControllablePromise : BaseControllablePromise, IControllablePromise
    {

        private readonly IList<Action> _successCallbacks = new List<Action>();

        /// <inheritdoc cref="IControllablePromise.Success"/>
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

        /// <inheritdoc cref="IPromise.OnSuccess(Action)"/>
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

        /// <inheritdoc cref="IPromise.GetAwaiter"/>
        public IPromiseAwaiter GetAwaiter()
        {
            return new PromiseAwaiter(this, SynchronizationContext.Current);
        }

        /// <inheritdoc cref="IPromise.ConfigureAwait"/>
        public IConfiguredPromiseAwaiterContainer ConfigureAwait(bool continueOnCapturedContext)
        {
            if (continueOnCapturedContext)
                return new ConfiguredPromiseAwaiterContainer(GetAwaiter());
            else
                return new ConfiguredPromiseAwaiterContainer(new PromiseAwaiter(this, null));
        }

        /// <inheritdoc cref="IPromise.ThrowIfFailed"/>
        public void ThrowIfFailed()
        {
            if (FailException != null)
                throw FailException;
        }
    }

    /// <inheritdoc cref="IControllablePromise{T}"/>
    public class ControllablePromise<T> : BaseControllablePromise, IControllablePromise<T>
    {
        private readonly IList<Action<T>> _successCallbacks = new List<Action<T>>();

        private T _result;
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
                    callback?.Invoke(_result);
                }
                else
                {
                    _successCallbacks.Add(callback);
                }
            }

            return this;
        }

        /// <inheritdoc cref="IPromise{T}.TryGetResult(out T)"/>
        public bool TryGetResult(out T result)
        {
            if (FailException != null)
                throw FailException;
            result = _result;
            return _resultSet;
        }

        /// <inheritdoc cref="IPromise{T}.GetAwaiter"/>
        public IPromiseAwaiter<T> GetAwaiter()
        {
            return new PromiseAwaiter<T>(this, SynchronizationContext.Current);
        }

        /// <inheritdoc cref="IPromise{T}.ConfigureAwait(bool)"/>
        public IConfiguredPromiseAwaiterContainer<T> ConfigureAwait(bool continueOnCapturedContext)
        {
            if (continueOnCapturedContext)
                return new ConfiguredPromiseAwaiterContainer<T>(GetAwaiter());
            else
                return new ConfiguredPromiseAwaiterContainer<T>(new PromiseAwaiter<T>(this, null));
        }
    }
}