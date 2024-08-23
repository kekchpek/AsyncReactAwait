using AsyncReactAwait.Bindable;
using AsyncReactAwait.Bindable.BindableExtensions;
using BenchmarkDotNet.Attributes;

namespace AraBenchmarking;

[MemoryDiagnoser]
public class BindableAggregatorBenchmarks
{
    
    private IMutable<int>? _mutable1;
    private IMutable<int>? _mutable2;
    private IBindable<int>? _aggregator;

    [GlobalSetup]
    public void CreateMutable()
    {
        _mutable1 = new Mutable<int>();
        _mutable2 = new Mutable<int>();
        _aggregator = Bindable.Aggregate(_mutable1, _mutable2, (a, b) => a + b);
        _aggregator.Bind(x => { });
    }

    [Benchmark]
    public void SetMutableValues()
    {
        _mutable1!.Set(1);
        _mutable2!.Set(1);
    }

    [Benchmark]
    public void SetMutableValuesTwice()
    {
        _mutable1!.Set(1);
        _mutable2!.Set(2);
        _mutable1!.Set(3);
        _mutable2!.Set(4);
    }

}