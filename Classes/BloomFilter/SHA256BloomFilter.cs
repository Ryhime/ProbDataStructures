using System.Security.Cryptography;

public class SHA256BloomFilter<T> : BloomFilter<T>
{
    /// <summary>
    /// The hash function to use 
    /// </summary>
    SHA256 hashFn = SHA256.Create();

    public SHA256BloomFilter(int numHashFns, int numBitsInFilter) : base(numHashFns, numBitsInFilter){
    }

    public SHA256BloomFilter(int numBitsInFilter, float falsePositiveRate) : base(numBitsInFilter, falsePositiveRate){
    }
    
    /// <summary>
    /// Gets the hashed indexs override
    /// </summary>
    /// <param name="toHash">The bytes to hash</param>
    /// <param name="bloomFilterSize">The size of the bloom filter in bits</param>
    /// <param name="numHashFns">The number of hash functions to use</param>
    /// <returns>The indexs to check in the bloom filter for the byte array</returns>
    protected override int[] GetHashedIndexs(byte[] toHash, int bloomFilterSize, int numHashFns)
    {
        int[] indexs = new int[numHashFns];
        for (int i = 0; i < numHashFns; i++){
            byte[] toAdd = BitConverter.GetBytes(256 + i*20);
            indexs[i] = BitConverter.ToUInt16(hashFn.ComputeHash(toAdd.Concat(toHash).Concat(toAdd).ToArray())) % bloomFilterSize;
        }
        return indexs;
    }
}