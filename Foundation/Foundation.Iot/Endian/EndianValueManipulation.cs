using System.Runtime.Versioning;
using Foundation.Iot.BasicType;

namespace Foundation.Iot.Endian;

/// <summary>
/// Used to assist with Endian Value Manipulations. The given <see cref="TValue"/> should be
/// an integer type such as int, long, UInt16, ...
/// </summary>
/// <typeparam name="TValue">The type of the value that's being manipulated</typeparam>
/// <typeparam name="TValueSize">The type size of the value being manipulated, we can use a different size
/// then the given TValue type so we can use larger types to manipulate smaller types, for example to
/// manipulate UInt48 (6 byte) types UInt64 (8 byte) types are used.</typeparam>
internal static class EndianValueManipulation<TValue, TValueSize>
    where TValue : struct, IComparable, IComparable<TValue>, IConvertible, IEquatable<TValue>
    where TValueSize : ITypeSideOf
{
    /// <summary>
    /// The index used for the <see cref="BitsToShiftTable"/> to get the bits to shift.
    /// 0 = EndianFormatIndex, 1 = BitsToShiftIndex
    /// </summary>
    private const int BitsToShiftIndex = 1;

    /// <summary>
    /// There are only 2 choices when picking the Endianness: Big and Little
    /// </summary>
    private const int MaxEndianness = 2;

    /// <summary>
    /// Get's the number of bytes for the <see cref="TValue"/>.
    /// </summary>
    public static int NumberOfBytes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => BitsToShiftTable.GetLength(BitsToShiftIndex);
    }

    /// <summary>
    /// Returns the number of bits that should be shifted in a value to get the byte associated
    /// with the given <see cref="index"/> taking into account the <see cref="EndianFormat"/>
    /// </summary>
    /// <param name="endianFormat">The endian format being considered</param>
    /// <param name="index">Index to the byte of interest, must be from 0 to less then <see cref="NumberOfBytes"/></param>
    /// <returns>The number of bits that need to be shifted in order to get the specified byte at the given <see cref="index"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int BitsToShift(EndianFormat endianFormat, int index) => BitsToShiftTable[(int)endianFormat, index];
    
    /// <summary>
    /// This is a lookup table to allow the number of bits to be retrieved for a given <see cref="EndianFormat"/> and byte index.
    /// The first index is the <see cref="EndianFormat"/> and the second index is the <see cref="BitsToShift"/>
    /// </summary>
    // ReSharper disable once StaticMemberInGenericType (We want each TValue to generate it's own static table)
    private static readonly int[,] BitsToShiftTable;

    /// <summary>
    /// This sets up the <see cref="BitsToShiftTable"/> table with the proper values.
    /// </summary>
    [RequiresPreviewFeatures]
    static EndianValueManipulation()
    {
        var count = TValueSize.TypeSizeOf;
        BitsToShiftTable = new int[MaxEndianness, count];
        for (int index = 0; index < count; index++)
        {
            var bytePosition = count - 1 - index;
            BitsToShiftTable[(int)EndianFormat.Big, index] = bytePosition * ByteExtension.BitsInByte;

            BitsToShiftTable[(int)EndianFormat.Little, index] = index * ByteExtension.BitsInByte;
        }
    }
}