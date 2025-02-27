using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class MembershipBenchmarks{

    int numAddToFilter = 10000;
    int numToCheck = 10000;

    [Benchmark]
    public void BloomFilter()
    {
        BloomFilter<int> bf = new BloomFilter<int>(5, 1000);
        for (int i = 0; i < numAddToFilter; i++){
            bf.AddToSet(i);
        }
        for (int i = 0; i < numToCheck; i++){
            bf.ObjectInSet(i);
        }
    }

    [Benchmark]
    public void CountingBloomFilter()
    {
        CountingBloomFilter<int> bf = new CountingBloomFilter<int>(5, 1000);
        for (int i = 0; i < numAddToFilter; i++){
            bf.AddToSet(i);
        }
        for (int i = 0; i < numToCheck; i++){
            bf.ObjectInSet(i);
        }
    }

    [Benchmark]
    public void CuckooFilter()
    {
        CuckooFilter<int> bf = new CuckooFilter<int>(5, 1000);
        for (int i = 0; i < numAddToFilter; i++){
            bf.AddToSet(i);
        }
        for (int i = 0; i < numToCheck; i++){
            bf.ObjectInSet(i);
        }
    }
}