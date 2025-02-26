using Xunit;

public class BloomFilterTests{

    [Fact]
    public static void TestCreate(){
        IProbMembership<int> bf = new BloomFilter<int>(5, 100);
        bf.AddToSet(5);
        Assert.True(bf.ObjectInSet(5));
        Assert.False(bf.ObjectInSet(6));
    }

    [Fact]
    public static void TestComputedCreate(){
        IProbMembership<int> bf = new BloomFilter<int>(1, (float).000000000000000000001);
        bf.AddToSet(20);
        Assert.False(bf.ObjectInSet(21));
        Assert.True(bf.ObjectInSet(20));
    }

    [Fact]
    public static void TestOverlapFalsePositive(){
        IProbMembership<int> bf = new BloomFilter<int>(1, 1);
        bf.AddToSet(55);
        Assert.True(bf.ObjectInSet(10));
        Assert.True(bf.ObjectInSet(55));
    }

    [Fact]
    public static void StringType(){
        IProbMembership<string> bf = new BloomFilter<string>(5, 100);
        bf.AddToSet("IN");
        Assert.True(bf.ObjectInSet("IN"));
        Assert.False(bf.ObjectInSet("in"));
        Assert.False(bf.ObjectInSet("NOT IN"));
    }

    [Fact]
    public static void ComplexDataType(){
        IProbMembership<IProbMembership<int>> bf = new BloomFilter<IProbMembership<int>>(5, 100);
        IProbMembership<int> bf1 = new BloomFilter<int>(5, 200);
        BloomFilter<int> bf2 = new BloomFilter<int>(2, 100);
        bf.AddToSet(bf1);
        Assert.True(bf.ObjectInSet(bf1));
        Assert.False(bf.ObjectInSet(bf2));
    }
}