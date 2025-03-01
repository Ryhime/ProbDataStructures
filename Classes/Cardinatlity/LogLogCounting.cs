public class LogLogCounting<T> : ICardinality<T>
{
    public LogLogCounting(){

    }

    public void AddToSet(T toAdd)
    {
        throw new NotImplementedException();
    }

    public void ClearSet()
    {
        throw new NotImplementedException();
    }

    public int GetCardinality()
    {
        throw new NotImplementedException();
    }
}