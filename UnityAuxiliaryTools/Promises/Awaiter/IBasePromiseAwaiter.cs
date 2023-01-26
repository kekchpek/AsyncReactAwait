﻿using System.Runtime.CompilerServices;

namespace UnityAuxiliaryTools.Promises.Awaiter
{
    /// <summary>
    /// Base API for promise awaiter.
    /// </summary>
    public interface IBasePromiseAwaiter : INotifyCompletion
    {
        /// <summary>
        /// Indicates that promise is completed.
        /// </summary>
        bool IsCompleted { get; }

    }
}