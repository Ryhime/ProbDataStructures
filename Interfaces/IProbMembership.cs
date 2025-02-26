public interface IProbMembership<T>{
    /// <summary>
    /// Checks if the object is in the set
    /// </summary>
    /// <param name="toCheck">The object to check if the object is in the set</param>
    /// <returns>If the object is in the set</returns>
    public bool ObjectInSet(T toCheck);

    /// <summary>
    /// Adds the object to the set
    /// </summary>
    /// <param name="toAdd">The object to add to the set</param>
    public void AddToSet(T toAdd);
}