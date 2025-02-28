using System.Collections;

public class LinearCounting<T> : ICardinality<T>
{
    /// <summary>
    /// The bits used in the set
    /// </summary>
    private BitArray bits;

    /// <summary>
    /// Default Constructor
    /// </summary>
    /// <param name="numFilterBits">The number of bits in the filter</param>
    public LinearCounting(int numFilterBits){
        bits = new BitArray(numFilterBits);
    }

    /// <summary>
    /// Gets the index a generic object should be placed at in the bit field
    /// </summary>
    /// <param name="toGet">The object to hash</param>
    /// <returns>The index the object should look at in the bit field</returns>
    /// <exception cref="ArgumentNullException">Throws if toGet is set to null</exception>
    private int GetBitIndex(T toGet){
        if (toGet == null){
            throw new ArgumentNullException();
        }
        return toGet.GetHashCode() % bits.Count;
    }

    /// <summary>
    /// Adds the object to the set
    /// </summary>
    /// <param name="toAdd">The object to add to the set</param>
    public void AddToSet(T toAdd)
    {
        int index = GetBitIndex(toAdd);
        bits.Set(index, true);
    }

    /// <summary>
    /// Gets the number of unique elements in the set
    /// </summary>
    /// <returns>The number of unique elements in the set</returns>
    public int GetCardinality()
    {
        int numZero = 0;
        for (int i = 0; i < bits.Length; i++){
            numZero += bits.Get(i) ? 0 : 1;
        }

        double log = numZero == 0 ? -1.0 : Math.Log((float)numZero / bits.Length);
        return (int)Math.Floor(-1 * bits.Length * log);
    }

    /// <summary>
    /// Clears the set
    /// </summary>
    public void ClearSet()
    {
        bits = new BitArray(bits.Length);
    }
}