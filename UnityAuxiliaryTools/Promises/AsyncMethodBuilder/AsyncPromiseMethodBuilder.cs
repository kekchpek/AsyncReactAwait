using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityAuxiliaryTools.Promises.AsyncMethodBuilder
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public struct AsyncPromiseMethodBuilder
    {

        public static AsyncPromiseMethodBuilder Create()
        {
            return new AsyncPromiseMethodBuilder();
        }

        private IControllablePromise _promise;

        public IPromise Task
        {
            get
            {
                return _promise ??= new ControllablePromise();
            }
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        public void SetException(Exception exception)
        {
            _promise.Fail(exception);
        }

        public void SetResult()
        {
            _promise.Success();
        }

    }


    public struct AsyncPromiseMethodBuilder<T>
    {

        public static AsyncPromiseMethodBuilder<T> Create()
        {
            return new AsyncPromiseMethodBuilder<T>();
        }

        private IControllablePromise<T> _promise;

        public IPromise<T> Task
        {
            get
            {
                return _promise ??= new ControllablePromise<T>();
            }
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        public void SetException(Exception exception)
        {
            _promise.Fail(exception);
        }

        public void SetResult(T result)
        {
            _promise.Success(result);
        }

    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

}
