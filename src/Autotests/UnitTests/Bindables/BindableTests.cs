using System;
using AsyncReactAwait.Bindable;
using AsyncReactAwait.Bindable.BindableExtensions;
using NUnit.Framework;

namespace Autotests.UnitTests.Bindables;

public class BindableTests
{
    [Test]
    public void BindInstantlyCalled()
    {
        // Arrange
        var bindable = new Mutable<int>(100);
        int? val = null;
        
        // Act
        bindable.Bind(x => val = x);
        
        // Assert
        Assert.AreEqual(val, 100);
    }
    
    [Test]
    public void BindBlindInstantlyCalled()
    {
        // Arrange
        var bindable = new Mutable<int>(100);
        int? val = null;
        
        // Act
        bindable.Bind(() => val = bindable.Value);
        
        // Assert
        Assert.AreEqual(val, 100);
    }
    
    [Test]
    public void BindInstantlyNotCalled()
    {
        // Arrange
        var bindable = new Mutable<int>(100);
        int? val = null;
        
        // Act
        bindable.Bind(x => val = x, false);
        
        // Assert
        Assert.AreEqual(null, val);
    }
    
    [Test]
    public void BindBlindInstantlyNotCalled()
    {
        // Arrange
        var bindable = new Mutable<int>(100);
        int? val = null;
        
        // Act
        bindable.Bind(() => val = bindable.Value, false);
        
        // Assert
        Assert.AreEqual(val, null);
    }

    [Test]
    public void BindFull()
    {
        // Arrange
        var bindable = new Mutable<int>(100);
        int? prevVal = null;
        int? newVal = null;
        
        // Act
        bindable.Bind((p, n) =>
        {
            prevVal = p;
            newVal = n;
        });
        bindable.Set(200);
        
        // Assert
        Assert.AreEqual(prevVal, 100);
        Assert.AreEqual(newVal, 200);
    }

    [Test]
    public void UnbindFull()
    {
        // Arrange
        var bindable = new Mutable<int>(100);
        int? prevVal = null;
        int? newVal = null;
        
        // Act
        void Handler(int p, int n)
        {
            prevVal = p;
            newVal = n;
        }

        bindable.Bind((Action<int, int>)Handler);
        bindable.Unbind((Action<int, int>)Handler);
        bindable.Set(200);
        
        // Assert
        Assert.AreEqual(prevVal, null);
        Assert.AreEqual(newVal, null);
    }

    [Test]
    public void Proxy_ValueChanged()
    {
        // Arrange
        var proxied = new Mutable<int>(123);
        var proxy = new Mutable<int>();
        
        // Act
        proxy.Proxy(proxied);
        
        // Assert
        Assert.AreEqual(proxied.Value, proxy.Value);
    }

    [Test]
    public void Proxy_ProxiedChanged_ValueChanged()
    {
        // Arrange
        var proxied = new Mutable<int>(123);
        var proxy = new Mutable<int>();
        
        // Act
        proxy.Proxy(proxied);
        proxied.Value = 333;
        
        // Assert
        Assert.AreEqual(proxied.Value, proxy.Value);
    }

    [Test]
    public void Proxy_StopProxying_ValueNotChanged()
    {
        // Arrange
        var proxied = new Mutable<int>(123);
        var proxy = new Mutable<int>();
        
        // Act
        proxy.Proxy(proxied);
        proxied.Value = 333;
        proxy.StopProxying();
        proxied.Value = 444;
        
        // Assert
        Assert.AreEqual(333, proxy.Value);
    }

    [Test]
    public void Proxy_SetValue_ValueNotChanged()
    {
        // Arrange
        var proxied = new Mutable<int>(123);
        var proxy = new Mutable<int>();
        
        // Act
        proxy.Proxy(proxied);
        proxied.Value = 333;
        proxy.Value = 12345;
        proxied.Value = 444;
        
        // Assert
        Assert.AreEqual(12345, proxy.Value);
    }

    [Test]
    public void Proxy_ForceSetValue_ValueNotChanged()
    {
        // Arrange
        var proxied = new Mutable<int>(123);
        var proxy = new Mutable<int>();
        
        // Act
        proxy.Proxy(proxied);
        proxied.Value = 333;
        proxy.ForceSet(12345);
        proxied.Value = 444;
        
        // Assert
        Assert.AreEqual(12345, proxy.Value);
    }

    [Test]
    public void OnAnySubscription_NoActions_NotFired()
    {
        // Arrange
        var mutable = new Mutable<int>();
        var fired = false;
        mutable.OnAnySubscription += () => fired = true;
        
        // Act 
        // No action
        
        // Assert
        Assert.IsFalse(fired);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void OnAnySubscription_HandlerBound_NotFired(bool callHandlerImmediately)
    {
        // Arrange
        var mutable = new Mutable<int>();
        var fired = false;
        mutable.OnAnySubscription += () => fired = true;
        
        // Act
        mutable.Bind(() => { }, callHandlerImmediately);
        
        // Assert
        Assert.IsTrue(fired);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void OnAnySubscription_ValueHandlerBound_NotFired(bool callHandlerImmediately)
    {
        // Arrange
        var mutable = new Mutable<int>();
        var fired = false;
        mutable.OnAnySubscription += () => fired = true;
        
        // Act
        mutable.Bind((int _) => { }, callHandlerImmediately);
        
        // Assert
        Assert.IsTrue(fired);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void OnAnySubscription_ValueRawHandlerBound_NotFired(bool callHandlerImmediately)
    {
        // Arrange
        var mutable = new Mutable<int>();
        var fired = false;
        mutable.OnAnySubscription += () => fired = true;
        
        // Act
        mutable.Bind((object _) => { }, callHandlerImmediately);
        
        // Assert
        Assert.IsTrue(fired);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void OnAnySubscription_ValueRawChangeHandlerBound_NotFired(bool callHandlerImmediately)
    {
        // Arrange
        var mutable = new Mutable<int>();
        var fired = false;
        mutable.OnAnySubscription += () => fired = true;
        
        // Act
        mutable.Bind((object _, object _) => { }, callHandlerImmediately);
        
        // Assert
        Assert.IsTrue(fired);
    }
    
}