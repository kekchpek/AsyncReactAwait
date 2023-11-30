using System;
using AsyncReactAwait.Bindable;
using AsyncReactAwait.Bindable.BindableExtensions;
using NSubstitute;
using NUnit.Framework;

namespace Autotests.UnitTests.Bindables;

public class BindableAggregationTests
{
    [Test]
    public void AggregationBindInstantCall()
    {
        // Arrange
        var bindable1 = new Mutable<int>(100);
        var bindable2 = new Mutable<int>(1000);
        var aggregated = Bindable.Aggregate(
            bindable1,
            bindable2,
            (val1, val2) => val1 + val2);
        int? val = null;
        
        // Act
        aggregated.Bind(x => val = x);
        
        // Assert
        Assert.AreEqual(1100, val);
    }

    [Test]
    public void AggregationBindNoCall()
    {
        // Arrange
        var bindable1 = new Mutable<int>(100);
        var bindable2 = new Mutable<int>(1000);
        var aggregated = Bindable.Aggregate(
            bindable1,
            bindable2,
            (val1, val2) => val1 + val2);
        int? val = null;
        
        // Act
        aggregated.Bind(x => val = x, false);
        
        // Assert
        Assert.AreEqual(null, val);
    }

    [Test]
    public void AggregationBind()
    {
        // Arrange
        var bindable1 = new Mutable<int>(100);
        var bindable2 = new Mutable<int>(1000);
        var aggregated = Bindable.Aggregate(
            bindable1,
            bindable2,
            (val1, val2) => val1 + val2);
        int? val = null;
        
        // Act
        aggregated.Bind(x => val = x, false);
        bindable1.Set(2000);
        
        // Assert
        Assert.AreEqual(3000, val);
    }

    [Test]
    public void AggregationUnBind()
    {
        // Arrange
        var bindable1 = new Mutable<int>(100);
        var bindable2 = new Mutable<int>(1000);
        var aggregated = Bindable.Aggregate(
            bindable1,
            bindable2,
            (val1, val2) => val1 + val2);
        int? val = null;
        
        // Act
        void Handler(int x) => val = x;
        aggregated.Bind(Handler, false);
        aggregated.Unbind(Handler);
        bindable1.Set(2000);
        
        // Assert
        Assert.AreEqual(null, val);
    }

    [Test]
    public void AggregationBindBlindInstantCall()
    {
        // Arrange
        var bindable1 = new Mutable<int>(100);
        var bindable2 = new Mutable<int>(1000);
        var aggregated = Bindable.Aggregate(
            bindable1,
            bindable2,
            (val1, val2) => val1 + val2);
        int? val = null;
        
        // Act
        aggregated.Bind(() => val = aggregated.Value);
        
        // Assert
        Assert.AreEqual(1100, val);
    }

    [Test]
    public void AggregationBindBlindNoCall()
    {
        // Arrange
        var bindable1 = new Mutable<int>(100);
        var bindable2 = new Mutable<int>(1000);
        var aggregated = Bindable.Aggregate(
            bindable1,
            bindable2,
            (val1, val2) => val1 + val2);
        int? val = null;
        
        // Act
        aggregated.Bind(() => val = aggregated.Value, false);
        
        // Assert
        Assert.AreEqual(null, val);
    }

    [Test]
    public void AggregationBindBlind()
    {
        // Arrange
        var bindable1 = new Mutable<int>(100);
        var bindable2 = new Mutable<int>(1000);
        var aggregated = Bindable.Aggregate(
            bindable1,
            bindable2,
            (val1, val2) => val1 + val2);
        int? val = null;
        
        // Act
        aggregated.Bind(() => val = aggregated.Value, false);
        bindable1.Set(10000);
        
        // Assert
        Assert.AreEqual(11000, val);
    }

    [Test]
    public void AggregationUnbindBlind()
    {
        // Arrange
        var bindable1 = new Mutable<int>(100);
        var bindable2 = new Mutable<int>(1000);
        var aggregated = Bindable.Aggregate(
            bindable1,
            bindable2,
            (val1, val2) => val1 + val2);
        int? val = null;
        
        // Act
        void Handler() => val = aggregated.Value;
        aggregated.Bind(Handler, false);
        aggregated.Unbind(Handler);
        bindable1.Set(10000);
        
        // Assert
        Assert.AreEqual(null, val);
    }

    [Test]
    public void AggregationBindFullNoCall()
    {
        // Arrange
        var bindable1 = new Mutable<int>(100);
        var bindable2 = new Mutable<int>(1000);
        var aggregated = Bindable.Aggregate(
            bindable1,
            bindable2,
            (val1, val2) => val1 + val2);
        int? prevVal = null;
        int? newVal = null;
        
        // Act
        aggregated.Bind((p, n) =>
        {
            prevVal = p;
            newVal = n;
        });
        
        // Assert
        Assert.AreEqual(null, prevVal);
        Assert.AreEqual(null, newVal);
    }

