public class CountingBloomFilter<T> : IProbMembership<T>
{
    /// <summary>
    /// The counter filter
    /// </summary>
    private int[] counters = new int[1];

    /// <summary>
    /// The number of hash functions to use
    /// </summary>
    private int numHashFns = 1;

    /// <summary>
    /// The direct constructor with number of hash functions and the number of counters
    /// </summary>
    /// <param name="numHashFns">The number of hash functions</param>
    /// <param name="numCounters">The number of counters to filter</param>
    public CountingBloomFilter(int numHashFns, int numCounters){
        SetupCountingBloomFilter(numHashFns, numCounters);
    }

    /// <summary>
    /// Constructor that calculates the best number of hash functions and counters to use
    /// </summary>
    /// <param name="numItemsToAddToFilter">The number of expected items to be held in the filter</param>
    /// <param name="falsePositiveRate">The false positive rate</param>
    public CountingBloomFilter(int numItemsToAddToFilter, float falsePositiveRate){
        // Reference: https://hur.st/bloomfilter/?n=4000&p=1.0E-7&m=&k=
        int numCounters = (int)Math.Ceiling(numItemsToAddToFilter * Math.Log(falsePositiveRate)/Math.Log(1/Math.Pow(2, Math.Log(2))));
        int numHashFns = (int)Math.Round((numCounters / numItemsToAddToFilter) * Math.Log(2));
        SetupCountingBloomFilter(numHashFns, numCounters);
    }

    /// <summary>
    /// Sets up the filter from other constructors
    /// </summary>
    /// <param name="numHashFns">The number of hash functions to use</param>
    /// <param name="numCounters">The number of counters to use in the filter</param>
    private void SetupCountingBloomFilter(int numHashFns, int numCounters){
        counters = new int[numCounters];
        this.numHashFns = numHashFns;
    }

    /// <summary>
    /// Adds the object to the set
    /// </summary>
    /// <param name="toAdd">The object to add</param>
    public void AddToSet(T toAdd)
    {
        int[] indexs = GetHashedIndexs(toAdd);
        foreach (int index in indexs){
            counters[index]++;
        }
    }

    /// <summary>
    /// Removes the object from the set
    /// </summary>
    /// <param name="toRemove">The object to remove from the set</param>
    public void RemoveFromSet(T toRemove){
        int[] indexs = GetHashedIndexs(toRemove);
        foreach (int index in indexs){
            if (counters[index] > 0){
                counters[index]--;
            }
        }
    }

    /// <summary>
    /// Gets the hashed indexs
    /// </summary>
    /// <param name="toHash">The bytes to hash</param>
    /// <param name="bloomFilterSize">The size of the bloom filter in bits</param>
    /// <param name="numHashFns">The number of hash functions to use</param>
    /// <returns>The indexs to check in the bloom filter for the byte array</returns>
    /// <exception cref="ArgumentNullException">Throws if toHash is null</exception>
    private int[] GetHashedIndexs(T toHash)
    {
        if (toHash == null){
            throw new ArgumentNullException();
        }

        int[] indexs = new int[numHashFns];
        for (int i = 0; i < numHashFns; i++){
            indexs[i] = (Math.Abs(toHash.GetHashCode()) + i * 20) % counters.Length;
        }
        return indexs;
    }

    /// <summary>
    /// Removes all the objects from the set
    /// </summary>
    public void ClearSet()
    {
        counters = new int[counters.Length];
    }

    /// <summary>
    /// Can have both false positives and false negatives, determines if the object is in the set
    /// </summary>
    /// <param name="toCheck">The object to check if its in the set</param>
    /// <returns>If the object is in the set</returns>
    public bool ObjectInSet(T toCheck)
    {
        int[] indexs = GetHashedIndexs(toCheck);
        foreach (int index in indexs){
            if (counters[index] <= 0) {
                return false;
            }
        }
        return true;
    }
}