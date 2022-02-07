using System;
using NSubstitute;
using NUnit.Framework;
using UnityAuxiliaryTools.Promises;
using UnityAuxiliaryTools.UnityExecutor;

namespace UnityAuxiliartyTools.Tests.Promises
{
    public class GenericControllablePromiseTests
    {

        private ControllablePromise<object> CreatePromise(out IUnityExecutor unityExecutor)
        {
            unityExecutor = Substitute.For<IUnityExecutor>();
            return new ControllablePromise<object>(unityExecutor);
        }
        
        [Test]
        public void OnSuccess_ReturnsItself()
        {
            // Arrange
            var promise = CreatePromise(out var unityExecutor);
            
            // Act
            var retVal = promise.OnSuccess(x => { });
            
            // Assert
            Assert.AreEqual(promise, retVal);
        }
        
        [Test]
        public void Success_OnSuccessCallback_SubmittedToExecute()
        {
            // Arrange
            var promise = CreatePromise(out var unityExecutor);
            var someObject = new object();
            object successResult = null;
            var callback = new Action<object>(x =>
            {
                successResult = x;
            });
            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));
            
            // Act
            promise.OnSuccess(callback);
            promise.Success(someObject);
            
            // Assert
            Assert.AreEqual(someObject, successResult);
        }
        
        [Test]
        public void Success_OnSuccessBefore_SubmittedToExecute()
        {
            // Arrange
            var promise = CreatePromise(out var unityExecutor);
            var someObject = new object();
            object successResult = null;
            var callback = new Action<object>(x =>
            {
                successResult = x;
            });
            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));
            
            // Act
            promise.Success(someObject);
            promise.OnSuccess(callback);
            
            // Assert
            Assert.AreEqual(someObject, successResult);
        }
        
        [Test]
        public void Success_FinallyCallback_SubmittedToExecute()
        {
            // Arrange
            var promise = CreatePromise(out var unityExecutor);
            var isExecuted = false;
            var callback = new Action(() =>
            {
                isExecuted = true;
            });
            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));
            
            // Act
            promise.Finally(callback);
            promise.Success(new object());
            
            // Assert
            Assert.IsTrue(isExecuted);
        }
        
        [Test]
        public void Success_FinallyBefore_SubmittedToExecute()
        {
            // Arrange
            var promise = CreatePromise(out var unityExecutor);
            var isExecuted = false;
            var callback = new Action(() =>
            {
                isExecuted = true;
            });
            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));
            
            // Act
            promise.Success(new object());
            promise.Finally(callback);
            
            // Assert
            Assert.IsTrue(isExecuted);
        }

        public void SuccessTwice_ExceptionThrown()
        {
            // Arrange 
            var promise = CreatePromise(out var unityExecutor);
            promise.Success(new object());

            // Act
            // no act
            
            // Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                promise.Success(new object());
            });
        }
    }
}