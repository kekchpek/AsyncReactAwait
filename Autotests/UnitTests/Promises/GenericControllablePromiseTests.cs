using System;
using NSubstitute;
using NUnit.Framework;
using UnityAuxiliaryTools.Promises;
using UnityAuxiliaryTools.UnityExecutor;

namespace UnityAuxiliartyTools.Tests.Promises
{
    public class GenericControllablePromiseTests
    {

        private ControllablePromise<object> CreatePromise()
        {
            return new ControllablePromise<object>();
        }
        
        [Test]
        public void OnSuccess_ReturnsItself()
        {
            // Arrange
            var promise = CreatePromise();
            
            // Act
            var retVal = promise.OnSuccess(x => { });
            
            // Assert
            Assert.AreEqual(promise, retVal);
        }
        
        [Test]
        public void Success_OnSuccessCallback_SubmittedToExecute()
        {
            // Arrange
            var promise = CreatePromise();
            var someObject = new object();
            object successResult = null;
            var callback = new Action<object>(x =>
            {
                successResult = x;
            });
            
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
            var promise = CreatePromise();
            var someObject = new object();
            object successResult = null;
            var callback = new Action<object>(x =>
            {
                successResult = x;
            });
            
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
            var promise = CreatePromise();
            var isExecuted = false;
            var callback = new Action(() =>
            {
                isExecuted = true;
            });
            
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
            var promise = CreatePromise();
            var isExecuted = false;
            var callback = new Action(() =>
            {
                isExecuted = true;
            });
            
            // Act
            promise.Success(new object());
            promise.Finally(callback);
            
            // Assert
            Assert.IsTrue(isExecuted);
        }

        public void SuccessTwice_ExceptionThrown()
        {
            // Arrange 
            var promise = CreatePromise();
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