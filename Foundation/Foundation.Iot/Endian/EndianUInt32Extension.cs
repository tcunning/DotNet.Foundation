using System.Runtime.CompilerServices;

namespace Foundation.Iot.Endian;

public static class EndianUInt32Extension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EndianUInt32 AsEndianUInt32(this UInt32 value, EndianFormat endianFormat) => new EndianUInt32(value, endianFormat);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EndianUInt32 AsEndianUInt32(this byte[] buffer, EndianFormat endianFormat) => new EndianUInt32(buffer, endianFormat);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UInt32 AsUInt32(this byte[] buffer, int startIndex, EndianFormat endianFormat) => (new ArraySegment<byte>(buffer, startIndex, sizeof(UInt32))).AsEndianUInt32(endianFormat).Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UInt32 AsUInt32(this byte[] buffer, EndianFormat endianFormat) => AsUInt32(buffer, 0, endianFormat);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EndianUInt32 AsEndianUInt32(this ArraySegment<byte> buffer, EndianFormat endianFormat) => new EndianUInt32(buffer, endianFormat);

}