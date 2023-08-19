using AsyncReactAwait.Bindable;
using AsyncReactAwait.Bindable.BindableExtensions;
using NUnit.Framework;

namespace Autotests.UnitTests.Bindables;

public class BindableDecoratorTests
{
    [Test]
    public void TestConversionBindInstantCall()
    {
        // Arrange
        var bindable = new Mutable<int>(100);
        var convertedBindable = bindable.ConvertTo(x => x * 1000);
        int? val = null;
        
        // Act
        convertedBindable.Bind(x => val = x);
        
        // Assert
        Assert.AreEqual(val, 100000);
    }

    [Test]
    public void TestConversionBindNoCall()
    {
        // Arrange
        var bindable = new Mutable<int>(100);
        var convertedBindable = bindable.ConvertTo(x => x * 1000);
        int? val = null;
        
        // Act
        convertedBindable.Bind(x => val = x, false);
        
        // Assert
        Assert.AreEqual(val, null);
    }

    [Test]
    public void TestConversionBind()
    {
        // Arrange
        var bindable = new Mutable<int>(100);
        var convertedBindable = bindable.ConvertTo(x => x * 1000);
        int? val = null;
        
        // Act
        convertedBindable.Bind(x => val = x, false);
        bindable.Value = 10;
        
        // Assert
        Assert.AreEqual(val, 10000);
    }
    
    [Test]
    public void TestConversionBindBlindInstantCall()
    {
        // Arrange
        var bindable = new Mutable<int>(100);
        var convertedBindable = bindable.ConvertTo(x => x * 1000);
        int? val = null;
        
        // Act
        convertedBindable.Bind(() => val = convertedBindable.Value);
        
        // Assert
        Assert.AreEqual(100000, val);
    }
    
    [Test]
    public void TestConversionBindBlindNoCall()
    {
        // Arrange
        var bindable = new Mutable<int>(100);
        var convertedBindable = bindable.ConvertTo(x => x * 1000);
        int? val = null;
        
        // Act
        convertedBindable.Bind(() => val = bindable.Value, false);
        
        // Assert
        Assert.AreEqual(val, null);
    }
    
    [Test]
    public void TestConversionBindBlind()
    {
        // Arrange
        var bindable = new Mutable<int>(100);
        var convertedBindable = bindable.ConvertTo(x => x * 1000);
        int? val = null;
        
        // Act
        convertedBindable.Bind(() => val = convertedBindable.Value, false);
        bindable.Set(10);
        
        // Assert
        Assert.AreEqual(10000, val);
    }

    [Test]
    public void TestConversionBindFullNoCall()
    {
        // Arrange
        var bindable = new Mutable<int>(100);
        var convertedBindable = bindable.ConvertTo(x => x * 1000);
        int? newVal = null;
        int? prevVal = null;
        
        // Act
        convertedBindable.Bind((p, n) =>
        {
            newVal = n;
            prevVal = p;
        });
        
        // Assert
        Assert.AreEqual(null, prevVal);
        Assert.AreEqual(null, newVal);
    }

    [Test]
    public void TestConversionBindFull()
    {
        // Arrange
        var bindable = new Mutable<int>(100);
        var convertedBindable = bindable.ConvertTo(x => x * 1000);
        int? newVal = null;
        int? prevVal = null;
        
        // Act
        convertedBindable.Bind((p, n) =>
        {
            newVal = n;
            prevVal = p;
        });
        bindable.Set(10);
        
        // Assert
        Assert.AreEqual(100000, prevVal);
        Assert.AreEqual(10000, newVal);
    }
}