using System;
using System.Threading;
using AsyncReactAwait.Logging;

namespace AsyncReactAwait.Promises.Awaiter
{
    internal class PromiseAwaiter : BasePromiseAwaiter<IPromiseAwaiter>, IPromiseAwaiter
    {

        private IPromise _sourcePromise;

        public PromiseAwaiter(IPromise promise, SynchronizationContext capturedContext)
            : base(promise, capturedContext)
        {
            Logger.Log($"PromiseAwaiter constructor promise = {promise?.GetHashCode()}");
            Logger.Log($"PromiseAwaiter constructor capturedContext = {capturedContext?.GetHashCode()}");
            _sourcePromise = promise;
        }

        public override IPromiseAwaiter GetAwaiter()
        {
            Logger.Log($"GetAwaiter");
            return this;
        }

        public void GetResult() 
        {
            Logger.Log($"GetResult");
            _sourcePromise.ThrowIfFailed();
        }
    }

    internal class PromiseAwaiter<T> : BasePromiseAwaiter<IPromiseAwaiter<T>>, IPromiseAwaiter<T>
    {

        private readonly IPromise<T> _sourcePromise;

        public PromiseAwaiter(IPromise<T> promise, SynchronizationContext capturedContext)
            :base(promise, capturedContext)
        {
            _sourcePromise = promise;
        }

        public override IPromiseAwaiter<T> GetAwaiter()
        {
            return this;
        }

        public T GetResult()
        {
            if (_sourcePromise.TryGetResult(out var res))
            {
                return res;
            }
            throw new Exception("Can not obtain promise result!");
        }
    }
}
