﻿using Foundation.Iot.Collection;
using System;
using System.Numerics;
using System.Text;

namespace Foundation.Iot.Endian;

public readonly ref struct Endian<TValue>
    where TValue : IBinaryInteger<TValue>
{
    /// <summary>
    /// Gets the <see cref="EndianFormat"/> being used to do the conversation.
    /// </summary>
    public EndianFormat EndianFormat { get; }

    /// <summary>
    /// Get's the value taking into account the <see cref="EndianFormat"/> when converting to/from a memory buffer.
    /// </summary>
    public TValue Value { get; }

    /// <summary>
    /// The size of the native value used to determine how many bytes are used <see cref="Value"/>.
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// <para>
    /// The data returned by this value takes into account the desired <see cref="EndianFormat"/> and it does this without
    /// allocating any heap memory.
    /// </para>
    /// <example>
    /// <see cref="Foundation.Iot.Endian.EndianFormat.Little"/> with value of 0x12345678
    /// and Given a Value of 0x1234:
    /// <para/>this[0] == 0x34
    /// <para/>this[1] == 0x12
    /// </example>
    /// </summary>
    /// <param name="index">The index of the byte to retrieve after taking into account the byte order via <see cref="EndianFormat"/></param>
    /// <returns>The byte at the given index after taking into account the byte order via <see cref="EndianFormat"/></returns>
    public byte this[int index]
    {
        get
        {
            if (index >= Count || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            Span<byte> buffer = stackalloc byte[Count];
            switch (EndianFormat)
            {
                case EndianFormat.Big:
                    Value.WriteBigEndian(buffer);
                    break;

                case EndianFormat.Little:
                    Value.WriteLittleEndian(buffer);
                    break;
             
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return buffer[index];
        }
    }

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
    public Endian(TValue value, EndianFormat endianFormat)
    {
        EndianFormat = endianFormat;
        Value = value;
        Count = value.GetByteCount();
    }

    /// <summary>
    /// <para>
    /// This constructor takes an ArraySegment which must contain exactly, no more and no less, the expected number of
    /// bytes <see cref="Count"/> needed to convert it to <see cref="Value"/>, and it does this without allocating any
    /// heap memory.
    /// </para>
    /// <para>
    /// This is a ref struct so it will never be on the heap and the lifetime of this struct is expected to be short,
    /// usually just enough time to finish whatever conversation is desired.
    /// </para>
    /// </summary>
    /// <param name="buffer">An ArraySegment which must contain exactly the expected number of bytes <see cref="Count"/>
    /// needed by the value.  Anything that can be converted into an ArraySegment can also be used such as a byte array, but
    /// it too must exactly match the expected numbers of bytes <see cref="Count"/>.</param>
    /// <param name="endianFormat">The endian format to use when converting from the buffer to the value</param>
    public Endian(ArraySegment<byte> buffer, EndianFormat endianFormat)
    {
        EndianFormat = endianFormat;
        Value = MakeValue(buffer, endianFormat);
        Count = Value.GetByteCount();
    }

    /// <summary>
    /// <para>
    /// Takes an ArraySegment which must contain exactly, no more and no less, the expected number of bytes needed to convert it to
    /// the returned value, and it does this without allocating any heap memory.
    /// </para>
    /// </summary>
    /// <param name="buffer">An ArraySegment which must contain exactly the expected number of bytes need to convert it to the
    /// returned value.  Anything that can be converted into an ArraySegment can also be used such as a byte array, but
    /// it too must exactly match the expected numbers of bytes.</param>
    /// <param name="endianFormat">The endian format to use when converting from the buffer to the value</param>
    public static TValue MakeValue(ArraySegment<byte> buffer, EndianFormat endianFormat)
    {
        var count = TValue.Zero.GetByteCount();

        if (buffer.Count != count)
            throw new ArgumentOutOfRangeException(nameof(buffer), $"Unexpected buffer size");

        TValue value;
        switch (endianFormat)
        {
            case EndianFormat.Big:
                if (!TValue.TryReadBigEndian(buffer, isUnsigned: false, out value))
                    throw new ArgumentOutOfRangeException(nameof(endianFormat), endianFormat, null);
                break;

            case EndianFormat.Little:
                if (!TValue.TryReadLittleEndian(buffer, isUnsigned: false, out value))
                    throw new ArgumentOutOfRangeException(nameof(endianFormat), endianFormat, null);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(endianFormat), endianFormat, null);
        }

        return value;
    }

    /// <summary>
    /// Converts the <see cref="Value"/> to an array of bytes using the <see cref="EndianFormat"/>.  This will cause a heap allocation
    /// as a new array is created.
    /// </summary>
    /// <returns>The <see cref="Value"/> converted to an array of bytes in <see cref="EndianFormat"/></returns>
    public byte[] ToArray() => GetEnumerator().ToArray();

    /// <summary>
    /// Converts the <see cref="Value"/> to <see cref="List{T}"/> of bytes using the <see cref="EndianFormat"/>.  This will cause a heap allocation
    /// as a new list is created.
    /// </summary>
    /// <returns>The <see cref="Value"/> converted to a <see cref="List{T}"/> of bytes in <see cref="EndianFormat"/></returns>
    public List<byte> ToList() => GetEnumerator().ToList();

    /// <summary>
    /// Copies the <see cref="Value"/> to the given <see cref="destinationArray"/> using the <see cref="EndianFormat"/>, and it does
    /// this without allocating any heap memory. 
    /// </summary>
    /// <param name="sourceStartIndex">The source index this can be 0 to the size of the <see cref="Value"/></param>
    /// <param name="destinationArray">The array to copy the value into after taking into account the <see cref="EndianFormat"/></param>
    /// <param name="destinationStartIndex">Index in the destination buffer were the copy should be started</param>
    /// <param name="count">From 0 to the size of the <see cref="Value"/> but can't exceed the source buffer size while also taking
    /// into account the <see cref="sourceStartIndex"/></param>
    public void CopyTo(int sourceStartIndex, byte[] destinationArray, int destinationStartIndex, int count) =>
        GetEnumerator().CopyTo(sourceStartIndex, destinationArray, destinationStartIndex, count);

    /// <summary>
    /// Copies the <see cref="Value"/> to the given <see cref="destinationArray"/> using the <see cref="EndianFormat"/>, and it does
    /// this without allocating any heap memory. There must be enough room in the <see cref="destinationArray"/> to hold the entire
    /// value.
    /// </summary>
    /// <param name="destinationArray">The array to copy the value into after taking into account the <see cref="EndianFormat"/></param>
    /// <param name="destinationStartIndex">Index in the destination buffer were the copy should be started</param>
    public void CopyTo(byte[] destinationArray, int destinationStartIndex = 0) => 
        GetEnumerator().CopyTo(destinationArray, destinationStartIndex);

    /// <summary>
    /// Returns an enumerator that iterates through a collection without allocating additional heap.  We explicitly return a
    /// struct so we can avoid the boxing overhead of using an <see cref="IEnumerable{T}"/> interface.  The <see cref="GetEnumerator"/>
    /// is used by C#'s foreach statement via what is know as Duck Typing.  Even without an interface, C# will look for a method named
    /// <see cref="GetEnumerator"/> to perform the iterating.
    /// </summary>
    /// <returns>A struct that conforms to <see cref="IEnumerator{T}"/> that iterates over the data taking into account the <see cref="EndianFormat"/></returns>
    public EnumeratorForTwo<byte> GetEnumerator() => new(this[0], this[1]);

    public override string ToString()
    {
        var strBuilder = new StringBuilder($"{Value:X}");
        foreach (var byteValue in this)
        {
            strBuilder.Append($" ");
            strBuilder.Append($"{byteValue:X2}");
        }
        return strBuilder.ToString();
    }
}

