using Foundation.Iot.Collection;
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

        endianValue.Count.ShouldBe(sizeof(UInt32));

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

        endianValue.Count.ShouldBe(sizeof(UInt32));

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
        var buffer = endianValue.ToArray();

        var intoBuffer = new byte[20];

        Array.Fill(intoBuffer, (byte)0xFF);
        endianValue.CopyTo(intoBuffer, 0);
        intoBuffer[0].ShouldBe(endianValue[0]);
        intoBuffer[1].ShouldBe(endianValue[1]);
        intoBuffer[2].ShouldBe(endianValue[2]);
        intoBuffer[3].ShouldBe(endianValue[3]);
        intoBuffer[4].ShouldBe((byte)0xFF);

        Array.Fill(intoBuffer, (byte)0xFF);
        endianValue.CopyTo(intoBuffer, 1);
        intoBuffer[0].ShouldBe((byte)0xFF);
        intoBuffer[1].ShouldBe(endianValue[0]);
        intoBuffer[2].ShouldBe(endianValue[1]);
        intoBuffer[3].ShouldBe(endianValue[2]);
        intoBuffer[4].ShouldBe(endianValue[3]);
        intoBuffer[5].ShouldBe((byte)0xFF);
    }

    [DataTestMethod]
    [DataRow((UInt32)0x12345678, EndianFormat.Big)]
    [DataRow((UInt32)0x12345678, EndianFormat.Little)]
    public void CopyToBufferAdvancedTest(UInt32 value, EndianFormat endianFormat)
    {
        var endianValue = new EndianUInt32(value, endianFormat);
        var buffer = endianValue.ToArray();

        var intoBuffer = new byte[20];

        // Fill from start of both buffers
        //
        Array.Fill(intoBuffer, (byte)0xFF);
        endianValue.CopyTo(0,intoBuffer, 0, 4);
        intoBuffer[0].ShouldBe(endianValue[0]);
        intoBuffer[1].ShouldBe(endianValue[1]);
        intoBuffer[2].ShouldBe(endianValue[2]);
        intoBuffer[3].ShouldBe(endianValue[3]);
        intoBuffer[4].ShouldBe((byte)0xFF);
        
        // Fill from start of source buffer but 1 into destination buffer
        //
        Array.Fill(intoBuffer, (byte)0xFF);
        endianValue.CopyTo(0, intoBuffer, 1, 4);
        intoBuffer[0].ShouldBe((byte)0xFF);
        intoBuffer[1].ShouldBe(endianValue[0]);
        intoBuffer[2].ShouldBe(endianValue[1]);
        intoBuffer[3].ShouldBe(endianValue[2]);
        intoBuffer[4].ShouldBe(endianValue[3]);
        intoBuffer[5].ShouldBe((byte)0xFF);

        // Fill from start of source buffer, but right at end of destination buffer
        //
        Array.Fill(intoBuffer, (byte)0xFF);
        endianValue.CopyTo(0, intoBuffer, 16, 4);
        intoBuffer[15].ShouldBe((byte)0xFF);
        intoBuffer[16].ShouldBe(endianValue[0]);
        intoBuffer[17].ShouldBe(endianValue[1]);
        intoBuffer[18].ShouldBe(endianValue[2]);
        intoBuffer[19].ShouldBe(endianValue[3]);

        // Fill from 1 into the source
        //
        Array.Fill(intoBuffer, (byte)0xFF);
        endianValue.CopyTo(1, intoBuffer, 0, 3);
        intoBuffer[0].ShouldBe(endianValue[1]);
        intoBuffer[1].ShouldBe(endianValue[2]);
        intoBuffer[2].ShouldBe(endianValue[3]);
        intoBuffer[3].ShouldBe((byte)0xFF);
    }

    [DataTestMethod]
    [DataRow((UInt32)0x12345678, EndianFormat.Big)]
    [DataRow((UInt32)0x12345678, EndianFormat.Little)]
    public void CopyToErrorsTest(UInt32 value, EndianFormat endianFormat)
    {
        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt32(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(-1, myBuffer, 1, 4);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt32(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(0, myBuffer, -1, 4);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt32(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(1, myBuffer, 0, 4);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt32(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(0, myBuffer, 17, 4);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt32(new byte[3], endianFormat);
        });
    }

    [DataTestMethod]
    [DataRow((UInt32)0x12345678, EndianFormat.Big, new byte[] { 0x12, 0x34, 0x56, 0x78 })]
    [DataRow((UInt32)0x12345678, EndianFormat.Little, new byte[] { 0x78, 0x56, 0x34, 0x12 })]
    public void FromExtensionTest(UInt32 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = value.AsEndianUInt32(endianFormat);
        endianValue.Count.ShouldBe(sizeof(UInt32));
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
        Array.Fill(buffer, (byte)0xFF);
        value.CopyToBuffer(endianFormat, 0, buffer, 1, sizeof(UInt32));
        buffer[1].ShouldBe(valueBuffer[0]);
        buffer[2].ShouldBe(valueBuffer[1]);
        buffer[3].ShouldBe(valueBuffer[2]);
        buffer[4].ShouldBe(valueBuffer[3]);

        Array.Fill(buffer, (byte)0xFF);
        value.CopyToBuffer(endianFormat, buffer, 1);
        buffer[1].ShouldBe(valueBuffer[0]);
        buffer[2].ShouldBe(valueBuffer[1]);
        buffer[3].ShouldBe(valueBuffer[2]);
        buffer[4].ShouldBe(valueBuffer[3]);
    }

    [TestMethod]
    public void EnumeratorBuilderTest()
    {
        Should.Throw<InvalidOperationException>(() => {
            var builder = new EnumeratorBuilderForFour<UInt32>();
            builder.CurrentValueForIndex(sizeof(UInt32));
        });
    }
}
