using System;
using System.Runtime.CompilerServices;
using AsyncReactAwait.Logging;

namespace AsyncReactAwait.Promises.AsyncMethodBuilder
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public struct AsyncPromiseMethodBuilder
    {

        public static AsyncPromiseMethodBuilder Create()
        {
            Logger.Log($"Create");
            return new AsyncPromiseMethodBuilder();
        }

        private IControllablePromise _promise;

        public IPromise Task => TaskInternal;

        private IControllablePromise TaskInternal => _promise ??= new ControllablePromise();

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            Logger.Log($"State machine set = {stateMachine}");
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            Logger.Log($"Move next state machine = {stateMachine?.GetHashCode()}");
            stateMachine.MoveNext();
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            Logger.Log($"AwaitUnsafeOnCompleted state machine = {stateMachine?.GetHashCode()}");
            Logger.Log($"AwaitUnsafeOnCompleted state machine = {awaiter?.GetHashCode()}");
            awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            Logger.Log($"AwaitOnCompleted state machine = {stateMachine?.GetHashCode()}");
            Logger.Log($"AwaitOnCompleted state machine = {awaiter?.GetHashCode()}");
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        public void SetException(Exception exception)
        {
            Logger.Log($"Set exception = {exception?.Message}");
            TaskInternal.Fail(exception);
        }

        public void SetResult()
        {
            Logger.Log($"Set result");
            TaskInternal.Success();
        }

    }


    public struct AsyncPromiseMethodBuilder<T>
    {

        public static AsyncPromiseMethodBuilder<T> Create()
        {
            return new AsyncPromiseMethodBuilder<T>();
        }

        private IControllablePromise<T> _promise;
        
        public IPromise<T> Task => TaskInternal;

        private IControllablePromise<T> TaskInternal => _promise ??= new ControllablePromise<T>();

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
            TaskInternal.Fail(exception);
        }

        public void SetResult(T result)
        {
            TaskInternal.Success(result);
        }

    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

}
