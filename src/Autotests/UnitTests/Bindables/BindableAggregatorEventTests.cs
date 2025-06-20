using System;
using System.Collections.Generic;
using System.Linq;
using AsyncReactAwait.Bindable;
using AsyncReactAwait.Bindable.BindableExtensions;
using NUnit.Framework;

namespace Autotests.UnitTests.Bindables;

[TestFixture]
public class BindableAggregatorEventTests
{
    private static void CheckEvents(IBindable aggregator, Action firstBind, Action secondBind,
        Action firstUnbind, Action secondUnbind)
    {
        int anyCount = 0;
        int clearedCount = 0;
        aggregator.OnAnySubscription += () => anyCount++;
        aggregator.OnSubscriptionsCleared += () => clearedCount++;

        // first bind
        firstBind();
        Assert.AreEqual(1, anyCount, "First OnAnySubscription should fire");
        Assert.AreEqual(0, clearedCount);

        // second bind => second OnAnySubscription
        secondBind();
        Assert.AreEqual(2, anyCount, "Second OnAnySubscription should fire");
        Assert.AreEqual(0, clearedCount);

        // remove one listener => still listeners left, no cleared
        firstUnbind();
        Assert.AreEqual(2, anyCount);
        Assert.AreEqual(0, clearedCount);

        // remove last listener => cleared fires
        secondUnbind();
        Assert.AreEqual(2, anyCount);
        Assert.AreEqual(1, clearedCount, "OnSubscriptionsCleared should fire once after last listener removed");
    }

    [Test]
    public void Fixed2_SubscriptionEvents()
    {
        var m1 = new Mutable<int>();
        var m2 = new Mutable<int>();
        var agg = new BindableAggregator<int,int,int>(m1,m2,(a,b)=>a+b);

        Action<int> h1 = _=>{};
        Action<int> h2 = _=>{};

        CheckEvents(agg,
            () => agg.Bind(h1,false),
            () => agg.Bind(h2,false),
            () => agg.Unbind(h1),
            () => agg.Unbind(h2));
    }

    [Test]
    public void Fixed3_SubscriptionEvents()
    {
        var agg = new BindableAggregator<int,int,int,int>(
            new Mutable<int>(),new Mutable<int>(),new Mutable<int>(),(a,b,c)=>a+b+c);
        Action<int> h1=_=>{}; Action<int> h2=_=>{};
        CheckEvents(agg,
            ()=>agg.Bind(h1,false),
            ()=>agg.Bind(h2,false),
            ()=>agg.Unbind(h1),
            ()=>agg.Unbind(h2));
    }

    [Test]
    public void Fixed4_SubscriptionEvents()
    {
        var agg = new BindableAggregator<int,int,int,int,int>(
            new Mutable<int>(),new Mutable<int>(),new Mutable<int>(),new Mutable<int>(),(a,b,c,d)=>a+b+c+d);
        Action<int> h1=_=>{}; Action<int> h2=_=>{};
        CheckEvents(agg,
            ()=>agg.Bind(h1,false),
            ()=>agg.Bind(h2,false),
            ()=>agg.Unbind(h1),
            ()=>agg.Unbind(h2));
    }

    [Test]
    public void GenericAggregator_SubscriptionEvents()
    {
        var list = new[] { new Mutable<int>(), new Mutable<int>() } ;
        var agg = new BindableAggregator<int,int>(list, b=> b.Sum(x=>x.Value));
        Action<int> h1=_=>{}; Action<int> h2=_=>{};
        CheckEvents(agg,
            ()=>agg.Bind(h1,false),
            ()=>agg.Bind(h2,false),
            ()=>agg.Unbind(h1),
            ()=>agg.Unbind(h2));
    }

    [Test]
    public void RawAggregator_SubscriptionEvents()
    {
        IBindableRaw[] raws = { new Mutable<int>(), new Mutable<int>() };
        var agg = new BindableAggregator<int>(raws, r=>0);
        Action<int> h1=_=>{}; Action<int> h2=_=>{};
        CheckEvents(agg,
            ()=>agg.Bind(h1,false),
            ()=>agg.Bind(h2,false),
            ()=>agg.Unbind(h1),
            ()=>agg.Unbind(h2));
    }

    [Test]
    public void Decorator_SubscriptionEvents()
    {
        var m = new Mutable<int>();
        var dec = m.ConvertTo(x=>x.ToString());
        Action<string> h1=_=>{}; Action<string> h2=_=>{};
        CheckEvents(dec,
            ()=>dec.Bind(h1,false),
            ()=>dec.Bind(h2,false),
            ()=>dec.Unbind(h1),
            ()=>dec.Unbind(h2));
    }

