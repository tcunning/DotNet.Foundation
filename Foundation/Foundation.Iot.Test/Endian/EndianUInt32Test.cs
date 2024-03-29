﻿using Foundation.Iot.Collection;
using Foundation.Iot.Endian;

namespace Foundation.Iot.Test.Endian;

[TestClass]
[ExcludeFromCodeCoverage]
public class EndianUInt32Test
{
    [DataTestMethod]
    [DataRow((UInt32)0x12345678, EndianFormat.Big, new byte[] { 0x12, 0x34, 0x56, 0x78})]
    [DataRow((UInt32)0x12345678, EndianFormat.Little, new byte[] { 0x78, 0x56, 0x34, 0x12 })]
    public void FromValueTest(UInt32 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = new EndianUInt32(value, endianFormat);

        endianValue.Count.ShouldBe(EndianUInt32.Size);

        endianValue.Value.ShouldBe(value);
        endianValue[0].ShouldBe(valueBuffer[0]);
        endianValue[1].ShouldBe(valueBuffer[1]);
        endianValue[2].ShouldBe(valueBuffer[2]);
        endianValue[3].ShouldBe(valueBuffer[3]);

        var index = 0;
        foreach (var foundValue in endianValue)
            foundValue.ShouldBe(valueBuffer[index++]);
        
        endianValue.ToArray().ShouldBe(valueBuffer);
        endianValue.ToList().ShouldBe(valueBuffer);
    }

    [DataTestMethod]
    [DataRow((UInt32)0x12345678, EndianFormat.Big, new byte[] { 0x12, 0x34, 0x56, 0x78 })]
    [DataRow((UInt32)0x12345678, EndianFormat.Little, new byte[] { 0x78, 0x56, 0x34, 0x12 })]
    public void FromBufferTest(UInt32 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = new EndianUInt32(valueBuffer, endianFormat);

        endianValue.Count.ShouldBe(EndianUInt32.Size);

        endianValue.Value.ShouldBe(value);
        endianValue[0].ShouldBe(valueBuffer[0]);
        endianValue[1].ShouldBe(valueBuffer[1]);
        endianValue[2].ShouldBe(valueBuffer[2]);
        endianValue[3].ShouldBe(valueBuffer[3]);

        var index = 0;
        foreach (var foundValue in endianValue)
            foundValue.ShouldBe(valueBuffer[index++]);

        IEnumerator test = endianValue.GetEnumerator();
        test.MoveNext();
        test.Current.ShouldBe(valueBuffer[0]);
        test.Reset();
        test.MoveNext();
        test.Current.ShouldBe(valueBuffer[0]);
        
        endianValue.ToArray().ShouldBe(valueBuffer);
        endianValue.ToList().ShouldBe(valueBuffer);
    }

    [DataTestMethod]
    [DataRow((UInt32)0x12345678, EndianFormat.Big)]
    [DataRow((UInt32)0x12345678, EndianFormat.Little)]
    public void CopyToBufferSimpleTest(UInt32 value, EndianFormat endianFormat)
    {
        var endianValue = new EndianUInt32(value, endianFormat);
        var intoBuffer = new byte[20];

        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(intoBuffer, 0);
        intoBuffer[0].ShouldBe(endianValue[0]);
        intoBuffer[1].ShouldBe(endianValue[1]);
        intoBuffer[2].ShouldBe(endianValue[2]);
        intoBuffer[3].ShouldBe(endianValue[3]);
        intoBuffer[4].ShouldBe((byte)0xEE);

        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(intoBuffer, 1);
        intoBuffer[0].ShouldBe((byte)0xEE);
        intoBuffer[1].ShouldBe(endianValue[0]);
        intoBuffer[2].ShouldBe(endianValue[1]);
        intoBuffer[3].ShouldBe(endianValue[2]);
        intoBuffer[4].ShouldBe(endianValue[3]);
        intoBuffer[5].ShouldBe((byte)0xEE);
    }

    [DataTestMethod]
    [DataRow((UInt32)0x12345678, EndianFormat.Big)]
    [DataRow((UInt32)0x12345678, EndianFormat.Little)]
    public void CopyToBufferAdvancedTest(UInt32 value, EndianFormat endianFormat)
    {
        var endianValue = new EndianUInt32(value, endianFormat);
        var intoBuffer = new byte[20];

        // Fill from start of both buffers
        //
        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(0,intoBuffer, 0, EndianUInt32.Size);
        intoBuffer[0].ShouldBe(endianValue[0]);
        intoBuffer[1].ShouldBe(endianValue[1]);
        intoBuffer[2].ShouldBe(endianValue[2]);
        intoBuffer[3].ShouldBe(endianValue[3]);
        intoBuffer[4].ShouldBe((byte)0xEE);
        
        // Fill from start of source buffer but 1 into destination buffer
        //
        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(0, intoBuffer, 1, EndianUInt32.Size);
        intoBuffer[0].ShouldBe((byte)0xEE);
        intoBuffer[1].ShouldBe(endianValue[0]);
        intoBuffer[2].ShouldBe(endianValue[1]);
        intoBuffer[3].ShouldBe(endianValue[2]);
        intoBuffer[4].ShouldBe(endianValue[3]);
        intoBuffer[5].ShouldBe((byte)0xEE);

        // Fill from start of source buffer, but right at end of destination buffer
        //
        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(0, intoBuffer, intoBuffer.Length-EndianUInt32.Size, EndianUInt32.Size);
        intoBuffer[15].ShouldBe((byte)0xEE);
        intoBuffer[16].ShouldBe(endianValue[0]);
        intoBuffer[17].ShouldBe(endianValue[1]);
        intoBuffer[18].ShouldBe(endianValue[2]);
        intoBuffer[19].ShouldBe(endianValue[3]);

        // Fill from 1 into the source
        //
        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(1, intoBuffer, 0, EndianUInt32.Size-1);
        intoBuffer[0].ShouldBe(endianValue[1]);
        intoBuffer[1].ShouldBe(endianValue[2]);
        intoBuffer[2].ShouldBe(endianValue[3]);
        intoBuffer[3].ShouldBe((byte)0xEE);
    }

