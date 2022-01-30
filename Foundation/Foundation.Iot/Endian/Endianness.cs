namespace Foundation.Iot.Endian;

/// <summary>
/// Endianness is the ordering or sequencing of bytes that represent numeric values.  It is expressed as big endian or little endian.
/// A big-endian system stores the most significant byte at the smallest memory address and the least significant byte at the largest.
/// A little-endian system, in contrast, stores the least-significant byte at the smallest address.
/// </summary>
public enum EndianFormat
{
    /// <summary>
    /// A big-endian system stores the most significant byte at the smallest memory address and the least significant byte at the largest.
    /// <code>
    /// Take the number 0x12345678
    ///
    ///   Pointer/  Big   
    ///   Address   Endian
    ///   ================
    ///     a:      0x12  
    ///     a+1:    0x34  
    ///     a+2:    0x56  
    ///     a+3:    0x78  
    /// </code>
    /// </summary>
    Big,

    /// <summary>
    /// A little-endian system stores the least-significant byte at the smallest address and the most significant byte at the largest.
    /// <code>
    /// Take the number 0x12345678
    ///
    ///   Pointer/  Little
    ///   Address   Endian
    ///   =================
    ///     a:       0x78
    ///     a+1:     0x56
    ///     a+2:     0x34
    ///     a+3:     0x12
    /// </code>
    /// </summary>
    Little
}
