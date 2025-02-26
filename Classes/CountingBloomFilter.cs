public class CountingBloomFilter<T> : IProbMembership<T>
{
    private int[] counters = new int[1];

    private int numHashFns = 1;

    public CountingBloomFilter(int numHashFns, int numCounters){
        SetupCountingBloomFilter(numHashFns, numCounters);
    }

    public CountingBloomFilter(int numItemsToAddToFilter, float falsePositiveRate){
        // Reference: https://hur.st/bloomfilter/?n=4000&p=1.0E-7&m=&k=
        int numCounters = (int)Math.Ceiling(numItemsToAddToFilter * Math.Log(falsePositiveRate)/Math.Log(1/Math.Pow(2, Math.Log(2))));
        int numHashFns = (int)Math.Round((numCounters / numItemsToAddToFilter) * Math.Log(2));
        SetupCountingBloomFilter(numHashFns, numCounters);
    }

    private void SetupCountingBloomFilter(int numHashFns, int numCounters){
        counters = new int[numCounters];
        this.numHashFns = numHashFns;
    }

    public void AddToSet(T toAdd)
    {
        int[] indexs = GetHashedIndexs(toAdd);
        foreach (int index in indexs){
            counters[index]++;
        }
    }

    public void RemoveFromSet(T toRemove){
        int[] indexs = GetHashedIndexs(toRemove);
        foreach (int index in indexs){
            if (counters[index] > 0){
                counters[index]--;
            }
        }
    }

    /// <summary>
    /// Gets the hashed indexs override
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


    public void ClearSet()
    {
        counters = new int[counters.Length];
    }

    /// <summary>
    /// Can have both false positives and false negatives 
    /// </summary>
    /// <param name="toCheck"></param>
    /// <returns></returns>
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