    [DataTestMethod]
    [DataRow((UInt32)0x12345678, EndianFormat.Big)]
    [DataRow((UInt32)0x12345678, EndianFormat.Little)]
    public void CopyToErrorsTest(UInt32 value, EndianFormat endianFormat)
    {
        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt32(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(-1, myBuffer, 1, EndianUInt32.Size);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt32(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(0, myBuffer, -1, EndianUInt32.Size);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt32(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(1, myBuffer, 0, EndianUInt32.Size);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt32(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(0, myBuffer, 17, EndianUInt32.Size);
        });
    }

    [DataTestMethod]
    [DataRow(EndianUInt32.Size + 1, EndianFormat.Big)]
    [DataRow(EndianUInt32.Size + 1, EndianFormat.Little)]
    [DataRow(EndianUInt32.Size - 1, EndianFormat.Big)]
    [DataRow(EndianUInt32.Size - 1, EndianFormat.Little)]
    [DataRow(0, EndianFormat.Big)]
    [DataRow(0, EndianFormat.Little)]
    [DataRow(1, EndianFormat.Big)]
    [DataRow(1, EndianFormat.Little)]
    public void ByteConstructionErrorsTest(int size, EndianFormat endianFormat)
    {
        Should.Throw<ArgumentOutOfRangeException>(() => {
            _ = new EndianUInt32(new byte[size], endianFormat);
        });
    }

    [DataTestMethod]
    [DataRow((UInt32) 0x12345678, EndianFormat.Big)]
    [DataRow((UInt32) 0x12345678, EndianFormat.Little)]
    public void IndexErrorsTest(UInt32 value, EndianFormat endianFormat)
    {
        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt32(value, endianFormat);
            _ = myEndianValue[EndianUInt32.Size];
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt32(value, endianFormat);
            _ = myEndianValue[-1];
        });
    }

    [DataTestMethod]
    [DataRow((UInt32)0x12345678, EndianFormat.Big, new byte[] { 0x12, 0x34, 0x56, 0x78 })]
    [DataRow((UInt32)0x12345678, EndianFormat.Little, new byte[] { 0x78, 0x56, 0x34, 0x12 })]
    public void FromExtensionTest(UInt32 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = value.AsEndianUInt32(endianFormat);
        endianValue.Count.ShouldBe(EndianUInt32.Size);
        endianValue.Value.ShouldBe(value);
        endianValue[0].ShouldBe(valueBuffer[0]);
        endianValue[1].ShouldBe(valueBuffer[1]);
        endianValue[2].ShouldBe(valueBuffer[2]);
        endianValue[3].ShouldBe(valueBuffer[3]);

        var endianBuffer = valueBuffer.AsEndianUInt32(endianFormat);
        endianBuffer[0].ShouldBe(valueBuffer[0]);
        endianBuffer[1].ShouldBe(valueBuffer[1]);
        endianBuffer[2].ShouldBe(valueBuffer[2]);
        endianBuffer[3].ShouldBe(valueBuffer[3]);

        endianBuffer = (new ArraySegment<byte>(valueBuffer, 0, 4)).AsEndianUInt32(endianFormat);
        endianBuffer[0].ShouldBe(valueBuffer[0]);
        endianBuffer[1].ShouldBe(valueBuffer[1]);
        endianBuffer[2].ShouldBe(valueBuffer[2]);
        endianBuffer[3].ShouldBe(valueBuffer[3]);

        valueBuffer.AsUInt32(endianFormat).ShouldBe(value);

        var buffer = new byte[20];
        Array.Fill(buffer, (byte)0xEE);
        value.CopyToBuffer(endianFormat, 0, buffer, 1, EndianUInt32.Size);
        buffer[0].ShouldBe((byte)0xEE);
        buffer[1].ShouldBe(valueBuffer[0]);
        buffer[2].ShouldBe(valueBuffer[1]);
        buffer[3].ShouldBe(valueBuffer[2]);
        buffer[4].ShouldBe(valueBuffer[3]);
        buffer[5].ShouldBe((byte)0xEE);

        Array.Fill(buffer, (byte)0xEE);
        value.CopyToBuffer(endianFormat, buffer, 1);
        buffer[0].ShouldBe((byte)0xEE);
        buffer[1].ShouldBe(valueBuffer[0]);
        buffer[2].ShouldBe(valueBuffer[1]);
        buffer[3].ShouldBe(valueBuffer[2]);
        buffer[4].ShouldBe(valueBuffer[3]);
        buffer[5].ShouldBe((byte)0xEE);
    }

    [TestMethod]
    public void EnumeratorBuilderTest()
    {
        Should.Throw<InvalidOperationException>(() => {
            var builder = new EnumeratorForFour<byte>();
            builder.CurrentValueForIndex(EndianUInt32.Size);
        });
    }
}
