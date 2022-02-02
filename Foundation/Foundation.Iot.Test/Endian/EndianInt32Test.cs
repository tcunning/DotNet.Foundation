using Foundation.Iot.Collection;
using Foundation.Iot.Endian;

namespace Foundation.Iot.Test.Endian;

[TestClass]
[ExcludeFromCodeCoverage]
public class EndianInt32Test
{
    [DataTestMethod]
    [DataRow((Int32)0x12345678, EndianFormat.Big, new byte[] { 0x12, 0x34, 0x56, 0x78})]
    [DataRow((Int32)0x12345678, EndianFormat.Little, new byte[] { 0x78, 0x56, 0x34, 0x12 })]
    [DataRow((Int32)(-1), EndianFormat.Little, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF })]
    [DataRow((Int32)(-2), EndianFormat.Little, new byte[] { 0xFE, 0xFF, 0xFF, 0xFF })]
    [DataRow((Int32)(-2), EndianFormat.Big, new byte[] { 0xFF, 0xFF, 0xFF, 0xFE })]
    public void FromValueTest(Int32 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = new EndianInt32(value, endianFormat);

        endianValue.Count.ShouldBe(sizeof(Int32));

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
    [DataRow((Int32)0x12345678, EndianFormat.Big, new byte[] { 0x12, 0x34, 0x56, 0x78 })]
    [DataRow((Int32)0x12345678, EndianFormat.Little, new byte[] { 0x78, 0x56, 0x34, 0x12 })]
    [DataRow((Int32)(-1), EndianFormat.Little, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF })]
    [DataRow((Int32)(-2), EndianFormat.Little, new byte[] { 0xFE, 0xFF, 0xFF, 0xFF })]
    [DataRow((Int32)(-2), EndianFormat.Big, new byte[] { 0xFF, 0xFF, 0xFF, 0xFE })]
    public void FromBufferTest(Int32 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = new EndianInt32(valueBuffer, endianFormat);

        endianValue.Count.ShouldBe(sizeof(Int32));

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
    [DataRow((Int32)0x12345678, EndianFormat.Big)]
    [DataRow((Int32)0x12345678, EndianFormat.Little)]
    public void CopyToBufferSimpleTest(Int32 value, EndianFormat endianFormat)
    {
        var endianValue = new EndianInt32(value, endianFormat);
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
    [DataRow((Int32)0x12345678, EndianFormat.Big)]
    [DataRow((Int32)0x12345678, EndianFormat.Little)]
    public void CopyToBufferAdvancedTest(Int32 value, EndianFormat endianFormat)
    {
        var endianValue = new EndianInt32(value, endianFormat);
        var intoBuffer = new byte[20];

        // Fill from start of both buffers
        //
        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(0,intoBuffer, 0, 4);
        intoBuffer[0].ShouldBe(endianValue[0]);
        intoBuffer[1].ShouldBe(endianValue[1]);
        intoBuffer[2].ShouldBe(endianValue[2]);
        intoBuffer[3].ShouldBe(endianValue[3]);
        intoBuffer[4].ShouldBe((byte)0xEE);
        
        // Fill from start of source buffer but 1 into destination buffer
        //
        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(0, intoBuffer, 1, 4);
        intoBuffer[0].ShouldBe((byte)0xEE);
        intoBuffer[1].ShouldBe(endianValue[0]);
        intoBuffer[2].ShouldBe(endianValue[1]);
        intoBuffer[3].ShouldBe(endianValue[2]);
        intoBuffer[4].ShouldBe(endianValue[3]);
        intoBuffer[5].ShouldBe((byte)0xEE);

        // Fill from start of source buffer, but right at end of destination buffer
        //
        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(0, intoBuffer, 16, 4);
        intoBuffer[15].ShouldBe((byte)0xEE);
        intoBuffer[16].ShouldBe(endianValue[0]);
        intoBuffer[17].ShouldBe(endianValue[1]);
        intoBuffer[18].ShouldBe(endianValue[2]);
        intoBuffer[19].ShouldBe(endianValue[3]);

        // Fill from 1 into the source
        //
        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(1, intoBuffer, 0, 3);
        intoBuffer[0].ShouldBe(endianValue[1]);
        intoBuffer[1].ShouldBe(endianValue[2]);
        intoBuffer[2].ShouldBe(endianValue[3]);
        intoBuffer[3].ShouldBe((byte)0xEE);
    }

    [DataTestMethod]
    [DataRow((Int32)0x12345678, EndianFormat.Big)]
    [DataRow((Int32)0x12345678, EndianFormat.Little)]
    public void CopyToErrorsTest(Int32 value, EndianFormat endianFormat)
    {
        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianInt32(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(-1, myBuffer, 1, 4);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianInt32(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(0, myBuffer, -1, 4);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianInt32(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(1, myBuffer, 0, 4);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianInt32(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(0, myBuffer, 17, 4);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianInt32(new byte[3], endianFormat);
        });
    }

    [DataTestMethod]
    [DataRow((Int32)0x12345678, EndianFormat.Big, new byte[] { 0x12, 0x34, 0x56, 0x78 })]
    [DataRow((Int32)0x12345678, EndianFormat.Little, new byte[] { 0x78, 0x56, 0x34, 0x12 })]
    [DataRow((Int32)(-1), EndianFormat.Little, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF })]
    [DataRow((Int32)(-2), EndianFormat.Little, new byte[] { 0xFE, 0xFF, 0xFF, 0xFF })]
    [DataRow((Int32)(-2), EndianFormat.Big, new byte[] { 0xFF, 0xFF, 0xFF, 0xFE })]
    public void FromExtensionTest(Int32 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = value.AsEndianInt32(endianFormat);
        endianValue.Count.ShouldBe(sizeof(Int32));
        endianValue.Value.ShouldBe(value);
        endianValue[0].ShouldBe(valueBuffer[0]);
        endianValue[1].ShouldBe(valueBuffer[1]);
        endianValue[2].ShouldBe(valueBuffer[2]);
        endianValue[3].ShouldBe(valueBuffer[3]);

        var endianBuffer = valueBuffer.AsEndianInt32(endianFormat);
        endianBuffer[0].ShouldBe(valueBuffer[0]);
        endianBuffer[1].ShouldBe(valueBuffer[1]);
        endianBuffer[2].ShouldBe(valueBuffer[2]);
        endianBuffer[3].ShouldBe(valueBuffer[3]);

        endianBuffer = (new ArraySegment<byte>(valueBuffer, 0, 4)).AsEndianInt32(endianFormat);
        endianBuffer[0].ShouldBe(valueBuffer[0]);
        endianBuffer[1].ShouldBe(valueBuffer[1]);
        endianBuffer[2].ShouldBe(valueBuffer[2]);
        endianBuffer[3].ShouldBe(valueBuffer[3]);

        valueBuffer.AsInt32(endianFormat).ShouldBe(value);

        var buffer = new byte[20];
        Array.Fill(buffer, (byte)0xEE);
        value.CopyToBuffer(endianFormat, 0, buffer, 1, sizeof(Int32));
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
            builder.CurrentValueForIndex(sizeof(Int32));
        });
    }
}
