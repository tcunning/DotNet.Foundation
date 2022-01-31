using System;
using Foundation.Iot.Collection;

namespace Foundation.Iot.Endian;

/// <summary>
/// <para>
/// Provides a no-heap solution for converting an Endian value to/from memory which is both faster and more memory efficient then
/// <see cref="BitConverter"/>.  This is targeted for processing raw data which is changing at a high rate such as for gaming application
/// or IoT devices.
/// </para>
/// <para>
/// The following operations have been written to allocate any heap (GC) memory:
/// <list type="bullet">
/// <item><description>Creation with value</description></item>
/// <item><description>Creation with <see cref="ArraySegment{T}"/>, byte array, etc</description></item>
/// <item><description>Indexers</description></item>
/// <item><description><see cref="GetEnumerator"/> used by foreach"/></description></item>
/// <item><description><see cref="CopyTo(int,byte[],int,int)"/></description></item>
/// </list>
/// </para>
/// <para>
/// Operations that cause arrays or lists to be create will cause heap allocation such as <see cref="ToArray"/> and <see cref="ToList"/>. 
/// </para>
/// </summary>
public readonly ref struct EndianUInt32 
{
    /// <summary>
    /// Gets the <see cref="EndianFormat"/> being used to do the conversation.
    /// </summary>
    public EndianFormat EndianFormat { get; }

    /// <summary>
    /// Get's the value taking into account the <see cref="EndianFormat"/> when converting to/from a memory buffer.
    /// </summary>
    public UInt32 Value { get; }

    /// <summary>
    /// The size of the native value used to determine how many bytes are used by the native <see cref="Value"/>.
    /// </summary>
    public int Count => sizeof(UInt32);

    /// <summary>
    /// <para>
    /// The data returned by this value takes into account the desired <see cref="EndianFormat"/> and it does this without
    /// allocating any heap memory.
    /// </para>
    /// <example>
    /// <see cref="Foundation.Iot.Endian.EndianFormat.Little"/> with value of 0x12345678
    /// and Given a Value of 0x12345678:
    /// <para/>this[0] == 0x78
    /// <para/>this[1] == 0x56
    /// <para/>this[2] == 0x34
    /// <para/>this[3] == 0x12
    /// </example>
    /// </summary>
    /// <param name="index">The index of the byte to retrieve after taking into account the byte order via <see cref="EndianFormat"/></param>
    /// <returns>The byte at the given index after taking into account the byte order via <see cref="EndianFormat"/></returns>
    public byte this[int index] => (byte)((Value >> EndianValueManipulation<UInt32>.BitsToShift(EndianFormat, index)) & Byte.MaxValue);

    /// <summary>
    /// <para>
    /// This is the main constructor which takes a value in its native endian format without using heap memory.  
    /// </para>
    /// <para>
    /// This is a ref struct so it will never be on the heap and the lifetime of this struct is expected to be short,
    /// usually just enough time to finish whatever conversation is desired.
    /// </para>
    /// </summary>
    /// <param name="value">The value in its native format</param>
    /// <param name="endianFormat">The endian format to use when converting to/from memory</param>
    public EndianUInt32(UInt32 value, EndianFormat endianFormat)
    {
        EndianFormat = endianFormat;
        Value = value;
    }

    /// <summary>
    /// <para>
    /// This constructor takes an ArraySegment which must contain exactly, no more and no less, the expected number of
    /// bytes <see cref="Count"/> need to convert it the <see cref="Value"/> without using any heap memory.
    /// </para>
    /// <para>
    /// This is a ref struct so it will never be on the heap and the lifetime of this struct is expected to be short,
    /// usually just enough time to finish whatever conversation is desired.
    /// </para>
    /// </summary>
    /// <param name="buffer">An ArraySegment which must contain exactly the expected number of bytes <see cref="Count"/>
    /// needed by the value.  Anything that can be converted into an ArraySegment can also be used such as a byte array, but
    /// it too must exactly match the expected numbers of bytes <see cref="Count"/>.</param>
    /// <param name="endianFormat"></param>
    public EndianUInt32(ArraySegment<byte> buffer, EndianFormat endianFormat) : this(MakeValue(buffer, endianFormat), endianFormat)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="endianFormat"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static UInt32 MakeValue(ArraySegment<byte> buffer, EndianFormat endianFormat)
    {
        if (buffer.Count != EndianValueManipulation<UInt32>.NumberOfBytes)
            throw new ArgumentOutOfRangeException(nameof(buffer), $"Unexpected buffer size");

        UInt32 value = 0x00;
        for (var index = 0; index < buffer.Count; index++) {
            value |= ((UInt32)buffer[index]) << EndianValueManipulation<UInt32>.BitsToShift(endianFormat, index);
        }

        return value;
    }

    public byte[] ToArray() => GetEnumerator().ToArray();

    public List<byte> ToList() => GetEnumerator().ToList();

    public void CopyTo(int sourceStartIndex, byte[] destinationArray, int destinationStartIndex, int count) => GetEnumerator().CopyTo(sourceStartIndex, destinationArray, destinationStartIndex, count);

    public void CopyTo(byte[] destinationArray, int destinationStartIndex = 0) => GetEnumerator().CopyTo(destinationArray, destinationStartIndex);
    
    public EnumeratorBuilderForFour<byte> GetEnumerator() => new (this[0], this[1], this[2], this[3]);
}