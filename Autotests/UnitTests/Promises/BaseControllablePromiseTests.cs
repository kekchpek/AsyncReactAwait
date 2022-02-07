using System;
using NSubstitute;
using NUnit.Framework;
using UnityAuxiliaryTools.Promises;
using UnityAuxiliaryTools.UnityExecutor;

namespace UnityAuxiliartyTools.Tests.Promises
{
    public class BaseControllablePromiseTests
    {
        private class TestPromise : BaseControllablePromise
        {
            public TestPromise(IUnityExecutor unityExecutor) : base(unityExecutor)
            {
            }
        }

        private TestPromise CreatePromise(out IUnityExecutor unityExecutor)
        {
            unityExecutor = Substitute.For<IUnityExecutor>();
            return new TestPromise(unityExecutor);
        }

        [Test]
        public void Fails_OnFailCallback_SubmittedToExecute()
        {
            // Arrange 
            var promise = CreatePromise(out var unityExecutor);
            Exception proceedException = null;
            var testException = new Exception("Test exception");
            var failCallback = new Action<Exception>(e =>
            {
                proceedException = e;
            });
            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));

            // Act
            promise.OnFail(failCallback);
            promise.Fail(testException);
            
            // Assert
            Assert.AreEqual(testException, proceedException);
        }

        [Test]
        public void Fails_OnFailBefore_SubmittedToExecute()
        {
            // Arrange 
            var promise = CreatePromise(out var unityExecutor);
            Exception proceedException = null;
            var testException = new Exception("Test exception");
            var failCallback = new Action<Exception>(e =>
            {
                proceedException = e;
            });
            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));

            // Act
            promise.Fail(testException);
            promise.OnFail(failCallback);
            
            // Assert
            Assert.AreEqual(testException, proceedException);
        }

        [Test]
        public void Fails_FinallyCallback_SubmittedToExecute()
        {
            // Arrange 
            var promise = CreatePromise(out var unityExecutor);
            var wasCalled = false;
            var finallyCallback = new Action(() =>
            {
                wasCalled = true;
            });
            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));

            // Act
            promise.Finally(finallyCallback);
            promise.Fail(new Exception("Test exception"));
            
            // Assert
            Assert.IsTrue(wasCalled);
        }

        [Test]
        public void Fails_FinallyBefore_SubmittedToExecute()
        {
            // Arrange 
            var promise = CreatePromise(out var unityExecutor);
            var wasCalled = false;
            var finallyCallback = new Action(() =>
            {
                wasCalled = true;
            });
            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));

            // Act
            promise.Fail(new Exception("Test exception"));
            promise.Finally(finallyCallback);
            
            // Assert
            Assert.IsTrue(wasCalled);
        }

        [Test]
        public void Finally_ReturnsItself()
        {
            // Arrange 
            var promise = CreatePromise(out var unityExecutor);

            // Act
            var retVal = promise.Finally(() => { });
            
            // Assert
            Assert.AreEqual(promise, retVal);
        }

        [Test]
        public void OnFail_ReturnsItself()
        {
            // Arrange 
            var promise = CreatePromise(out var unityExecutor);

            // Act
            var retVal = promise.OnFail(e => { });
            
            // Assert
            Assert.AreEqual(promise, retVal);
        }
    
        [Test]
        public void FailTwice_ExceptionThrown()
        {
            // Arrange 
            var promise = CreatePromise(out var unityExecutor);
            promise.Fail(new Exception("Test exception"));

            // Act
            // no act
            
            // Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                promise.Fail(new Exception("Test exception"));
            });
        }
    }
}