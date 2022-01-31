using System.Runtime.CompilerServices;
using Foundation.Iot.Endian;

namespace Foundation.Iot.BasicType;

public readonly struct ReadOnlyListUInt32 : IReadOnlyList<byte>
{
    public readonly EndianFormat EndianFormat;
    public readonly UInt32 Value;

    public byte this[int index]
    {
        get
        {
            //if (index < 0 || index >= Count)
            //    throw new ArgumentOutOfRangeException(nameof(index));

            var bytePosition = EndianFormat == EndianFormat.Little ? index : Count - 1 - index;
            var numberOfBitsToShift = bytePosition * ByteExtension.BitsInByte;
            var value = (byte)((Value >> numberOfBitsToShift) & Byte.MaxValue);

            return value;
        }
    }

    public int Count => Unsafe.SizeOf<UInt32>();

    public ReadOnlyListUInt32(UInt32 value, EndianFormat endianFormat = EndianFormat.Big)
    {
        EndianFormat = endianFormat;
        Value = value;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        for (var index = 0; index < Count; index += 1)
            yield return this[index];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public static class UInt32Extension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyListUInt32 AsList(this UInt32 value, EndianFormat endianFormat = EndianFormat.Big) => new ReadOnlyListUInt32(value, endianFormat);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EndianUInt32 AsEndian(this UInt32 value, EndianFormat endianFormat = EndianFormat.Big) => new EndianUInt32(value, endianFormat);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Endian<UInt16> AsEndian(this UInt16 value, EndianFormat endianFormat = EndianFormat.Big) =>
        new Endian<UInt16>(value, endianFormat, (val, numBits) => (byte)(val >> numBits));

}
