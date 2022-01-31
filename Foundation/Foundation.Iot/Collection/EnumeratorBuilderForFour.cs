namespace Foundation.Iot.Collection;

public struct EnumeratorBuilderForFour<TValue> : IEnumerator<TValue>
    where TValue : struct
{
    private EnumeratorTracker _tracker;

    private readonly TValue _byte0;
    private readonly TValue _byte1;
    private readonly TValue _byte2;
    private readonly TValue _byte3;
    
    public EnumeratorBuilderForFour(TValue byte0, TValue byte1, TValue byte2, TValue byte3)
    {
        _tracker = new EnumeratorTracker(4);
        _byte0 = byte0;
        _byte1 = byte1;
        _byte2 = byte2;
        _byte3 = byte3;
    }

    public bool MoveNext() => _tracker.MoveNext();

    public void Reset() => _tracker.Reset();

    public TValue Current
    {
        get
        {
            return _tracker.CurrentIndex switch
            {
                0 => _byte0,
                1 => _byte1,
                2 => _byte2,
                3 => _byte3,
                _ => throw new InvalidOperationException()
            };
        }
    }

    object IEnumerator.Current => Current;

    public TValue[] ToArray() => new[] {_byte0, _byte1, _byte2, _byte3};

    public List<TValue> ToList() => new(_tracker.Count) {_byte0, _byte1, _byte2, _byte3};
    
    public void CopyTo(TValue[] destinationArray, int destinationStartIndex)
    {
        destinationArray[destinationStartIndex + 0] = _byte0;
        destinationArray[destinationStartIndex + 1] = _byte1;
        destinationArray[destinationStartIndex + 2] = _byte2;
        destinationArray[destinationStartIndex + 3] = _byte3;
    }

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
            destinationArray[destinationStartIndex] = _byte0;
            destinationStartIndex += 1;
            count -= 1;
        }

        if (sourceStartIndex <= 1 && count > 0) {
            destinationArray[destinationStartIndex] = _byte1;
            destinationStartIndex += 1;
            count -= 1;
        }

        if (sourceStartIndex <= 2 && count > 0) {
            destinationArray[destinationStartIndex] = _byte2;
            destinationStartIndex += 1;
            count -= 1;
        }

        if (sourceStartIndex <= 3 && count > 0) {
            destinationArray[destinationStartIndex] = _byte3;
            //destinationStartIndex += 1;
            //count -= 1;
        }
    }

    void IDisposable.Dispose() { }
}