    [Test]
    public void AggregationBindFull()
    {
        // Arrange
        var bindable1 = new Mutable<int>(100);
        var bindable2 = new Mutable<int>(1000);
        var aggregated = Bindable.Aggregate(
            bindable1,
            bindable2,
            (val1, val2) => val1 + val2);
        int? prevVal = null;
        int? newVal = null;
        
        // Act
        aggregated.Bind((p, n) =>
        {
            prevVal = p;
            newVal = n;
        });
        bindable1.Set(10000);
        
        // Assert
        Assert.AreEqual(1100, prevVal);
        Assert.AreEqual(11000, newVal);
    }

    [Test]
    public void AggregationUnbindFull()
    {
        // Arrange
        var bindable1 = new Mutable<int>(100);
        var bindable2 = new Mutable<int>(1000);
        var aggregated = Bindable.Aggregate(
            bindable1,
            bindable2,
            (val1, val2) => val1 + val2);
        int? prevVal = null;
        int? newVal = null;
        
        // Act
        void Handler(int p, int n) 
        {
            prevVal = p;
            newVal = n;
        }
        aggregated.Bind(Handler);
        aggregated.Unbind(Handler);
        bindable1.Set(10000);
        
        // Assert
        Assert.AreEqual(null, prevVal);
        Assert.AreEqual(null, newVal);
    }

    [Test]
    public void BindingProxyingNoBind()
    {
        // Arrange
        IBindableRaw bindable1 = Substitute.For<IBindableRaw>();
        IBindableRaw bindable2 = Substitute.For<IBindableRaw>();
        
        // Act
        // ReSharper disable once UnusedVariable
        var aggregatedBindable = new BindableAggregator<int>(
            new [] { bindable1, bindable2 }, 
            values => (int)values[0] + (int)values[1]);
        
        // Assert
        bindable1.DidNotReceiveWithAnyArgs().Bind(null!, false);
        bindable1.DidNotReceiveWithAnyArgs().Bind(null!, false);
        bindable1.DidNotReceiveWithAnyArgs().Bind(Arg.Any<Action<object, object>>());
        bindable2.DidNotReceiveWithAnyArgs().Bind(null!, false);
        bindable2.DidNotReceiveWithAnyArgs().Bind(null!, false);
        bindable2.DidNotReceiveWithAnyArgs().Bind(Arg.Any<Action<object, object>>());
    }

    [Test]
    public void BindingProxyingBind()
    {
        // Arrange
        IBindableRaw bindable1 = Substitute.For<IBindableRaw>();
        bindable1.Value.Returns(100);
        IBindableRaw bindable2 = Substitute.For<IBindableRaw>();
        bindable2.Value.Returns(100);
        
        // Act
        var aggregatedBindable = new BindableAggregator<int>(
            new [] { bindable1, bindable2 }, 
            values => (int)values[0] + (int)values[1]);
        void Handler(int x) { }
        aggregatedBindable.Bind(Handler, false);
        
        // Assert
        bindable1.DidNotReceiveWithAnyArgs().Bind(Arg.Any<Action>(), false);
        bindable1.Received(1).Bind(Arg.Any<Action<object>>(), false);
        bindable1.Received(1).Bind(Arg.Any<Action<object, object>>());
        bindable2.DidNotReceiveWithAnyArgs().Bind(Arg.Any<Action>(), false);
        bindable2.Received(1).Bind(Arg.Any<Action<object>>(), false);
        bindable2.Received(1).Bind(Arg.Any<Action<object, object>>());
    }

    [Test]
    public void BindingProxyingUnbind()
    {
        // Arrange
        IBindableRaw bindable1 = Substitute.For<IBindableRaw>();
        bindable1.Value.Returns(100);
        IBindableRaw bindable2 = Substitute.For<IBindableRaw>();
        bindable2.Value.Returns(100);
        
        // Act
        var aggregatedBindable = new BindableAggregator<int>(
            new [] { bindable1, bindable2 }, 
            values => (int)values[0] + (int)values[1]);
        void Handler(int x) { }
        aggregatedBindable.Bind(Handler, false);
        aggregatedBindable.Unbind(Handler);
        
        // Assert
        bindable1.Received(1).Unbind(Arg.Any<Action<object>>());
        bindable1.Received(1).Unbind(Arg.Any<Action<object, object>>());
        bindable2.Received(1).Unbind(Arg.Any<Action<object>>());
        bindable2.Received(1).Unbind(Arg.Any<Action<object, object>>());
    }
}