using System.Runtime.CompilerServices;

namespace Foundation.Iot.Endian;

public static class EndianUInt16Extension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EndianUInt16 AsEndianUInt16(this UInt16 value, EndianFormat endianFormat) => new EndianUInt16(value, endianFormat);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EndianUInt16 AsEndianUInt16(this byte[] buffer, EndianFormat endianFormat) => new EndianUInt16(buffer, endianFormat);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UInt16 AsUInt16(this byte[] buffer, int startIndex, EndianFormat endianFormat) => (new ArraySegment<byte>(buffer, startIndex, sizeof(UInt16))).AsEndianUInt16(endianFormat).Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UInt16 AsUInt16(this byte[] buffer, EndianFormat endianFormat) => AsUInt16(buffer, 0, endianFormat);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EndianUInt16 AsEndianUInt16(this ArraySegment<byte> buffer, EndianFormat endianFormat) => new EndianUInt16(buffer, endianFormat);
}