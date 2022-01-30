using System.Runtime.CompilerServices;
using Foundation.Iot.BasicType;

namespace Foundation.Iot.Endian;

public readonly struct Endian<TValue> : IReadOnlyList<byte>
    where TValue : struct, IComparable, IComparable<TValue>, IConvertible, IEquatable<TValue>
{
    public delegate byte RightShiftValue(TValue value, int numberOfBitsToShift);

    public EndianFormat EndianFormat { get; }
    
    public TValue Value { get; }

    public int Count { get; }

    private readonly RightShiftValue? _rightShift;    // Value >> numberOfBitsToShift

    public byte this[int index]
    {
        get
        {
            //if (index < 0 || index >= Count)
            //    throw new ArgumentOutOfRangeException(nameof(index));

            var bytePosition = EndianFormat == EndianFormat.Little ? index : Count - 1 - index;
            var numberOfBitsToShift = bytePosition * ByteExtension.BitsInByte;
            var value = _rightShift?.Invoke(Value, numberOfBitsToShift) ?? 0x00 & Byte.MaxValue;

            return value;
        }
    }

    public Endian(TValue value, EndianFormat endianFormat, RightShiftValue shiftValue)
    {
        EndianFormat = endianFormat;
        Value = value;
        _rightShift = shiftValue;
        Count = Unsafe.SizeOf<TValue>();
    }

    public IEnumerator<byte> GetEnumerator()
    {
        for (var index = 0; index < Count; index += 1)
            yield return this[index];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}