    [Test]
    public void Fixed2_BlindBind_SubscriptionEvents()
    {
        var agg = new BindableAggregator<int,int,int>(new Mutable<int>(), new Mutable<int>(), (a, b) => a + b);

        Action h1 = () => { };
        Action h2 = () => { };

        CheckEvents(agg,
            () => agg.Bind(h1, false),
            () => agg.Bind(h2, false),
            () => agg.Unbind(h1),
            () => agg.Unbind(h2));
    }

    [Test]
    public void Fixed2_FullBind_SubscriptionEvents()
    {
        var agg = new BindableAggregator<int,int,int>(new Mutable<int>(), new Mutable<int>(), (a, b) => a + b);

        Action<int, int> h1 = (_, _) => { };
        Action<int, int> h2 = (_, _) => { };

        CheckEvents(agg,
            () => agg.Bind(h1),
            () => agg.Bind(h2),
            () => agg.Unbind(h1),
            () => agg.Unbind(h2));
    }

    [Test]
    public void Fixed2_RawBind_SubscriptionEvents()
    {
        var agg = new BindableAggregator<int,int,int>(new Mutable<int>(), new Mutable<int>(), (a, b) => a + b);
        var raw = (IBindableRaw)agg;

        Action<object?> h1 = _ => { };
        Action<object?> h2 = _ => { };

        CheckEvents(agg,
            () => raw.Bind(h1, false),
            () => raw.Bind(h2, false),
            () => raw.Unbind(h1),
            () => raw.Unbind(h2));
    }

    [Test]
    public void Fixed2_RawFullBind_SubscriptionEvents()
    {
        var agg = new BindableAggregator<int,int,int>(new Mutable<int>(), new Mutable<int>(), (a, b) => a + b);
        var raw = (IBindableRaw)agg;

        Action<object?, object?> h1 = (_, _) => { };
        Action<object?, object?> h2 = (_, _) => { };

        CheckEvents(agg,
            () => raw.Bind(h1),
            () => raw.Bind(h2),
            () => raw.Unbind(h1),
            () => raw.Unbind(h2));
    }

    // Fixed3 overload tests
    [Test]
    public void Fixed3_BlindBind_SubscriptionEvents()
    {
        var agg = new BindableAggregator<int, int, int, int>(new Mutable<int>(), new Mutable<int>(), new Mutable<int>(), (a, b, c) => a + b + c);

        Action h1 = () => { };
        Action h2 = () => { };

        CheckEvents(agg,
            () => agg.Bind(h1, false),
            () => agg.Bind(h2, false),
            () => agg.Unbind(h1),
            () => agg.Unbind(h2));
    }

    [Test]
    public void Fixed3_FullBind_SubscriptionEvents()
    {
        var agg = new BindableAggregator<int, int, int, int>(new Mutable<int>(), new Mutable<int>(), new Mutable<int>(), (a, b, c) => a + b + c);

        Action<int, int> h1 = (_, _) => { };
        Action<int, int> h2 = (_, _) => { };

        CheckEvents(agg,
            () => agg.Bind(h1),
            () => agg.Bind(h2),
            () => agg.Unbind(h1),
            () => agg.Unbind(h2));
    }

    [Test]
    public void Fixed3_RawBind_SubscriptionEvents()
    {
        var agg = new BindableAggregator<int, int, int, int>(new Mutable<int>(), new Mutable<int>(), new Mutable<int>(), (a, b, c) => a + b + c);
        var raw = (IBindableRaw)agg;

        Action<object?> h1 = _ => { };
        Action<object?> h2 = _ => { };

        CheckEvents(agg,
            () => raw.Bind(h1, false),
            () => raw.Bind(h2, false),
            () => raw.Unbind(h1),
            () => raw.Unbind(h2));
    }

    [Test]
    public void Fixed3_RawFullBind_SubscriptionEvents()
    {
        var agg = new BindableAggregator<int, int, int, int>(new Mutable<int>(), new Mutable<int>(), new Mutable<int>(), (a, b, c) => a + b + c);
        var raw = (IBindableRaw)agg;

        Action<object?, object?> h1 = (_, _) => { };
        Action<object?, object?> h2 = (_, _) => { };

        CheckEvents(agg,
            () => raw.Bind(h1),
            () => raw.Bind(h2),
            () => raw.Unbind(h1),
            () => raw.Unbind(h2));
    }

