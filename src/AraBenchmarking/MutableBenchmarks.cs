using AsyncReactAwait.Bindable;
using AsyncReactAwait.Bindable.BindableExtensions;
using BenchmarkDotNet.Attributes;

namespace AraBenchmarking;

[MemoryDiagnoser]
public class MutableBenchmarks
{

    private IMutable<int>? _mutable;

    [GlobalSetup]
    public void CreateMutable()
    {
        _mutable = new Mutable<int>();
    }
    
    [Benchmark]
    public void MutableSet()
    {
        _mutable!.Set(2);
    }
}