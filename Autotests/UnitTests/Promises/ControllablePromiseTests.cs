using System;
using NSubstitute;
using NUnit.Framework;
using AsyncReactAwait.Promises;

namespace AsyncReactAwait.Tests.Promises
{
    public class ControllablePromiseTests
    {

        private ControllablePromise CreatePromise()
        {
            return new ControllablePromise();
        }
        
        [Test]
        public void OnSuccess_ReturnsItself()
        {
            // Arrange
            var promise = CreatePromise();
            
            // Act
            var retVal = promise.OnSuccess(() => { });
            
            // Assert
            Assert.AreEqual(promise, retVal);
        }
        
        [Test]
        public void Success_OnSuccessCallback_SubmittedToExecute()
        {
            // Arrange
            var promise = CreatePromise();
            var isExecuted = false;
            var callback = new Action(() =>
            {
                isExecuted = true;
            });
            
            // Act
            promise.OnSuccess(callback);
            promise.Success();
            
            // Assert
            Assert.IsTrue(isExecuted);
        }
        
        [Test]
        public void Success_OnSuccessBefore_SubmittedToExecute()
        {
            // Arrange
            var promise = CreatePromise();
            var isExecuted = 0;
            var callback = new Action(() =>
            {
                isExecuted++;
            });
            
            // Act
            promise.Success();
            promise.OnSuccess(callback);
            
            // Assert
            Assert.AreEqual(1, isExecuted);
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
            promise.Success();
            
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
            promise.Success();
            promise.Finally(callback);
            
            // Assert
            Assert.IsTrue(isExecuted);
        }

        public void SuccessTwice_ExceptionThrown()
        {
            // Arrange 
            var promise = CreatePromise();
            promise.Success();

            // Act
            // no act
            
            // Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                promise.Success();
            });
        }
    }
}