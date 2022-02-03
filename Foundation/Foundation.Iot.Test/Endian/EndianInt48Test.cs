using Foundation.Iot.Collection;
using Foundation.Iot.Endian;

namespace Foundation.Iot.Test.Endian;

[TestClass]
[ExcludeFromCodeCoverage]
public class EndianInt48Test
{
    [DataTestMethod]
    [DataRow((Int64)0x123456789ABC, EndianFormat.Big, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC})]
    [DataRow((Int64)0x123456789ABC, EndianFormat.Little, new byte[] { 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12 })]
    [DataRow((Int64)(-1), EndianFormat.Little, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF })]
    [DataRow((Int64)(-2), EndianFormat.Little, new byte[] { 0xFE, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF })]
    [DataRow((Int64)(-2), EndianFormat.Big, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFE })]
    public void FromValueTest(Int64 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = new EndianInt48(value, endianFormat);

        endianValue.Count.ShouldBe(EndianInt48.Size);

        endianValue.Value.ShouldBe(value);
        endianValue[0].ShouldBe(valueBuffer[0]);
        endianValue[1].ShouldBe(valueBuffer[1]);
        endianValue[2].ShouldBe(valueBuffer[2]);
        endianValue[3].ShouldBe(valueBuffer[3]);
        endianValue[4].ShouldBe(valueBuffer[4]);
        endianValue[5].ShouldBe(valueBuffer[5]);

        var index = 0;
        foreach (var foundValue in endianValue)
            foundValue.ShouldBe(valueBuffer[index++]);
        
        endianValue.ToArray().ShouldBe(valueBuffer);
        endianValue.ToList().ShouldBe(valueBuffer);
    }

    [DataTestMethod]
    [DataRow((Int64)0x123456789ABC, EndianFormat.Big, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC })]
    [DataRow((Int64)0x123456789ABC, EndianFormat.Little, new byte[] { 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12 })]
    [DataRow((Int64)(-1), EndianFormat.Little, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF })]
    [DataRow((Int64)(-2), EndianFormat.Little, new byte[] { 0xFE, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF })]
    [DataRow((Int64)(-2), EndianFormat.Big, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFE })]
    public void FromBufferTest(Int64 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = new EndianInt48(valueBuffer, endianFormat);

        endianValue.Count.ShouldBe(EndianInt48.Size);

        endianValue.Value.ShouldBe(value);
        endianValue[0].ShouldBe(valueBuffer[0]);
        endianValue[1].ShouldBe(valueBuffer[1]);
        endianValue[2].ShouldBe(valueBuffer[2]);
        endianValue[3].ShouldBe(valueBuffer[3]);
        endianValue[4].ShouldBe(valueBuffer[4]);
        endianValue[5].ShouldBe(valueBuffer[5]);

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
    [DataRow((Int64)0x123456789ABC, EndianFormat.Big)]
    [DataRow((Int64)0x123456789ABC, EndianFormat.Little)]
    public void CopyToBufferSimpleTest(Int64 value, EndianFormat endianFormat)
    {
        var endianValue = new EndianInt48(value, endianFormat);
        var intoBuffer = new byte[20];

        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(intoBuffer, 0);
        intoBuffer[0].ShouldBe(endianValue[0]);
        intoBuffer[1].ShouldBe(endianValue[1]);
        intoBuffer[2].ShouldBe(endianValue[2]);
        intoBuffer[3].ShouldBe(endianValue[3]);
        intoBuffer[4].ShouldBe(endianValue[4]);
        intoBuffer[5].ShouldBe(endianValue[5]);
        intoBuffer[6].ShouldBe((byte)0xEE);

        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(intoBuffer, 1);
        intoBuffer[0].ShouldBe((byte)0xEE);
        intoBuffer[1].ShouldBe(endianValue[0]);
        intoBuffer[2].ShouldBe(endianValue[1]);
        intoBuffer[3].ShouldBe(endianValue[2]);
        intoBuffer[4].ShouldBe(endianValue[3]);
        intoBuffer[5].ShouldBe(endianValue[4]);
        intoBuffer[6].ShouldBe(endianValue[5]);
        intoBuffer[7].ShouldBe((byte)0xEE);
    }

    [DataTestMethod]
    [DataRow((Int64)0x123456789ABC, EndianFormat.Big)]
    [DataRow((Int64)0x123456789ABC, EndianFormat.Little)]
    public void CopyToBufferAdvancedTest(Int64 value, EndianFormat endianFormat)
    {
        var endianValue = new EndianInt48(value, endianFormat);
        var intoBuffer = new byte[20];

        // Fill from start of both buffers
        //
        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(0,intoBuffer, 0, EndianInt48.Size);
        intoBuffer[0].ShouldBe(endianValue[0]);
        intoBuffer[1].ShouldBe(endianValue[1]);
        intoBuffer[2].ShouldBe(endianValue[2]);
        intoBuffer[3].ShouldBe(endianValue[3]);
        intoBuffer[4].ShouldBe(endianValue[4]);
        intoBuffer[5].ShouldBe(endianValue[5]);
        intoBuffer[6].ShouldBe((byte)0xEE);
        
        // Fill from start of source buffer but 1 into destination buffer
        //
        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(0, intoBuffer, 1, EndianInt48.Size);
        intoBuffer[0].ShouldBe((byte)0xEE);
        intoBuffer[1].ShouldBe(endianValue[0]);
        intoBuffer[2].ShouldBe(endianValue[1]);
        intoBuffer[3].ShouldBe(endianValue[2]);
        intoBuffer[4].ShouldBe(endianValue[3]);
        intoBuffer[5].ShouldBe(endianValue[4]);
        intoBuffer[6].ShouldBe(endianValue[5]);
        intoBuffer[7].ShouldBe((byte)0xEE);

        // Fill from start of source buffer, but right at end of destination buffer
        //
        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(0, intoBuffer, intoBuffer.Length- EndianInt48.Size, EndianInt48.Size);
        intoBuffer[13].ShouldBe((byte)0xEE);
        intoBuffer[14].ShouldBe(endianValue[0]);
        intoBuffer[15].ShouldBe(endianValue[1]);
        intoBuffer[16].ShouldBe(endianValue[2]);
        intoBuffer[17].ShouldBe(endianValue[3]);
        intoBuffer[18].ShouldBe(endianValue[4]);
        intoBuffer[19].ShouldBe(endianValue[5]);

        // Fill from 1 into the source
        //
        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(1, intoBuffer, 0, EndianInt48.Size - 2);
        intoBuffer[0].ShouldBe(endianValue[1]);
        intoBuffer[1].ShouldBe(endianValue[2]);
        intoBuffer[2].ShouldBe(endianValue[3]);
        intoBuffer[3].ShouldBe(endianValue[4]);
        intoBuffer[4].ShouldBe((byte)0xEE);
    }

    [DataTestMethod]
    [DataRow((Int64)0x123456789ABC, EndianFormat.Big)]
    [DataRow((Int64)0x123456789ABC, EndianFormat.Little)]
    public void CopyToErrorsTest(Int64 value, EndianFormat endianFormat)
    {
        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianInt48(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(-1, myBuffer, 1, EndianInt48.Size);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianInt48(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(0, myBuffer, -1, EndianInt48.Size);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianInt48(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(1, myBuffer, 0, EndianInt48.Size);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianInt48(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(0, myBuffer, 17, EndianInt48.Size);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            _ = new EndianInt48(new byte[EndianInt48.Size - 1], endianFormat);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianInt48(value, endianFormat);
            _ = myEndianValue[EndianInt48.Size];
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianInt48(value, endianFormat);
            _ = myEndianValue[-1];
        });
    }

    [DataTestMethod]
    [DataRow((Int64)0x123456789ABC, EndianFormat.Big, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC })]
    [DataRow((Int64)0x123456789ABC, EndianFormat.Little, new byte[] { 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12 })]
    [DataRow((Int64)(-1), EndianFormat.Little, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF })]
    [DataRow((Int64)(-2), EndianFormat.Little, new byte[] { 0xFE, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF })]
    [DataRow((Int64)(-2), EndianFormat.Big, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFE })]
    public void FromExtensionTest(Int64 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = value.AsEndianInt48(endianFormat);
        endianValue.Count.ShouldBe(EndianInt48.Size);
        endianValue.Value.ShouldBe(value);
        endianValue[0].ShouldBe(valueBuffer[0]);
        endianValue[1].ShouldBe(valueBuffer[1]);
        endianValue[2].ShouldBe(valueBuffer[2]);
        endianValue[3].ShouldBe(valueBuffer[3]);
        endianValue[4].ShouldBe(valueBuffer[4]);
        endianValue[5].ShouldBe(valueBuffer[5]);

        var endianBuffer = valueBuffer.AsEndianInt48(endianFormat);
        endianBuffer[0].ShouldBe(valueBuffer[0]);
        endianBuffer[1].ShouldBe(valueBuffer[1]);
        endianBuffer[2].ShouldBe(valueBuffer[2]);
        endianBuffer[3].ShouldBe(valueBuffer[3]);
        endianBuffer[4].ShouldBe(valueBuffer[4]);
        endianBuffer[5].ShouldBe(valueBuffer[5]);

        endianBuffer = (new ArraySegment<byte>(valueBuffer, 0, EndianInt48.Size)).AsEndianInt48(endianFormat);
        endianBuffer[0].ShouldBe(valueBuffer[0]);
        endianBuffer[1].ShouldBe(valueBuffer[1]);
        endianBuffer[2].ShouldBe(valueBuffer[2]);
        endianBuffer[3].ShouldBe(valueBuffer[3]);
        endianBuffer[4].ShouldBe(valueBuffer[4]);
        endianBuffer[5].ShouldBe(valueBuffer[5]);

        valueBuffer.AsInt48(endianFormat).ShouldBe(value);

        var buffer = new byte[20];
        Array.Fill(buffer, (byte)0xEE);
        value.CopyToBufferInt48(endianFormat, 0, buffer, 1, EndianInt48.Size);
        buffer[0].ShouldBe((byte)0xEE);
        buffer[1].ShouldBe(valueBuffer[0]);
        buffer[2].ShouldBe(valueBuffer[1]);
        buffer[3].ShouldBe(valueBuffer[2]);
        buffer[4].ShouldBe(valueBuffer[3]);
        buffer[5].ShouldBe(valueBuffer[4]);
        buffer[6].ShouldBe(valueBuffer[5]);
        buffer[7].ShouldBe((byte)0xEE);

        Array.Fill(buffer, (byte)0xEE);
        value.CopyToBufferInt48(endianFormat, buffer, 1);
        buffer[0].ShouldBe((byte)0xEE);
        buffer[1].ShouldBe(valueBuffer[0]);
        buffer[2].ShouldBe(valueBuffer[1]);
        buffer[3].ShouldBe(valueBuffer[2]);
        buffer[4].ShouldBe(valueBuffer[3]);
        buffer[5].ShouldBe(valueBuffer[4]);
        buffer[6].ShouldBe(valueBuffer[5]);
        buffer[7].ShouldBe((byte)0xEE);
    }

    [TestMethod]
    public void EnumeratorBuilderTest()
    {
        Should.Throw<InvalidOperationException>(() => {
            var builder = new EnumeratorForSix<byte>();
            builder.CurrentValueForIndex(EndianInt48.Size);
        });
    }
}
