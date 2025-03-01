public class QuotientFilter<T> : IProbMembership<T>
{
    /// <summary>
    /// The number of buckets in the cache
    /// </summary>
    int numBuckets;
    /// <summary>
    /// How many elements each bucket can hold
    /// </summary>
    int bucketSize;
    /// <summary>
    /// The actual cache
    /// </summary>
    List<Queue<UInt16>> cache;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="numBuckets">The number of buckets in the cache</param>
    /// <param name="bucketSize">The number of elements each bucket can hold</param>
    public QuotientFilter(int numBuckets, int bucketSize){
        if (0 == numBuckets || 0 == bucketSize){
            throw new ArgumentException();
        }

        this.numBuckets = numBuckets;
        this.bucketSize = bucketSize;

        cache = new List<Queue<UInt16>>(numBuckets);
        for (int i = 0; i < numBuckets; i++){
            cache.Add(new Queue<UInt16>(bucketSize));
        }
    }

    /// <summary>
    /// Gets the location and fingerprint for the object
    /// </summary>
    /// <param name="obj">The object to get the location and fingerprint for</param>
    /// <returns>The bucket index and the fingerprint of the object</returns>
    /// <exception cref="ArgumentNullException">Throws if the object is null</exception>
    private (UInt16 upperBitsLocation, UInt16 lowerBitsFingerprint) GetFingerPrint(T obj){
        if (obj == null){
            throw new ArgumentNullException();
        }

        UInt32 hash = (UInt32) obj.GetHashCode();
        return ((UInt16)((hash >> 16) % numBuckets), (UInt16)(hash & 0xFFFF));
    }

    private void EjectFromBucket(Queue<UInt16> bucketToRemoveFrom){
        bucketToRemoveFrom.Dequeue();
    }

    /// <summary>
    /// Adds the object to the set
    /// </summary>
    /// <param name="toAdd">The object to add</param>
    public void AddToSet(T toAdd)
    {
        UInt16 location; UInt16 fingerprint;
        (location, fingerprint) = GetFingerPrint(toAdd);
        Queue<UInt16> toAddTo = cache[location];

        if (toAddTo.Count >= bucketSize){
            EjectFromBucket(toAddTo);
        }
        toAddTo.Enqueue(fingerprint);
    }

    /// <summary>
    /// Clears all objects from the set
    /// </summary>
    public void ClearSet()
    {
        cache = new List<Queue<UInt16>>(numBuckets);
        for (int i = 0; i < numBuckets; i++){
            cache.Add(new Queue<UInt16>(bucketSize));
        }
    }

    /// <summary>
    /// Returns if the object is in the set or not
    /// </summary>
    /// <param name="toCheck">The object to check</param>
    /// <returns>If toCheck is in the set</returns>
    public bool ObjectInSet(T toCheck)
    {
        UInt16 location; UInt16 fingerprint;
        (location, fingerprint) = GetFingerPrint(toCheck);
        return cache[location].Contains(fingerprint);
    }

    /// <summary>
    /// Removes the element from the set
    /// </summary>
    /// <param name="toRemove">The object to remove</param>
    /// <returns>If an object was found and removed</returns>
    public bool RemoveFromSet(T toRemove){
        UInt16 location; UInt16 fingerprint;
        (location, fingerprint) = GetFingerPrint(toRemove);
        Queue<UInt16> toRemoveFrom = cache[location];
        int beforeRemoveCount = toRemoveFrom.Count;
        cache[location] = new Queue<UInt16>(toRemoveFrom.AsEnumerable().Where(x => x != fingerprint));
        return beforeRemoveCount != cache[location].Count;
    }
}