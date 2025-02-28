using Xunit;

public class LinearCountingTests{
    [Fact]
    public static void ShouldGetExactForSingle(){
        ICardinality<int> card = new LinearCounting<int>(1);
        Assert.Equal(0, card.GetCardinality());
        card.AddToSet(5);
        Assert.Equal(1, card.GetCardinality());
    }

    [Fact]
    public static void ShouldEstimateWithLargeNumberofBits(){
        ICardinality<int> card = new LinearCounting<int>(10000);
        card.AddToSet(10);
        card.AddToSet(10);
        card.AddToSet(20);
        Assert.Equal(1, card.GetCardinality());
    }

    [Fact]
    public static void ShouldClearSet(){
        ICardinality<int> card = new LinearCounting<int>(10000);
        card.AddToSet(10);
        Assert.Equal(1, card.GetCardinality());
        card.ClearSet();
        Assert.Equal(0, card.GetCardinality());
    }

    [Fact]
    public static void ShouldEstimateForLargeSet(){
        ICardinality<int> card = new LinearCounting<int>(1000);
        for (int i = 0; i < 500; i++){
            card.AddToSet(i);
        }
        Assert.Equal(693, card.GetCardinality());
    }
}