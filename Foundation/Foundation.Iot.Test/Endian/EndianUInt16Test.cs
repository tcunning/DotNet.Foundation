using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Foundation.Iot.Collection;
using Foundation.Iot.Endian;

namespace Foundation.Iot.Test.Endian;

[TestClass]
[ExcludeFromCodeCoverage]
public class EndianUInt16Test
{
    [DataTestMethod]
    [DataRow((UInt16)0x1234, EndianFormat.Big, new byte[] { 0x12, 0x34 })]
    [DataRow((UInt16)0x1234, EndianFormat.Little, new byte[] { 0x34, 0x12 })]
    public void FromValueTest(UInt16 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = new EndianUInt16(value, endianFormat);

        endianValue.Count.ShouldBe(sizeof(UInt16));

        endianValue.Value.ShouldBe(value);
        endianValue[0].ShouldBe(valueBuffer[0]);
        endianValue[1].ShouldBe(valueBuffer[1]);

        var index = 0;
        foreach (var foundValue in endianValue)
            foundValue.ShouldBe(valueBuffer[index++]);
        
        endianValue.ToArray().ShouldBe(valueBuffer);
        endianValue.ToList().ShouldBe(valueBuffer);
    }

    [DataTestMethod]
    [DataRow((UInt16)0x1234, EndianFormat.Big, new byte[] { 0x12, 0x34 })]
    [DataRow((UInt16)0x1234, EndianFormat.Little, new byte[] { 0x34, 0x12 })]
    public void FromBufferTest(UInt16 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = new EndianUInt16(valueBuffer, endianFormat);

        endianValue.Count.ShouldBe(sizeof(UInt16));

        endianValue.Value.ShouldBe(value);
        endianValue[0].ShouldBe(valueBuffer[0]);
        endianValue[1].ShouldBe(valueBuffer[1]);

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
    [DataRow((UInt16)0x1234, EndianFormat.Big)]
    [DataRow((UInt16)0x1234, EndianFormat.Little)]
    public void CopyToBufferSimpleTest(UInt16 value, EndianFormat endianFormat)
    {
        var endianValue = new EndianUInt16(value, endianFormat);
        var buffer = endianValue.ToArray();

        var intoBuffer = new byte[20];

        Array.Fill(intoBuffer, (byte)0xFF);
        endianValue.CopyTo(intoBuffer, 0);
        intoBuffer[0].ShouldBe(endianValue[0]);
        intoBuffer[1].ShouldBe(endianValue[1]);
        intoBuffer[2].ShouldBe((byte)0xFF);

        Array.Fill(intoBuffer, (byte)0xFF);
        endianValue.CopyTo(intoBuffer, 1);
        intoBuffer[0].ShouldBe((byte)0xFF);
        intoBuffer[1].ShouldBe(endianValue[0]);
        intoBuffer[2].ShouldBe(endianValue[1]);
        intoBuffer[3].ShouldBe((byte)0xFF);
    }

    [DataTestMethod]
    [DataRow((UInt16)0x1234, EndianFormat.Big)]
    [DataRow((UInt16)0x1234, EndianFormat.Little)]
    public void CopyToBufferAdvancedTest(UInt16 value, EndianFormat endianFormat)
    {
        var endianValue = new EndianUInt16(value, endianFormat);
        var intoBuffer = new byte[20];

        // Fill from start of both buffers
        //
        Array.Fill(intoBuffer, (byte)0xFF);
        endianValue.CopyTo(0,intoBuffer, 0, 2);
        intoBuffer[0].ShouldBe(endianValue[0]);
        intoBuffer[1].ShouldBe(endianValue[1]);
        intoBuffer[2].ShouldBe((byte)0xFF);
        
        // Fill from start of source buffer but 1 into destination buffer
        //
        Array.Fill(intoBuffer, (byte)0xFF);
        endianValue.CopyTo(0, intoBuffer, 1, 2);
        intoBuffer[0].ShouldBe((byte)0xFF);
        intoBuffer[1].ShouldBe(endianValue[0]);
        intoBuffer[2].ShouldBe(endianValue[1]);
        intoBuffer[3].ShouldBe((byte)0xFF);

        // Fill from start of source buffer, but right at end of destination buffer
        //
        Array.Fill(intoBuffer, (byte)0xFF);
        endianValue.CopyTo(0, intoBuffer, 18, 2);
        intoBuffer[17].ShouldBe((byte)0xFF);
        intoBuffer[18].ShouldBe(endianValue[0]);
        intoBuffer[19].ShouldBe(endianValue[1]);

        // Fill from 1 into the source
        //
        Array.Fill(intoBuffer, (byte)0xFF);
        endianValue.CopyTo(1, intoBuffer, 0, 1);
        intoBuffer[0].ShouldBe(endianValue[1]);
        intoBuffer[1].ShouldBe((byte)0xFF);
    }

    [DataTestMethod]
    [DataRow((UInt16)0x1234, EndianFormat.Big)]
    [DataRow((UInt16)0x1234, EndianFormat.Little)]
    public void CopyToErrorsTest(UInt16 value, EndianFormat endianFormat)
    {
        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt16(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(-1, myBuffer, 1, 2);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt16(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(0, myBuffer, -1, 2);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt16(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(1, myBuffer, 0, 2);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt16(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(0, myBuffer, 19, 2);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt16(new byte[3], endianFormat);
        });
    }

    [DataTestMethod]
    [DataRow((UInt16)0x1234, EndianFormat.Big, new byte[] { 0x12, 0x34 })]
    [DataRow((UInt16)0x1234, EndianFormat.Little, new byte[] { 0x34, 0x12 })]
    public void FromValueExtensionTest(UInt16 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = value.AsEndianUInt16(endianFormat);
        endianValue.Count.ShouldBe(sizeof(UInt16));
        endianValue.Value.ShouldBe(value);
        endianValue[0].ShouldBe(valueBuffer[0]);
        endianValue[1].ShouldBe(valueBuffer[1]);

        var endianBuffer = valueBuffer.AsEndianUInt16(endianFormat);
        endianBuffer[0].ShouldBe(valueBuffer[0]);
        endianBuffer[1].ShouldBe(valueBuffer[1]);

        endianBuffer = (new ArraySegment<byte>(valueBuffer, 0, 2)).AsEndianUInt16(endianFormat);
        endianBuffer[0].ShouldBe(valueBuffer[0]);
        endianBuffer[1].ShouldBe(valueBuffer[1]);

        valueBuffer.AsUInt16(endianFormat).ShouldBe(value);

        var buffer = new byte[20];
        Array.Fill(buffer, (byte)0xFF);
        value.CopyToBuffer(endianFormat, 0, buffer, 1, sizeof(UInt16));
        buffer[1].ShouldBe(valueBuffer[0]);
        buffer[2].ShouldBe(valueBuffer[1]);

        Array.Fill(buffer, (byte)0xFF);
        value.CopyToBuffer(endianFormat, buffer, 1);
        buffer[1].ShouldBe(valueBuffer[0]);
        buffer[2].ShouldBe(valueBuffer[1]);
    }

    [TestMethod]
    public void EnumeratorBuilderTest()
    {
        Should.Throw<InvalidOperationException>(() => {
            var builder = new EnumeratorBuilderForTwo<UInt16>();
            builder.CurrentValueForIndex(sizeof(UInt16));
        });
    }

}
