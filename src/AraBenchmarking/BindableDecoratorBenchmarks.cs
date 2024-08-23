using AsyncReactAwait.Bindable;
using AsyncReactAwait.Bindable.BindableExtensions;
using BenchmarkDotNet.Attributes;

namespace AraBenchmarking;

[MemoryDiagnoser]
public class BindableDecoratorBenchmarks
{
    private IMutable<int>? _mutable;
    private IBindable<object>? _decorator;

    private readonly object _mockObj = new();
    
    [GlobalSetup]
    public void CreateMutable()
    {
        _mutable = new Mutable<int>();
        _decorator = _mutable.ConvertTo(x => _mockObj);
    }

    [Benchmark]
    public void SetMutable()
    {
        _mutable!.Set(2);
    }
    
}