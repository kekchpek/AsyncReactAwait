using System;
using System.Runtime.CompilerServices;

namespace AsyncReactAwait.Promises.AsyncMethodBuilder
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [Obsolete]
    public struct AsyncPromiseMethodBuilder
    {
        public static AsyncPromiseMethodBuilder Create()
        {
            return new AsyncPromiseMethodBuilder()
            {
                _promise = new ControllablePromise(),
            };
        }

        private IControllablePromise _promise;

        public IPromise Task => _promise;


        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            // do nothing
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
            awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
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


    [Obsolete]
    public struct AsyncPromiseMethodBuilder<T>
    {

        public static AsyncPromiseMethodBuilder<T> Create()
        {
            return new AsyncPromiseMethodBuilder<T>()
            {
                _promise = new ControllablePromise<T>(),
            };
        }

        private IControllablePromise<T> _promise;
        
        public IPromise<T> Task => _promise;

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
            awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
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
