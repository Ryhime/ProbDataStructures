public interface ICardinality<T>{
    /// <summary>
    /// Adds the object to the set
    /// </summary>
    /// <param name="toAdd">The object to add</param>
    public void AddToSet(T toAdd);

    /// <summary>
    /// Gets an estimation of the number of unique elements in the set
    /// </summary>
    /// <param name="toCheck"></param>
    /// <returns></returns>
    public int GetCardinality();

    /// <summary>
    /// Clears the set
    /// </summary>
    public void ClearSet();
}