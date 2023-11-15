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
    
}