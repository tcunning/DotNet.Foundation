using Foundation.Iot.BasicType;

namespace Foundation.Iot.Collection;

/// <summary>
/// <para>
/// Allows for the enumeration of a limited set of values of type <see cref="TValue"/> without allocating
/// heap memory.  We limit the number of values so that we can allocate them on the stack and
/// avoid heap allocations.
/// </para>
/// <para>
/// Casting instances of this or returning it from a methods that use <see cref="IEnumerable"/>
/// may require boxing that will allocate heap memory.  To support C# foreach enumeration, it is recommend
/// to use the Duck Type technique to avoid casting to an interface.
/// </para>
/// </summary>
/// <typeparam name="TValue">The value type that is being iterated over</typeparam>
public struct EnumeratorForFour<TValue> : IEnumerator<TValue>, ITypeSideOf
    where TValue : struct
{
    public static int TypeSizeOf => 4;

    private EnumeratorTracker _tracker;

    private readonly TValue _value0;
    private readonly TValue _value1;
    private readonly TValue _value2;
    private readonly TValue _value3;

    /// <summary>
    /// Creates the enumerator for the give values
    /// </summary>
    /// <param name="value0">The 1st value to return</param>
    /// <param name="value1">The 2nd value to return</param>
    /// <param name="value2">The 4th value to return</param>
    /// <param name="value3">The 5th value to return</param>
    public EnumeratorForFour(TValue value0, TValue value1, TValue value2, TValue value3)
    {
        _tracker = new EnumeratorTracker(TypeSizeOf);
        _value0 = value0;
        _value1 = value1;
        _value2 = value2;
        _value3 = value3;
    }

    /// <summary>
    /// Starts and/or advances the enumeration to the first and/or next item.
    /// </summary>
    /// <returns>True if successful, otherwise we have hit the end of the enumerated list.</returns>
    public bool MoveNext() => _tracker.MoveNext();

    /// <summary>
    /// Resets the enumeration back to the beginning as though the enumeration hadn't been started.
    /// </summary>
    public void Reset() => _tracker.Reset();

    /// <summary>
    /// The current value being iterated over.
    /// </summary>
    /// <exception cref="InvalidOperationException">Will be thrown if the iteration hasn't begun or it's been reset and not started.
    /// <see cref="MoveNext"/> must be called to start the iteration</exception>
    public TValue Current => CurrentValueForIndex(_tracker.CurrentIndex);

    /// <summary>
    /// The value associated with the given iteration index.
    /// </summary>
    /// <param name="index">Must be a valid iteration index from 0 to less then the size of <see cref="TValue"/></param>
    /// <returns>The value associated with the index</returns>
    /// <exception cref="InvalidOperationException">If the given index is invalid/outside the iteration range</exception>
    public TValue CurrentValueForIndex(int index)
    {
        return index switch
        {
            0 => _value0,
            1 => _value1,
            2 => _value2,
            3 => _value3,
            _ => throw new InvalidOperationException()
        };
    }

    /// <summary>
    /// The current value being iterated over as a boxed value
    /// </summary>
    /// <exception cref="InvalidOperationException">Will be thrown if the iteration hasn't begun or it's been reset and not started.
    /// <see cref="MoveNext"/> must be called to start the iteration</exception>
    object IEnumerator.Current => Current;

    /// <summary>
    /// Create a new array with all values
    /// </summary>
    /// <returns>Array with all values</returns>
    public TValue[] ToArray() => new[] {_value0, _value1, _value2, _value3};

    /// <summary>
    /// Create a new list with all values
    /// </summary>
    /// <returns>Array with all values</returns>
    public List<TValue> ToList() => new(_tracker.Count) {_value0, _value1, _value2, _value3};

    /// <summary>
    /// Copies the values into the given array without allocating heap memory
    /// </summary>
    /// <param name="destinationArray">The array to copy the values into</param>
    /// <param name="destinationStartIndex">Index in the destination buffer were the copy should be started</param>
    public void CopyTo(TValue[] destinationArray, int destinationStartIndex)
    {
        destinationArray[destinationStartIndex + 0] = _value0;
        destinationArray[destinationStartIndex + 1] = _value1;
        destinationArray[destinationStartIndex + 2] = _value2;
        destinationArray[destinationStartIndex + 3] = _value3;
    }

    /// <summary>
    /// Copies the values into the given array without allocating heap memory
    /// </summary>
    /// <param name="sourceStartIndex">The source index this can be 0 to the size of <see cref="TValue"/></param>
    /// <param name="destinationArray">The array to copy the values into</param>
    /// <param name="destinationStartIndex">Index in the destination buffer were the copy should be started</param>
    /// <param name="count">From 0 to the size of the <see cref="TValue"/> but can't exceed the source buffer size while also taking
    /// into account the <see cref="sourceStartIndex"/></param>
    public void CopyTo(int sourceStartIndex, TValue[] destinationArray, int destinationStartIndex, int count)
    {
        if (sourceStartIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(sourceStartIndex));

        if( destinationStartIndex < 0 )
            throw new ArgumentOutOfRangeException(nameof(destinationStartIndex));

        if ( sourceStartIndex + count > _tracker.Count )
            throw new ArgumentOutOfRangeException(nameof(count));

        if( destinationStartIndex + count > destinationArray.Length)
            throw new ArgumentOutOfRangeException(nameof(count));
        
        if (sourceStartIndex == 0 && count > 0) {
            destinationArray[destinationStartIndex] = _value0;
            destinationStartIndex += 1;
            count -= 1;
        }

        if (sourceStartIndex <= 1 && count > 0) {
            destinationArray[destinationStartIndex] = _value1;
            destinationStartIndex += 1;
            count -= 1;
        }

        if (sourceStartIndex <= 2 && count > 0) {
            destinationArray[destinationStartIndex] = _value2;
            destinationStartIndex += 1;
            count -= 1;
        }

        if (sourceStartIndex <= 3 && count > 0) {
            destinationArray[destinationStartIndex] = _value3;
            //destinationStartIndex += 1;
            //count -= 1;
        }
    }

    /// <summary>
    /// There is nothing to dispose / cleanup
    /// </summary>
    void IDisposable.Dispose() { }
}