    // Fixed4 overload tests
    [Test]
    public void Fixed4_BlindBind_SubscriptionEvents()
    {
        var agg = new BindableAggregator<int, int, int, int, int>(new Mutable<int>(), new Mutable<int>(), new Mutable<int>(), new Mutable<int>(), (a, b, c, d) => a + b + c + d);

        Action h1 = () => { };
        Action h2 = () => { };

        CheckEvents(agg,
            () => agg.Bind(h1, false),
            () => agg.Bind(h2, false),
            () => agg.Unbind(h1),
            () => agg.Unbind(h2));
    }

    [Test]
    public void Fixed4_FullBind_SubscriptionEvents()
    {
        var agg = new BindableAggregator<int, int, int, int, int>(new Mutable<int>(), new Mutable<int>(), new Mutable<int>(), new Mutable<int>(), (a, b, c, d) => a + b + c + d);

        Action<int, int> h1 = (_, _) => { };
        Action<int, int> h2 = (_, _) => { };

        CheckEvents(agg,
            () => agg.Bind(h1),
            () => agg.Bind(h2),
            () => agg.Unbind(h1),
            () => agg.Unbind(h2));
    }

    [Test]
    public void Fixed4_RawBind_SubscriptionEvents()
    {
        var agg = new BindableAggregator<int, int, int, int, int>(new Mutable<int>(), new Mutable<int>(), new Mutable<int>(), new Mutable<int>(), (a, b, c, d) => a + b + c + d);
        var raw = (IBindableRaw)agg;

        Action<object?> h1 = _ => { };
        Action<object?> h2 = _ => { };

        CheckEvents(agg,
            () => raw.Bind(h1, false),
            () => raw.Bind(h2, false),
            () => raw.Unbind(h1),
            () => raw.Unbind(h2));
    }

    [Test]
    public void Fixed4_RawFullBind_SubscriptionEvents()
    {
        var agg = new BindableAggregator<int, int, int, int, int>(new Mutable<int>(), new Mutable<int>(), new Mutable<int>(), new Mutable<int>(), (a, b, c, d) => a + b + c + d);
        var raw = (IBindableRaw)agg;

        Action<object?, object?> h1 = (_, _) => { };
        Action<object?, object?> h2 = (_, _) => { };

        CheckEvents(agg,
            () => raw.Bind(h1),
            () => raw.Bind(h2),
            () => raw.Unbind(h1),
            () => raw.Unbind(h2));
    }

    // GenericAggregator overload tests
    [Test]
    public void GenericAggregator_BlindBind_SubscriptionEvents()
    {
        var list = new[] { new Mutable<int>(), new Mutable<int>() };
        var agg = new BindableAggregator<int, int>(list, b => b.Sum(x => x.Value));

        Action h1 = () => { };
        Action h2 = () => { };

        CheckEvents(agg,
            () => agg.Bind(h1, false),
            () => agg.Bind(h2, false),
            () => agg.Unbind(h1),
            () => agg.Unbind(h2));
    }

    [Test]
    public void GenericAggregator_FullBind_SubscriptionEvents()
    {
        var list = new[] { new Mutable<int>(), new Mutable<int>() };
        var agg = new BindableAggregator<int, int>(list, b => b.Sum(x => x.Value));

        Action<int, int> h1 = (_, _) => { };
        Action<int, int> h2 = (_, _) => { };

        CheckEvents(agg,
            () => agg.Bind(h1),
            () => agg.Bind(h2),
            () => agg.Unbind(h1),
            () => agg.Unbind(h2));
    }

    [Test]
    public void GenericAggregator_RawBind_SubscriptionEvents()
    {
        var list = new[] { new Mutable<int>(), new Mutable<int>() };
        var agg = new BindableAggregator<int, int>(list, b => b.Sum(x => x.Value));
        var raw = (IBindableRaw)agg;

        Action<object?> h1 = _ => { };
        Action<object?> h2 = _ => { };

        CheckEvents(agg,
            () => raw.Bind(h1, false),
            () => raw.Bind(h2, false),
            () => raw.Unbind(h1),
            () => raw.Unbind(h2));
    }

    [Test]
    public void GenericAggregator_RawFullBind_SubscriptionEvents()
    {
        var list = new[] { new Mutable<int>(), new Mutable<int>() };
        var agg = new BindableAggregator<int, int>(list, b => b.Sum(x => x.Value));
        var raw = (IBindableRaw)agg;

        Action<object?, object?> h1 = (_, _) => { };
        Action<object?, object?> h2 = (_, _) => { };

        CheckEvents(agg,
            () => raw.Bind(h1),
            () => raw.Bind(h2),
            () => raw.Unbind(h1),
            () => raw.Unbind(h2));
    }

