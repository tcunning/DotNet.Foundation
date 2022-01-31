namespace Foundation.Iot.Collection;

/// <summary>
/// Assists in the tracking of Enumerators such as those returned by <see cref="IEnumerable{T}.GetEnumerator"/>
/// </summary>
public struct EnumeratorTracker
{
    /// <summary>
    /// Used as the index when the enumeration hasn't been started yet or if it has been restarted.
    /// </summary>
    public const int EnumerationNotStarted = -1;

    /// <summary>
    /// The number of items that are going to be enumerated if the enumeration runs to completion
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// The index of the current item being enumerated.  Is EnumerationNotStarted if the enumeration
    /// hasn't been started or if it has been reset. 
    /// </summary>
    public int CurrentIndex { get; private set; }

    /// <summary>
    /// Creates a new tracker with the count of the number of items that will be enumerated
    /// </summary>
    /// <param name="count">The number of items that will be enumerated</param>
    /// <exception cref="ArgumentOutOfRangeException">If <see cref="count"/> is less then 0</exception>
    public EnumeratorTracker(int count)
    {
        if( count < 0 )
            throw new ArgumentOutOfRangeException(nameof(count));

        Count = count;
        CurrentIndex = EnumerationNotStarted;
    }
    
    /// <summary>
    /// Starts and/or advances the enumeration to the first and/or next item.
    /// </summary>
    /// <returns>True if successful, otherwise we have hit the end of the enumerated list.</returns>
    public bool MoveNext()
    {
        var nextIndex = CurrentIndex + 1;
        if (nextIndex >= Count)
            return false;

        CurrentIndex = nextIndex;
        return true;
    }

    /// <summary>
    /// Resets the enumeration back to the beginning as though the enumeration hadn't been started.
    /// </summary>
    public void Reset() => CurrentIndex = EnumerationNotStarted;
}