using Xunit;

public class SHA256BloomFilterTests{

    [Fact]
    public static void TestCreate(){
        SHA256BloomFilter<int> bf = new SHA256BloomFilter<int>(5, 100);
        bf.AddToBloomFilter(5);
        Assert.True(bf.GetErrorProneInSet(5));
        Assert.False(bf.GetErrorProneInSet(6));
    }

    [Fact]
    public static void TestComputedCreate(){
        SHA256BloomFilter<int> bf = new SHA256BloomFilter<int>(1, (float).000000000000000000001);
        bf.AddToBloomFilter(20);
        Assert.False(bf.GetErrorProneInSet(21));
        Assert.True(bf.GetErrorProneInSet(20));
    }

    [Fact]
    public static void TestOverlapFalsePositive(){
        SHA256BloomFilter<int> bf = new SHA256BloomFilter<int>(1, 1);
        bf.AddToBloomFilter(55);
        Assert.True(bf.GetErrorProneInSet(10));
        Assert.True(bf.GetErrorProneInSet(55));
    }
}