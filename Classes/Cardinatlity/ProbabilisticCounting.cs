using System.Collections;

public class ProbabilisticCounting<T> : ICardinality<T>
{
    /// <summary>
    /// The counters to use in the set
    /// </summary>
    List<BitArray> counters;
    /// <summary>
    /// A constant phi value given from the book used to calculate cardinality
    /// </summary>
    private const float phi = .77351f;

    /// <summary>
    /// Default Constructor
    /// </summary>
    /// <param name="numCounters">The number of counters to use</param>
    public ProbabilisticCounting(int numCounters){
        counters = new List<BitArray>(numCounters);
        for (int i = 0; i < numCounters; i++){
            counters.Add(new BitArray(numCounters));
        }
    }

    /// <summary>
    /// Gets the rank of a hash by getting the lowest 
    /// </summary>
    /// <param name="eval">The hashed value to get the rank of</param>
    /// <returns>The rank of eval</returns>
    private int Rank(int eval){
        List<char> repr = Convert.ToString(eval, 2).Reverse().ToList();
        int index = repr.FindIndex(x => '1' == x);
        if (-1 == index){
            return 0;
        }
        
        return index % counters.Count;
    }

    /// <summary>
    /// Adds the generic object to the set
    /// </summary>
    /// <param name="toAdd">The object to add to the set</param>
    /// <exception cref="ArgumentNullException">Throws if toAdd is null</exception>
    public void AddToSet(T toAdd)
    {
        if (null == toAdd){
            throw new ArgumentNullException();
        }

        int hash = Math.Abs(toAdd.GetHashCode());
        int r = hash % counters.Count;
        int q = hash / counters.Count;
        int rank = Rank(q);
        counters[r].Set(rank, true);
    }

    /// <summary>
    /// Clears the set
    /// </summary>
    public void ClearSet()
    {
        int size = counters.Count;
        counters = new List<BitArray>(size);
        for (int i = 0; i < size; i++){
            counters.Add(new BitArray(size));
        }
    }

    /// <summary>
    /// Estimates the number of unique elements in the set
    /// </summary>
    /// <returns>An estimate of the number of unique elements in the set</returns>
    public int GetCardinality()
    {
        int total = 0;
        foreach (BitArray bits in counters){
            for (int i = 0; i < bits.Count; i++){
                if (!bits.Get(i)){
                    total += i;
                    break;
                }
            }
        }

        return (int)Math.Floor((counters.Count / phi) * Math.Pow(2, (1.0/counters.Count) * total));
    }
}