using Xunit;

public class ProbabilisticCounting{
    [Fact]
    public static void ShouldEstimateWithLargeNumberofBits(){
        ICardinality<int> card = new ProbabilisticCounting<int>(1);
        card.AddToSet(10);
        card.AddToSet(10);
        card.AddToSet(20);
        Assert.Equal(1, card.GetCardinality());
    }

    [Fact]
    public static void ShouldClearSet(){
        ICardinality<int> card = new ProbabilisticCounting<int>(5);
        card.AddToSet(10);
        Assert.Equal(6, card.GetCardinality());
        card.ClearSet();
        Assert.Equal(6, card.GetCardinality());
    }

    [Fact]
    public static void ShouldEstimateForLargeSet(){
        ICardinality<int> card = new ProbabilisticCounting<int>(8);
        for (int i = 0; i < 500; i++){
            card.AddToSet(i);
        }
        Assert.Equal(661, card.GetCardinality());
    }
}