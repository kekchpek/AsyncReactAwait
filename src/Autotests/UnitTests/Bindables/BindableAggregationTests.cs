using System;
using System.Linq;
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

    [Test]
    public void Aggregate1000Bindables_AggregatedCorrectly()
    {
        // Arrange
        var floatBindables = new Mutable<float>[1000];
        for (int i = 0; i < floatBindables.Length; i++)
        {
            floatBindables[i] = new Mutable<float>();
        }
        var aggregatedBindable = 
            Bindable.Aggregate(floatBindables, values => values.Sum());
        
        // Act
        for (int i = 0; i < floatBindables.Length; i++)
        {
            floatBindables[i].Value = i;
        }
        
        // Assert
        Assert.AreEqual(
            floatBindables.Select(x => x.Value).Sum(),
            aggregatedBindable.Value);
    }

    [Test]
    public void Aggregate1000Bindables_AggregationCalled()
    {
        // Arrange
        var floatBindables = new Mutable<float>[1000];
        for (int i = 0; i < floatBindables.Length; i++)
        {
            floatBindables[i] = new Mutable<float>();
        }
        var aggregationCallsCount = 0;
        var aggregatedBindable = 
            Bindable.Aggregate(floatBindables, _ => 0f);
        aggregatedBindable.Bind(_ => aggregationCallsCount++);

        // Act
        for (int i = 0; i < floatBindables.Length; i++)
        {
            floatBindables[i].ForceSet(i);
        }
        
        // Assert
        Assert.AreEqual(
            floatBindables.Length + 1, // additional one on creation
            aggregationCallsCount);
    }
    
    
    [Test]
    public void Aggregate1000Decorators_AggregationCalled()
    {
        // Arrange
        var sourceFloatBindables = new Mutable<float>[1000];
        for (int i = 0; i < sourceFloatBindables.Length; i++)
        {
            sourceFloatBindables[i] = new Mutable<float>();
        }

        var stringBindables = new IBindable<string>[1000];
        for (int i = 0; i < sourceFloatBindables.Length; i++)
        {
            stringBindables[i] = sourceFloatBindables[i].ConvertTo(x => x.ToString());
        }
        var aggregationCallsCount = 0;
        var aggregatedBindable = 
            Bindable.Aggregate(stringBindables, _ => 0f);
        aggregatedBindable.Bind(_ => aggregationCallsCount++);

        // Act
        for (int i = 0; i < sourceFloatBindables.Length; i++)
        {
            sourceFloatBindables[i].ForceSet(i);
        }
        
        // Assert
        Assert.AreEqual(
            stringBindables.Length + 1, // additional one on creation
            aggregationCallsCount);
    }

    [Test]
    public void Aggregate1000Bindables_ValToVal_AggregatedCorrectly()
    {
        // Arrange
        var sourceFloatBindables = new Mutable<float>[1000];
        for (int i = 0; i < sourceFloatBindables.Length; i++)
        {
            sourceFloatBindables[i] = new Mutable<float>();
        }
        
        var floatBindables = new IBindable<float>[1000];
        for (int i = 0; i < sourceFloatBindables.Length; i++)
        {
            floatBindables[i] = sourceFloatBindables[i].ConvertTo(x => x + 1f);
        }
        var aggregatedBindable = 
            Bindable.Aggregate(floatBindables, values => values.Sum());
        
        // Act
        for (int i = 0; i < sourceFloatBindables.Length; i++)
        {
            sourceFloatBindables[i].ForceSet(i);
        }
        
        // Assert
        Assert.AreEqual(
            floatBindables.Select(x => x.Value).Sum(),
            aggregatedBindable.Value);
    }

    [Test]
    public void Aggregate1000Bindables_ValToRef_AggregatedCorrectly()
    {
        // Arrange
        var sourceFloatBindables = new Mutable<float>[1000];
        for (int i = 0; i < sourceFloatBindables.Length; i++)
        {
            sourceFloatBindables[i] = new Mutable<float>();
        }
        
        var stringBindables = new IBindable<string>[1000];
        for (int i = 0; i < sourceFloatBindables.Length; i++)
        {
            stringBindables[i] = sourceFloatBindables[i].ConvertTo(x => x.ToString());
        }
        var aggregatedBindable = 
            Bindable.Aggregate(stringBindables, values => string.Join("", values));
        
        // Act
        for (int i = 0; i < sourceFloatBindables.Length; i++)
        {
            sourceFloatBindables[i].ForceSet(i);
        }
        
        // Assert
        Assert.AreEqual(
            string.Join("", stringBindables.Select(x => x.Value)),
            aggregatedBindable.Value);
    }

    [Test]
    public void Aggregate1000Bindables_RefToVal_AggregatedCorrectly()
    {
        // Arrange
        var sourceStringBindables = new Mutable<string>[1000];
        for (int i = 0; i < sourceStringBindables.Length; i++)
        {
            sourceStringBindables[i] = new Mutable<string>("0");
        }
        
        var floatBindables = new IBindable<float>[1000];
        for (int i = 0; i < sourceStringBindables.Length; i++)
        {
            floatBindables[i] = sourceStringBindables[i].ConvertTo(float.Parse);
        }
        var aggregatedBindable = 
            Bindable.Aggregate(floatBindables, values => values.Sum());
        
        // Act
        for (int i = 0; i < sourceStringBindables.Length; i++)
        {
            sourceStringBindables[i].ForceSet(i.ToString());
        }
        
        // Assert
        Assert.AreEqual(
            floatBindables.Select(x => x.Value).Sum(),
            aggregatedBindable.Value);
    }
}