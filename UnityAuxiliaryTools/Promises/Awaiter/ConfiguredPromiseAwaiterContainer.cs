using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAuxiliaryTools.Promises.Awaiter
{
    internal class ConfiguredPromiseAwaiterContainer : IConfiguredPromiseAwaiterContainer
    {

        private readonly IPromiseAwaiter _promiseAwaiter;

        public ConfiguredPromiseAwaiterContainer(IPromiseAwaiter awaiter) 
        {
            _promiseAwaiter = awaiter;
        }

        public IPromiseAwaiter GetAwaiter()
        {
            return _promiseAwaiter;
        }
    }
    internal class ConfiguredPromiseAwaiterContainer<T> : IConfiguredPromiseAwaiterContainer<T>
    {

        private readonly IPromiseAwaiter<T> _promiseAwaiter;

        public ConfiguredPromiseAwaiterContainer(IPromiseAwaiter<T> awaiter)
        {
            _promiseAwaiter = awaiter;
        }

        public IPromiseAwaiter<T> GetAwaiter()
        {
            return _promiseAwaiter;
        }
    }
}
