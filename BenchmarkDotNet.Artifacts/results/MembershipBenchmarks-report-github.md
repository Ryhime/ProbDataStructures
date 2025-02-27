```

BenchmarkDotNet v0.14.0, Linux Mint 21.2 (Victoria)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK 8.0.406
  [Host]     : .NET 8.0.13 (8.0.1325.6609), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.13 (8.0.1325.6609), X64 RyuJIT AVX2


```
| Method              | Mean       | Error    | StdDev   | Gen0    | Gen1   | Allocated |
|-------------------- |-----------:|---------:|---------:|--------:|-------:|----------:|
| BloomFilter         |   324.7 μs |  4.57 μs |  4.27 μs | 57.1289 |      - | 937.77 KB |
| CountingBloomFilter |   345.0 μs |  1.82 μs |  1.61 μs | 57.6172 | 0.4883 | 941.49 KB |
| CuckooFilter        | 1,835.1 μs | 15.94 μs | 14.91 μs |  1.9531 |      - |  41.42 KB |
