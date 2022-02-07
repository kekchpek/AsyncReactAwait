using System;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using UnityAuxiliaryTools.Promises;
using UnityAuxiliaryTools.Promises.Factory;
using UnityAuxiliaryTools.UnityExecutor;
using SocialDemo.Code.Tests.EditMode.TestUtils;

namespace UnityAuxiliartyTools.Tests.Promises
{
    public class PromiseFactoryTests
    {

        private PromiseFactory CreateFactory(out IUnityExecutor unityExecutor)
        {
            unityExecutor = Substitute.For<IUnityExecutor>();
            return new PromiseFactory(unityExecutor);
        }
        
        [Test]
        public void CreateFailedPromise_OnFail_Executed()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));
            Exception receivedException = null;
            var failCallback = new Action<Exception>(e => receivedException = e);
            var testException = new Exception("TestException");
            
            // Act
            var promise = factory.CreateFailedPromise(testException);
            promise.OnFail(failCallback);
            
            // Assert
            Assert.AreEqual(testException, receivedException);
        }
        
        [Test]
        public void GenericCreateFailedPromise_OnFail_Executed()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));
            Exception receivedException = null;
            var failCallback = new Action<Exception>(e => receivedException = e);
            var testException = new Exception("TestException");
            
            // Act
            var promise = factory.CreateFailedPromise<object>(testException);
            promise.OnFail(failCallback);
            
            // Assert
            Assert.AreEqual(testException, receivedException);
        }

        [Test]
        public void CreateFailedPromise_CreatesControllablePromise()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            
            // Act
            var promise = factory.CreateFailedPromise(new Exception("TestException"));
            
            // Assert
            Assert.IsTrue(promise is ControllablePromise);
        }

        [Test]
        public void GenericCreateFailedPromise_CreatesControllablePromise()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            
            // Act
            var promise = factory.CreateFailedPromise<object>(new Exception("TestException"));
            
            // Assert
            Assert.IsTrue(promise is ControllablePromise<object>);
        }
        
        [Test]
        public void CreateSucceedPromise_OnSuccess_Executed()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));
            var callbackExecuted = false;
            var successCallback = new Action(() => callbackExecuted = true);
            
            // Act
            var promise = factory.CreateSucceedPromise();
            promise.OnSuccess(successCallback);
            
            // Assert
            Assert.IsTrue(callbackExecuted);
        }
        
        [Test]
        public void GenericCreateSucceedPromise_OnSuccess_Executed()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));
            object receivedObject = null;
            var successCallback = new Action<object>(x => receivedObject = x);
            var result = new object();
            
            // Act
            var promise = factory.CreateSucceedPromise(result);
            promise.OnSuccess(successCallback);
            
            // Assert
            Assert.AreEqual(result, receivedObject);
        }

        [Test]
        public void CreateSucceed_CreatesControllablePromise()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            
            // Act
            var promise = factory.CreateSucceedPromise();
            
            // Assert
            Assert.IsTrue(promise is ControllablePromise);
        }

        [Test]
        public void GenericCreateSucceed_CreatesControllablePromise()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            
            // Act
            var promise = factory.CreateSucceedPromise(new object());
            
            // Assert
            Assert.IsTrue(promise is ControllablePromise<object>);
        }

        [Test]
        public void Create_CreatesControllablePromise()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            
            // Act
            var promise = factory.CreatePromise();
            
            // Assert
            Assert.IsTrue(promise is ControllablePromise);
        }

        [Test]
        public void GenericCreate_CreatesControllablePromise()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            
            // Act
            var promise = factory.CreatePromise<object>();
            
            // Assert
            Assert.IsTrue(promise is ControllablePromise<object>);
        }

        [Test]
        public void CreateFromTask_CreatesControllablePromise()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            
            // Act
            var promise = factory.CreateFromTask(new Task(() => { }));
            
            // Assert
            Assert.IsTrue(promise is ControllablePromise);
        }

        [Test]
        public void GenericCreateFromTask_CreatesControllablePromise()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            
            // Act
            var promise = factory.CreateFromTask(new Task<object>(() => new object()));
            
            // Assert
            Assert.IsTrue(promise is ControllablePromise<object>);
        }
        
        [Test]
        public void CreateFromTask_UncompletedTask_FinallyNotExecuted()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            var semaphoreSlim = new SemaphoreSlim(0, 1);
            void TaskAction()
            {
                semaphoreSlim.Wait();
            }

            var finallyExecuted = false;
            void FinallyCallback()
            {
                finallyExecuted = true;
            }
            
            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));

            // Act
            var promise = factory.CreateFromTask(Task.Run(TaskAction));
            promise.Finally(FinallyCallback);
            WaitPromiseFinished(promise);

            // Assert
            Assert.IsFalse(finallyExecuted);
            
            // release the semaphore to complete a task
            semaphoreSlim.Release();
        }
        
        [Test]
        public void GenericCreateFromTask_UncompletedTask_FinallyNotExecuted()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            var semaphoreSlim = new SemaphoreSlim(0, 1);
            object TaskAction()
            {
                semaphoreSlim.Wait();
                return new object();
            }

            var finallyExecuted = false;
            void FinallyCallback()
            {
                finallyExecuted = true;
            }
            
            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));

            // Act
            var promise = factory.CreateFromTask(Task.Run(TaskAction));
            promise.Finally(FinallyCallback);
            WaitPromiseFinished(promise);

            // Assert
            Assert.IsFalse(finallyExecuted);
            
            // release the semaphore to complete a task
            semaphoreSlim.Release();
        }
        
        [Test]
        public void CreateFromTask_CompletedTask_OnSuccessExecuted()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            void TaskAction()
            {
                // do nothing
            }

            var successExecuted = false;
            void SuccessCallback()
            {
                successExecuted = true;
            }
            
            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));

            // Act
            var task = Task.Run(TaskAction);
            var promise = factory.CreateFromTask(task);
            promise.OnSuccess(SuccessCallback);
            WaitPromiseFinished(promise);

            // Assert
            Assert.IsTrue(successExecuted);
        }
        
        [Test]
        public void GenericCreateFromTask_CompletedTask_OnSuccessExecuted()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            var testObj = new object();
            object TaskAction()
            {
                return testObj;
            }

            object successResult = null;
            void SuccessCallback(object obj)
            {
                successResult = obj;
            }
            
            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));

            // Act
            var task = Task.Run(TaskAction);
            var promise = factory.CreateFromTask(task);
            promise.OnSuccess(SuccessCallback);
            WaitPromiseFinished(promise);

            // Assert
            Assert.AreEqual(testObj, successResult);
        }
        
        [Test]
        public void CreateFromTask_FailedTask_OnFailExecuted()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            var testException = new Exception("Test exception");

            void TaskAction()
            {
                throw testException;
            }

            Exception failException = null;

            void FailCallback(Exception exception)
            {
                failException = exception;
            }

            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));

            // Act
            var task = Task.Factory.StartNew(TaskAction);
            var promise = factory.CreateFromTask(task);
            promise.OnFail(FailCallback);
            Task.WaitAny(task);
            WaitPromiseFinished(promise);

            // Assert
            Assert.AreEqual(testException, failException);
        }
        
        [Test]
        public void GenericCreateFromTask_FailedTask_OnFailExecuted()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            var testException = new Exception("Test exception");

            object TaskAction()
            {
                throw testException;
            }

            Exception failException = null;

            void FailCallback(Exception exception)
            {
                failException = exception;
            }

            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));

            // Act
            var task = Task.Run(TaskAction);
            var promise = factory.CreateFromTask(task);
            promise.OnFail(FailCallback);
            Task.WaitAny(task);
            WaitPromiseFinished(promise);

            // Assert
            Assert.AreEqual(testException, failException);
        }

        [Test]
        public void CreateFromPromise_ControllablePromiseCreated()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);
            
            // Act
            var promise = factory.CreateFromPromise(Substitute.For<IPromise<object>>());
            
            // Assert
            Assert.IsTrue(promise is ControllablePromise);
        }

        [Test]
        public void CreateFromPromise_Success_Proxied()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);

            var sourcePromise = TestPromisesStaticFactory.CreatePromise<object>(out var successCallbacksRef,
                out var errorCallbacksRef,
                out var finallyCallbacksRef);
            var successCalled = false;
            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));
            
            // Act
            var promise = factory.CreateFromPromise(sourcePromise);
            promise.OnSuccess(() => successCalled = true);
            if (successCallbacksRef.TryGetTarget(out var successCallbacks))
            {
                foreach (var callback in successCallbacks)
                {
                    callback?.Invoke(new object());
                }
            }
            
            // Assert
            Assert.IsTrue(successCalled);
        }

        [Test]
        public void CreateFromPromise_Fail_Proxied()
        {
            // Arrange
            var factory = CreateFactory(out var unityExecutor);

            var sourcePromise = TestPromisesStaticFactory.CreatePromise<object>(out var successCallbacksRef,
                out var errorCallbacksRef,
                out var finallyCallbacksRef);
            Exception receivedException = null;
            var testException = new Exception("Test exception");
            unityExecutor.ExecuteOnFixedUpdate(Arg.Do<Action>(x => x?.Invoke()));
            
            // Act
            var promise = factory.CreateFromPromise(sourcePromise);
            promise.OnFail(x => receivedException = x);
            if (errorCallbacksRef.TryGetTarget(out var errorCallbacks))
            {
                foreach (var callback in errorCallbacks)
                {
                    callback?.Invoke(testException);
                }
            }
            
            // Assert
            Assert.AreEqual(testException, receivedException);
        }

        private static void WaitPromiseFinished(IBasePromise promise)
        {
            SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
            promise.Finally(() =>
            {
                semaphore.Release();
            });
            semaphore.Wait(100);
        }
    }
}