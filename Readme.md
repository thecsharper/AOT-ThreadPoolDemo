``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (******)
Intel Core i5-6600K CPU 3.50GHz (Skylake), 1 CPU, 4 logical and 4 physical cores
.NET SDK=7.0.100-preview.7.22377.5
  [Host]        : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2
  .NET 6.0      : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2
  .NET 7.0      : .NET 7.0.0 (7.0.22.37506), X64 RyuJIT AVX2
  NativeAOT 7.0 : .NET 7.0.0 (7.0.22.37506), X64 RyuJIT AVX2


```
|    Method |                Job |            Runtime |    N |     Mean |     Error |    StdDev |
|---------- |------------------- |------------------- |----- |---------:|----------:|----------:|
| BenchWait |           .NET 6.0 |           .NET 6.0 | 1000 | 4.226 ms | 0.0405 ms | 0.0359 ms |
| BenchWait |           .NET 7.0 |           .NET 7.0 | 1000 | 4.356 ms | 0.0516 ms | 0.0458 ms |
| BenchWait | .NET Framework 4.8 | .NET Framework 4.8 | 1000 |       NA |        NA |        NA |
| BenchWait |      NativeAOT 7.0 |      NativeAOT 7.0 | 1000 | 4.285 ms | 0.0758 ms | 0.0778 ms |

Benchmarks with issues:
  Program.BenchWait: .NET Framework 4.8(Runtime=.NET Framework 4.8) [N=1000]

  From https://www.awise.us/2022/09/18/dotnet-nativeaot-windows-threadpool.html