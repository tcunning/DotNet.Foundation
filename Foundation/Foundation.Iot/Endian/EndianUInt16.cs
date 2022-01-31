using Foundation.Iot.Collection;

namespace Foundation.Iot.Endian;

public readonly ref struct EndianUInt16
{
    public EndianFormat EndianFormat { get; }

    public UInt16 Value { get; }

    public int Count => sizeof(UInt16);

    public byte this[int index] => (byte)((Value >> EndianValueManipulation<UInt16>.BitsToShift(EndianFormat, index)) & Byte.MaxValue);

    public EndianUInt16(UInt16 value, EndianFormat endianFormat)
    {
        EndianFormat = endianFormat;
        Value = value;
    }

    public EndianUInt16(ArraySegment<byte> buffer, EndianFormat endian) : this(MakeValue(buffer, endian), endian)
    {
    }

    public static UInt16 MakeValue(ArraySegment<byte> buffer, EndianFormat endianFormat)
    {
        if (buffer.Count != EndianValueManipulation<UInt16>.NumberOfBytes)
            throw new ArgumentOutOfRangeException(nameof(buffer), $"Unexpected buffer size");

        UInt16 value = 0x00;
        for (var index = 0; index < buffer.Count; index++) {
            value |= (UInt16)(buffer[index] << EndianValueManipulation<UInt16>.BitsToShift(endianFormat, index));
        }

        return value;
    }
    
    public byte[] ToArray() => GetEnumerator().ToArray();

    public List<byte> ToList() => GetEnumerator().ToList();

    public void CopyTo(int sourceStartIndex, byte[] destinationArray, int destinationStartIndex, int count) => GetEnumerator().CopyTo(sourceStartIndex, destinationArray, destinationStartIndex, count);

    public void CopyTo(byte[] destinationArray, int destinationStartIndex = 0) => GetEnumerator().CopyTo(destinationArray, destinationStartIndex);

    public EnumeratorBuilderForTwo<byte> GetEnumerator() => new(this[0], this[1]);
}