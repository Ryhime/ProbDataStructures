public class CuckooFilter<T> : IProbMembership<T>
{
    /// <summary>
    /// The number of buckets
    /// </summary>
    int numBuckets = 5;

    /// <summary>
    /// The capacity of each bucket
    /// </summary>
    int bucketCapacity = 5;

    /// <summary>
    /// Used to generate random numbers
    /// </summary>
    Random rand;

    /// <summary>
    /// The buckets that hold the fingerprints
    /// </summary>
    List<List<int>> buckets;

    /// <summary>
    /// Default Constructor
    /// </summary>
    /// <param name="bucketCapacity">The capacity of each bucket</param>
    /// <param name="numBuckets">The number of buckets</param>
    public CuckooFilter(int numBuckets, int bucketCapacity){
        if (0 == numBuckets || 0 == bucketCapacity){
            throw new ArgumentException();
        }

        this.bucketCapacity = bucketCapacity;
        this.numBuckets = numBuckets;
        rand = new Random();
        buckets = new List<List<int>>(numBuckets);
        for (int i = 0; i < numBuckets; i++){
            buckets.Add(new List<int>(bucketCapacity));
        }
    }

    /// <summary>
    /// Gets the fingerprint of a hashed object
    /// </summary>
    /// <param name="toGetFingerPrint">The hashed object to get the fingerprint</param>
    /// <returns>The fingerprint of the object</returns>
    private int GetFingerPrint(int toGetFingerPrint){
        return toGetFingerPrint.GetHashCode();
    }

    /// <summary>
    /// Evicts a random element in the bucket
    /// </summary>
    /// <param name="bucket">The bucket to evict from</param>
    /// <returns>The evicted fingerprint</returns>
    private int EvictFromBucket(List<int> bucket){
        int randIndex = rand.Next(0, bucket.Count);
        int toReturn = bucket[randIndex];
        bucket.RemoveAt(randIndex);
        return toReturn;
    }

    /// <summary>
    /// Gets the two hashes and the fingerprint from an object
    /// </summary>
    /// <param name="toHash">The generic object to hash</param>
    /// <returns>The two hash codes pointing to the two buckets and the fingerprint</returns>
    /// <exception cref="ArgumentNullException">Thrown if toHash is null</exception>
    private (int hash1, int hash2, int fingerPrint) GetHashes(T toHash){
        if (toHash == null) {
            throw new ArgumentNullException();
        }

        int hash1 = Math.Abs(toHash.GetHashCode() + 20);
        int fingerPrint = GetFingerPrint(hash1);
        int hash2 = Math.Abs(hash1 + fingerPrint.GetHashCode()) % numBuckets;
        hash1 = hash1 % numBuckets;
        return (hash1, hash2, fingerPrint);
    }

    /// <summary>
    /// Adds the generic object to the set
    /// </summary>
    /// <param name="toAdd">The generic object to add to the set</param>
    public void AddToSet(T toAdd)
    {
        int hash1; int hash2; int fingerPrint;
        (hash1, hash2, fingerPrint) = GetHashes(toAdd);

        List<int> bucket1 = buckets[hash1];
        List<int> bucket2 = buckets[hash2];
        if (bucket1.Contains(fingerPrint) || bucket2.Contains(fingerPrint)){
            return;
        }

        if (bucket1.Count >= bucketCapacity){
            int evictedElement = EvictFromBucket(bucket1);
            // Move to second hash
            if (bucket2.Count >= bucketCapacity){
                EvictFromBucket(bucket2);
                bucket2.Add(evictedElement);
            }
        }
        bucket1.Add(fingerPrint);
    }

    /// <summary>
    /// Removes a generic object from the set
    /// </summary>
    /// <param name="toRemove">The object to remove</param>
    /// <returns>If the object was found and removed from the set</returns>
    public bool RemoveFromSet(T toRemove){
        int hash1; int hash2; int fingerPrint;
        (hash1, hash2, fingerPrint) = GetHashes(toRemove);

        return buckets[hash1].Remove(fingerPrint) || buckets[hash2].Remove(fingerPrint);
    }

    /// <summary>
    /// Clears the entire set of all objects
    /// </summary>
    public void ClearSet()
    {
        buckets = new List<List<int>>(numBuckets);
        for (int i = 0; i < numBuckets; i++){
            buckets.Add(new List<int>(bucketCapacity));
        }
    }

    /// <summary>
    /// Gets whether the object is in the set or not
    /// </summary>
    /// <param name="toCheck">The generic object to check membership in the set</param>
    /// <returns>If the toCheck appears in the set</returns>
    public bool ObjectInSet(T toCheck)
    {
        int hash1; int hash2; int fingerPrint;
        (hash1, hash2, fingerPrint) = GetHashes(toCheck);

        return buckets[hash1].Contains(fingerPrint) || buckets[hash2].Contains(fingerPrint);
    }
}