    // RawAggregator overload tests
    [Test]
    public void RawAggregator_BlindBind_SubscriptionEvents()
    {
        IBindableRaw[] raws = { new Mutable<int>(), new Mutable<int>() };
        var agg = new BindableAggregator<int>(raws, _ => 0);

        Action h1 = () => { };
        Action h2 = () => { };

        CheckEvents(agg,
            () => agg.Bind(h1, false),
            () => agg.Bind(h2, false),
            () => agg.Unbind(h1),
            () => agg.Unbind(h2));
    }

    [Test]
    public void RawAggregator_FullBind_SubscriptionEvents()
    {
        IBindableRaw[] raws = { new Mutable<int>(), new Mutable<int>() };
        var agg = new BindableAggregator<int>(raws, _ => 0);

        Action<int, int> h1 = (_, _) => { };
        Action<int, int> h2 = (_, _) => { };

        CheckEvents(agg,
            () => agg.Bind(h1),
            () => agg.Bind(h2),
            () => agg.Unbind(h1),
            () => agg.Unbind(h2));
    }

    [Test]
    public void RawAggregator_RawBind_SubscriptionEvents()
    {
        IBindableRaw[] raws = { new Mutable<int>(), new Mutable<int>() };
        var agg = new BindableAggregator<int>(raws, _ => 0);
        var raw = (IBindableRaw)agg;

        Action<object?> h1 = _ => { };
        Action<object?> h2 = _ => { };

        CheckEvents(agg,
            () => raw.Bind(h1, false),
            () => raw.Bind(h2, false),
            () => raw.Unbind(h1),
            () => raw.Unbind(h2));
    }

    [Test]
    public void RawAggregator_RawFullBind_SubscriptionEvents()
    {
        IBindableRaw[] raws = { new Mutable<int>(), new Mutable<int>() };
        var agg = new BindableAggregator<int>(raws, _ => 0);
        var raw = (IBindableRaw)agg;

        Action<object?, object?> h1 = (_, _) => { };
        Action<object?, object?> h2 = (_, _) => { };

        CheckEvents(agg,
            () => raw.Bind(h1),
            () => raw.Bind(h2),
            () => raw.Unbind(h1),
            () => raw.Unbind(h2));
    }

    // Decorator overload tests
    [Test]
    public void Decorator_BlindBind_SubscriptionEvents()
    {
        var m = new Mutable<int>();
        var dec = m.ConvertTo(x => x.ToString());

        Action h1 = () => { };
        Action h2 = () => { };

        CheckEvents(dec,
            () => dec.Bind(h1, false),
            () => dec.Bind(h2, false),
            () => dec.Unbind(h1),
            () => dec.Unbind(h2));
    }

    [Test]
    public void Decorator_FullBind_SubscriptionEvents()
    {
        var m = new Mutable<int>();
        var dec = m.ConvertTo(x => x.ToString());

        Action<string, string> h1 = (_, _) => { };
        Action<string, string> h2 = (_, _) => { };

        CheckEvents(dec,
            () => dec.Bind(h1),
            () => dec.Bind(h2),
            () => dec.Unbind(h1),
            () => dec.Unbind(h2));
    }

    [Test]
    public void Decorator_RawBind_SubscriptionEvents()
    {
        var m = new Mutable<int>();
        var dec = m.ConvertTo(x => x.ToString());
        var raw = (IBindableRaw)dec;

        Action<object?> h1 = _ => { };
        Action<object?> h2 = _ => { };

        CheckEvents(dec,
            () => raw.Bind(h1, false),
            () => raw.Bind(h2, false),
            () => raw.Unbind(h1),
            () => raw.Unbind(h2));
    }

    [Test]
    public void Decorator_RawFullBind_SubscriptionEvents()
    {
        var m = new Mutable<int>();
        var dec = m.ConvertTo(x => x.ToString());
        var raw = (IBindableRaw)dec;

        Action<object?, object?> h1 = (_, _) => { };
        Action<object?, object?> h2 = (_, _) => { };

        CheckEvents(dec,
            () => raw.Bind(h1),
            () => raw.Bind(h2),
            () => raw.Unbind(h1),
            () => raw.Unbind(h2));
    }
} 