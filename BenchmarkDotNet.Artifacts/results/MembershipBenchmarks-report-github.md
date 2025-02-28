```

BenchmarkDotNet v0.14.0, Linux Mint 21.2 (Victoria)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK 8.0.406
  [Host]     : .NET 8.0.13 (8.0.1325.6609), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.13 (8.0.1325.6609), X64 RyuJIT AVX2


```
| Method              | Mean      | Error     | StdDev    | Gen0     | Gen1   | Allocated  |
|-------------------- |----------:|----------:|----------:|---------:|-------:|-----------:|
| BloomFilter         |  3.119 ms | 0.0133 ms | 0.0111 ms | 570.3125 |      - | 9375.28 KB |
| CountingBloomFilter |  3.188 ms | 0.0165 ms | 0.0155 ms | 570.3125 | 7.8125 |    9379 KB |
| CuckooFilter        | 22.661 ms | 0.3784 ms | 0.3539 ms |        - |      - |   20.03 KB |
| QuotientFilter      |  3.705 ms | 0.0740 ms | 0.0936 ms |        - |      - |   10.21 KB |
