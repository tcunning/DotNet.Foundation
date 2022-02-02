namespace Foundation.Iot.Endian;

/// <summary>
/// Provides several extension methods that make this "easier" to convert values to/from buffers
/// in the desired <see cref="EndianFormat"/>.
/// </summary>
public static class EndianInt48Extension
{
    /// <summary>
    /// Creates a <see cref="EndianInt48"/> from the given value that will use the <see cref="EndianFormat"/>
    /// for endian conversions.
    /// <example>
    /// <code>
    /// Int48 value = 0x12345678;
    /// var endianValue = value.AsEndianInt48(EndianFormat.Big);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="value">The value to use for endian value, this will of course be in the native endian format</param>
    /// <param name="endianFormat">The endian format that should be used when going to/from memory</param>
    /// <returns>The <see cref="EndianInt48"/> that can format the value as the given <see cref="EndianFormat"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EndianInt48 AsEndianInt48(this UInt64 value, EndianFormat endianFormat) =>
        new (value, endianFormat);

    /// <summary>
    /// Creates a <see cref="EndianInt48"/> from the given buffer that uses the <see cref="EndianFormat"/>
    /// for conversions.
    /// <example>
    /// <code>
    /// var buffer = new byte[] { 0xFF, 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0, 0xFF };
    /// var bufferSegment = new ArraySegment&lt;byte&gt;(buffer, 1, sizeof(Int48));
    /// var endianValue = bufferSegment.AsEndianInt48(EndianFormat.Big);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="buffer">A buffer in the form of an array segment that is used as the source of the endian value</param>
    /// <param name="endianFormat">The endian format of the data used to determine how to interpret the bytes in the buffer</param>
    /// <returns>The <see cref="EndianInt48"/> that reads the given buffer that is in <see cref="EndianFormat"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EndianInt48 AsEndianInt48(this ArraySegment<byte> buffer, EndianFormat endianFormat) =>
        new (buffer, endianFormat);

    /// <summary>
    /// Creates a <see cref="EndianInt48"/> from the given buffer that uses the <see cref="EndianFormat"/>
    /// for conversions.
    /// <example>
    /// <code>
    /// var buffer = new byte[] { 0xFF, 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
    /// var endianValue = buffer.AsEndianInt48(EndianFormat.Big, startIndex: 1);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="buffer">A buffer in the form of an array segment that is used as the source of the endian value</param>
    /// <param name="endianFormat">The endian format of the data used to determine how to interpret the bytes in the buffer</param>
    /// <param name="startIndex">The starting index within the given buffer to start using to make the endian value</param>
    /// <returns>The <see cref="EndianInt48"/> that reads the given buffer that is in <see cref="EndianFormat"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EndianInt48 AsEndianInt48(this byte[] buffer, EndianFormat endianFormat, int startIndex = 0) =>
        new (new ArraySegment<byte>(buffer, startIndex, EndianInt48.Size), endianFormat);

    /// <summary>
    /// Creates a <see cref="EndianInt48"/> from the given buffer that uses the <see cref="EndianFormat"/>
    /// for conversions.
    /// <example>
    /// <code>
    /// var buffer = new byte[] { 0xFF, 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0  };
    /// var endianValue = buffer.AsEndianInt48(EndianFormat.Big, startIndex: 1 );
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="buffer">A buffer in the form of an array segment that is used as the source of the endian value</param>
    /// <param name="endianFormat">The endian format of the data used to determine how to interpret the bytes in the buffer</param>
    /// <param name="startIndex">The starting index within the given buffer to start using to make the endian value</param>
    /// <returns>The <see cref="EndianInt48"/> that reads the given buffer that is in <see cref="EndianFormat"/> starting
    /// at the given <see cref="startIndex"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UInt64 AsInt48(this byte[] buffer, EndianFormat endianFormat, int startIndex = 0) =>
        AsEndianInt48(buffer, endianFormat, startIndex).Value;

    /// <summary>
    /// Copies the given <see cref="value"/> into the given buffer using the <see cref="EndianFormat"/>
    /// <example>
    /// <code>
    /// Int48 value = 0x123456789ABCDEF0;
    /// var buffer = new byte[sizeof(Int48)];
    /// value.CopyToBuffer(EndianFormat.Big, 0, buffer, 0, sizeof(Int48));
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="value">The value to copy to the buffer</param>
    /// <param name="endianFormat">The endian format to use when copying to the buffer</param>
    /// <param name="sourceStartIndex">The starting source index this can be 0 to the size of the <see cref="value"/></param>
    /// <param name="destinationArray">The array to copy the value into after taking into account the <see cref="EndianFormat"/></param>
    /// <param name="destinationStartIndex">Index in the destination buffer were the copy should be started</param>
    /// <param name="count">From 0 to the size of the <see cref="Value"/> but can't exceed the source buffer size while also taking
    /// into account the <see cref="sourceStartIndex"/></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyToBufferInt48(this UInt64 value, EndianFormat endianFormat, int sourceStartIndex, byte[] destinationArray, int destinationStartIndex, int count) =>
        AsEndianInt48(value, endianFormat).CopyTo(sourceStartIndex, destinationArray, destinationStartIndex, count);

    /// <summary>
    /// Copies the given <see cref="value"/> into the given buffer using the <see cref="EndianFormat"/>
    /// <example>
    /// <code>
    /// Int48 value = 0x123456789ABCDEF0;
    /// var buffer = new byte[sizeof(Int48)];
    /// value.CopyToBuffer(EndianFormat.Big, buffer);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="value">The value to copy to the buffer</param>
    /// <param name="endianFormat">The endian format to use when copying to the buffer</param>
    /// <param name="destinationArray">The array to copy the value into after taking into account the <see cref="EndianFormat"/></param>
    /// <param name="destinationStartIndex">Index in the destination buffer were the copy should be started</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyToBufferInt48(this UInt64 value, EndianFormat endianFormat, byte[] destinationArray, int destinationStartIndex = 0) =>
        AsEndianInt48(value, endianFormat).CopyTo(destinationArray, destinationStartIndex);
}