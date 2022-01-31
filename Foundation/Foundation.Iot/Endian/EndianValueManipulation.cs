using System.Runtime.CompilerServices;
using Foundation.Iot.BasicType;

namespace Foundation.Iot.Endian;

internal static class EndianValueManipulation<TValue>
    where TValue : struct, IComparable, IComparable<TValue>, IConvertible, IEquatable<TValue>
{
    private const int BitsToShiftIndex = 1; // 0 = EndianFormatIndex, 1 = BitsToShiftIndex
    private const int MaxEndianness = 2;    // There is only Big and Little Endian

    public static int NumberOfBytes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => BitsToShiftTable.GetLength(BitsToShiftIndex);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int BitsToShift(EndianFormat endian, int index) => BitsToShiftTable[(int)endian, index];
    
    // ReSharper disable once StaticMemberInGenericType (We want each TValue to generate it's own static table)
    private static readonly int[,] BitsToShiftTable;

    static EndianValueManipulation()
    {
        var count = Unsafe.SizeOf<TValue>();
        BitsToShiftTable = new int[MaxEndianness, count];
        for (int index = 0; index < count; index++)
        {
            var bytePosition = count - 1 - index;
            BitsToShiftTable[(int)EndianFormat.Big, index] = bytePosition * ByteExtension.BitsInByte;

            BitsToShiftTable[(int)EndianFormat.Little, index] = index * ByteExtension.BitsInByte;
        }
    }
}