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
        int numHashFns = (int)Math.Round(numBitsInFilter / numBitsInFilter * Math.Log(2));
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
            indexs[i] = (Math.Abs(toHash.GetHashCode()) + i * 20) % bitArray.Length;
        }
        return indexs;
    }

    /// <summary>
    /// Adds the object to the bloom filter 
    /// </summary>
    /// <param name="toAdd">The object to add to the filter</param>
    /// <exception cref="ArgumentNullException">Throws if toAdd is null</exception>
    public void AddToSet(T toAdd){
        if (toAdd == null){
            throw new ArgumentNullException();
        }

        int[] indexsToSet = GetHashedIndexs(toAdd);
        foreach (int index in indexsToSet){
            bitArray[index] = true;
        }
    }

    /// <summary>
    /// Gets if the object is in the filter - prone to false positives
    /// </summary>
    /// <param name="toCheck">The object to check if it is in the set</param>
    /// <returns>True if the object is in the set false if the object is not in the set</returns>
    /// <exception cref="ArgumentNullException">Throws if toCheck is null</exception>
    public bool ObjectInSet(T toCheck){
        if (toCheck == null){
            throw new ArgumentNullException();
        }
        int[] indexsToCheck = GetHashedIndexs(toCheck);
        foreach (int index in indexsToCheck){
            if (!bitArray[index]){
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