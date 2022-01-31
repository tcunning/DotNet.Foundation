using System.Buffers;
using System.Runtime.CompilerServices;
using Foundation.Iot.BasicType;

namespace Foundation.Iot.Endian;

public struct EndianEnumerator<TValue> : IEnumerator<byte>
    where TValue : IReadOnlyList<byte>
{
    private readonly TValue _bytes;
    private int _currentIndex;

    public EndianEnumerator(TValue bytes)
    {
        _bytes = bytes;
        _currentIndex = -1;
    }

    public bool MoveNext()
    {
        var nextIndex = _currentIndex + 1;
        if (nextIndex >= _bytes.Count )
            return false;

        _currentIndex = nextIndex;
        return true;
    }

    public void Reset() => _currentIndex = -1;

    public byte Current => _bytes[_currentIndex];

    object IEnumerator.Current => Current;

    void IDisposable.Dispose() { }
}

public readonly struct Endian<TValue> : IReadOnlyList<byte>
    where TValue : struct, IComparable, IComparable<TValue>, IConvertible, IEquatable<TValue>
{
    public delegate byte RightShiftValue(TValue value, int numberOfBitsToShift);

    public EndianFormat EndianFormat { get; }

    public TValue Value { get; }

    private const int BitsToShiftIndex = 1; // 0 = EndianFormatIndex, 1 = BitsToShiftIndex
    private const int MaxEndianness = 2;    // There is only Big and Little Endian

    // ReSharper disable once StaticMemberInGenericType (We want each TValue to generate it's own static table)
    private static readonly int[,] BitsToShift;

    public int Count => BitsToShift.GetLength(BitsToShiftIndex);

    private readonly RightShiftValue _rightShift;    // Value >> numberOfBitsToShift

    static Endian()
    {
        var count = Unsafe.SizeOf<TValue>();
        BitsToShift = new int[MaxEndianness, count];
        for (int index = 0; index < count; index++) {
            var bytePosition = count - 1 - index;
            BitsToShift[(int)EndianFormat.Big, index] = bytePosition * ByteExtension.BitsInByte;

            BitsToShift[(int) EndianFormat.Little, index] = index * ByteExtension.BitsInByte;
        }
    }

    public byte this[int index] => (byte)(_rightShift(Value, BitsToShift[(int)EndianFormat, index]) & Byte.MaxValue);

    public Endian(TValue value, EndianFormat endianFormat, RightShiftValue shiftValue)
    {
        EndianFormat = endianFormat;
        Value = value;
        _rightShift = shiftValue;
    }
    
    public IEnumerator<byte> GetEnumerator() => new EndianEnumerator<Endian<TValue>>(this);

    IEnumerator IEnumerable.GetEnumerator() => new EndianEnumerator<Endian<TValue>>(this);
}


internal static class EndianValueManipulation<TValue>
    where TValue : struct, IComparable, IComparable<TValue>, IConvertible, IEquatable<TValue>
{
    private const int BitsToShiftIndex = 1; // 0 = EndianFormatIndex, 1 = BitsToShiftIndex
    private const int MaxEndianness = 2;    // There is only Big and Little Endian

    public static int NumberOfBytes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _bitsToShift.GetLength(BitsToShiftIndex);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int BitsToShift(EndianFormat endianFormat, int index) => _bitsToShift[(int)endianFormat, index];
    
    // ReSharper disable once StaticMemberInGenericType (We want each TValue to generate it's own static table)
    private static readonly int[,] _bitsToShift;

    static EndianValueManipulation()
    {
        var count = Unsafe.SizeOf<TValue>();
        _bitsToShift = new int[MaxEndianness, count];
        for (int index = 0; index < count; index++)
        {
            var bytePosition = count - 1 - index;
            _bitsToShift[(int)EndianFormat.Big, index] = bytePosition * ByteExtension.BitsInByte;

            _bitsToShift[(int)EndianFormat.Little, index] = index * ByteExtension.BitsInByte;
        }
    }
}










public struct ByteEnumerator : IEnumerator<byte>
{
    private readonly int _count;
    private readonly byte _byte0;
    private readonly byte _byte1;
    private readonly byte _byte2;
    private readonly byte _byte3;
    private int _currentIndex;
    
    public ByteEnumerator(byte byte0, byte byte1, byte byte2, byte byte3)
    {
        _byte0 = byte0;
        _byte1 = byte1;
        _byte2 = byte2;
        _byte3 = byte3;
        _currentIndex = -1;
        _count = 4;
    }

    public bool MoveNext()
    {
        var nextIndex = _currentIndex + 1;
        if (nextIndex >= _count)
            return false;

        _currentIndex = nextIndex;
        return true;
    }

    public void Reset() => _currentIndex = -1;

    public byte Current
    {
        get
        {
            return _currentIndex switch
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

    public byte[] ToArray() => new[] {_byte0, _byte1, _byte2, _byte3};

    public List<byte> ToList() => new(_count) {_byte0, _byte1, _byte2, _byte3};


    public void CopyTo(byte[] destinationArray, int destinationStartIndex)
    {
        destinationArray[destinationStartIndex + 0] = _byte0;
        destinationArray[destinationStartIndex + 1] = _byte1;
        destinationArray[destinationStartIndex + 2] = _byte2;
        destinationArray[destinationStartIndex + 3] = _byte3;
    }

    public void CopyTo(int sourceStartIndex, byte[] destinationArray, int destinationStartIndex, int count)
    {
        if (sourceStartIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(sourceStartIndex));

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

public readonly ref struct EndianUInt32 
{
    public EndianFormat EndianFormat { get; }

    public UInt32 Value { get; }

    public int Count => sizeof(UInt32);

    public byte this[int index] => (byte)((Value >> EndianValueManipulation<UInt32>.BitsToShift(EndianFormat, index)) & Byte.MaxValue);

    public EndianUInt32(UInt32 value, EndianFormat endianFormat)
    {
        EndianFormat = endianFormat;
        Value = value;
    }

    public byte[] ToArray() => GetEnumerator().ToArray();

    public List<byte> ToList() => GetEnumerator().ToList();

    public void CopyTo(int sourceStartIndex, byte[] destinationArray, int destinationStartIndex, int count) => GetEnumerator().CopyTo(sourceStartIndex, destinationArray, destinationStartIndex, count);

    public void CopyTo(byte[] destinationArray, int destinationStartIndex = 0) => GetEnumerator().CopyTo(destinationArray, destinationStartIndex);
    
    public ByteEnumerator GetEnumerator() => new ByteEnumerator(this[0], this[1], this[2], this[3]);
}