```

BenchmarkDotNet v0.14.0, Linux Mint 21.2 (Victoria)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK 8.0.406
  [Host]     : .NET 8.0.13 (8.0.1325.6609), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.13 (8.0.1325.6609), X64 RyuJIT AVX2


```
| Method              | Mean      | Error     | StdDev    | Allocated |
|-------------------- |----------:|----------:|----------:|----------:|
| BloomFilter         |  2.710 ms | 0.0508 ms | 0.0476 ms |     283 B |
| CountingBloomFilter |  2.536 ms | 0.0341 ms | 0.0319 ms |    1091 B |
| CuckooFilter        | 25.118 ms | 0.2416 ms | 0.2142 ms |   20511 B |
| QuotientFilter      |  2.806 ms | 0.0133 ms | 0.0111 ms |   10451 B |
