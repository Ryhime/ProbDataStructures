using System.Collections;

public class BloomFilter<T> : IProbMembership<T>{
    /// <summary>
    /// The bit array that contains the filter
    /// </summary>
    private BitArray bitArray = new BitArray(1);

    /// <summary>
    /// (U) The number of hash functions 
    /// </summary>
    private int numHashFns = 1;

    /// <summary>
    /// Constructor that takes directly the number of hash functions and number of bits in the filter
    /// </summary>
    /// <param name="numHashFns">The number of hash functions</param>
    /// <param name="numBitsInFilter">The number of bits in the filter</param>
    public BloomFilter(int numHashFns, int numBitsInFilter){
        SetupBloomFilter(numHashFns, numBitsInFilter);
    }

    /// <summary>
    /// Constructor that calculates the best number of bits in the filter and number of hash functions to use
    /// </summary>
    /// <param name="numItemsToAddToFilter">The expected number of items to add to the filter</param>
    /// <param name="falsePositiveRate">The false positive rate</param>
    public BloomFilter(int numItemsToAddToFilter, float falsePositiveRate){
        // Reference: https://hur.st/bloomfilter/?n=4000&p=1.0E-7&m=&k=
        int numBitsInFilter = (int)Math.Ceiling(numItemsToAddToFilter * Math.Log(falsePositiveRate)/Math.Log(1/Math.Pow(2, Math.Log(2))));
        int numHashFns = (int)Math.Round((numBitsInFilter / numItemsToAddToFilter) * Math.Log(2));
        SetupBloomFilter(numHashFns, numBitsInFilter);
    }

    /// <summary>
    /// Sets up the bloom filter from other constructors 
    /// </summary>
    /// <param name="numHashFns">The number of hash functions</param>
    /// <param name="numBitsInFilter">The number of bits in the filter</param>
    private void SetupBloomFilter(int numHashFns, int numBitsInFilter){
        bitArray = new BitArray(numBitsInFilter);
        this.numHashFns = numHashFns;
    }

    /// <summary>
    /// Adds the object to the bloom filter
    /// </summary>
    /// <param name="toAdd">The object to add to the filter</param>
    /// <exception cref="ArgumentNullException">Throws if toAdd is null</exception>
    public void AddToSet(T toAdd){
        if (null == toAdd){
            throw new ArgumentException();
        }

        Span<int> indexsToSet = stackalloc int[numHashFns];
        for (int i = 0; i < numHashFns; i++){
            indexsToSet[i] = (Math.Abs(toAdd.GetHashCode()) + i * 20) % bitArray.Length;
        }

        foreach (int index in indexsToSet) {
            bitArray.Set(index, true);
        }
    }

    /// <summary>
    /// Gets if the object is in the filter - prone to false positives
    /// </summary>
    /// <param name="toCheck">The object to check if it is in the set</param>
    /// <returns>True if the object is in the set false if the object is not in the set</returns>
    /// <exception cref="ArgumentNullException">Throws if toCheck is null</exception>
    public bool ObjectInSet(T toCheck){
        if (null == toCheck){
            throw new ArgumentNullException();
        }

        Span<int> indexsToCheck = stackalloc int[numHashFns];
        for (int i = 0; i < numHashFns; i++){
            indexsToCheck[i] = (Math.Abs(toCheck.GetHashCode()) + i * 20) % bitArray.Length;
        }

        foreach (int index in indexsToCheck){
            if (!bitArray.Get(index)){
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Clears the objects held in the set
    /// </summary>
    public void ClearSet(){
        bitArray = new BitArray(bitArray.Length);
    }
}