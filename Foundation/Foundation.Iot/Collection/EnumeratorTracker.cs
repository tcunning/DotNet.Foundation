namespace Foundation.Iot.Collection;

public struct EnumeratorTracker
{
    public int Count { get; }
    public int CurrentIndex { get; private set; }

    public EnumeratorTracker(int count)
    {
        Count = count;
        CurrentIndex = -1;
    }

    public bool MoveNext()
    {
        var nextIndex = CurrentIndex + 1;
        if (nextIndex >= Count)
            return false;

        CurrentIndex = nextIndex;
        return true;
    }

    public void Reset() => CurrentIndex = -1;
}