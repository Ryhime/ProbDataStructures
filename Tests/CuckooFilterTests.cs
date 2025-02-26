using Xunit;

public class CuckooFilterTests{

    [Fact]
    public static void TestCreate(){
        IProbMembership<int> bf = new CuckooFilter<int>(5, 5);
        bf.AddToSet(5);
        Assert.True(bf.ObjectInSet(5));
        Assert.False(bf.ObjectInSet(6));
    }

    [Fact]
    public static void TestComputedCreate(){
        IProbMembership<int> bf = new CuckooFilter<int>(5, 5);
        bf.AddToSet(20);
        Assert.False(bf.ObjectInSet(21));
        Assert.True(bf.ObjectInSet(20));
    }

    [Fact]
    public static void StringType(){
        IProbMembership<string> bf = new CuckooFilter<string>(5, 5);
        bf.AddToSet("IN");
        Assert.True(bf.ObjectInSet("IN"));
        Assert.False(bf.ObjectInSet("in"));
        Assert.False(bf.ObjectInSet("NOT IN"));
    }

    [Fact]
    public static void ComplexDataType(){
        IProbMembership<IProbMembership<int>> bf = new CuckooFilter<IProbMembership<int>>(5, 100);
        IProbMembership<int> bf1 = new BloomFilter<int>(5, 200);
        CuckooFilter<int> bf2 = new CuckooFilter<int>(2, 100);
        bf.AddToSet(bf1);
        Assert.True(bf.ObjectInSet(bf1));
        Assert.False(bf.ObjectInSet(bf2));
    }

    [Fact]
    public static void ShouldClearItself(){
        IProbMembership<int> bf = new CuckooFilter<int>(5, 100);
        bf.AddToSet(5);
        Assert.True(bf.ObjectInSet(5));
        bf.ClearSet();
        Assert.False(bf.ObjectInSet(5));
    }

    [Fact]
    public static void ShouldRemoveElement(){
        CuckooFilter<int> bf = new CuckooFilter<int>(2, 1);
        bf.AddToSet(5);
        bf.AddToSet(6);
        Assert.True(bf.ObjectInSet(5));
        bf.RemoveFromSet(5);
        Assert.False(bf.ObjectInSet(5));
        Assert.True(bf.ObjectInSet(6));
    }

    [Fact]
    public static void TestFullFilter(){
        IProbMembership<int> bf = new CuckooFilter<int>(1, 1);
        bf.AddToSet(5);
        Assert.True(bf.ObjectInSet(5));
        bf.AddToSet(6);
        Assert.False(bf.ObjectInSet(5));
        Assert.True(bf.ObjectInSet(6));
    }
}