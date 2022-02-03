using Foundation.Iot.Collection;
using Foundation.Iot.Endian;

namespace Foundation.Iot.Test.Endian;

[TestClass]
[ExcludeFromCodeCoverage]
public class EndianUInt48Test
{
    [DataTestMethod]
    [DataRow((UInt64)0x123456789ABC, EndianFormat.Big, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC})]
    [DataRow((UInt64)0x123456789ABC, EndianFormat.Little, new byte[] { 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12 })]
    public void FromValueTest(UInt64 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = new EndianUInt48(value, endianFormat);

        endianValue.Count.ShouldBe(EndianUInt48.Size);

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
    [DataRow((UInt64)0x123456789ABC, EndianFormat.Big, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC })]
    [DataRow((UInt64)0x123456789ABC, EndianFormat.Little, new byte[] { 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12 })]
    public void FromBufferTest(UInt64 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = new EndianUInt48(valueBuffer, endianFormat);

        endianValue.Count.ShouldBe(EndianUInt48.Size);

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
    [DataRow((UInt64)0x123456789ABC, EndianFormat.Big)]
    [DataRow((UInt64)0x123456789ABC, EndianFormat.Little)]
    public void CopyToBufferSimpleTest(UInt64 value, EndianFormat endianFormat)
    {
        var endianValue = new EndianUInt48(value, endianFormat);
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
    [DataRow((UInt64)0x123456789ABC, EndianFormat.Big)]
    [DataRow((UInt64)0x123456789ABC, EndianFormat.Little)]
    public void CopyToBufferAdvancedTest(UInt64 value, EndianFormat endianFormat)
    {
        var endianValue = new EndianUInt48(value, endianFormat);
        var intoBuffer = new byte[20];

        // Fill from start of both buffers
        //
        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(0,intoBuffer, 0, EndianUInt48.Size);
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
        endianValue.CopyTo(0, intoBuffer, 1, EndianUInt48.Size);
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
        endianValue.CopyTo(0, intoBuffer, intoBuffer.Length- EndianUInt48.Size, EndianUInt48.Size);
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
        endianValue.CopyTo(1, intoBuffer, 0, EndianUInt48.Size - 2);
        intoBuffer[0].ShouldBe(endianValue[1]);
        intoBuffer[1].ShouldBe(endianValue[2]);
        intoBuffer[2].ShouldBe(endianValue[3]);
        intoBuffer[3].ShouldBe(endianValue[4]);
        intoBuffer[4].ShouldBe((byte)0xEE);
    }

    [DataTestMethod]
    [DataRow((UInt64)0x123456789ABC, EndianFormat.Big)]
    [DataRow((UInt64)0x123456789ABC, EndianFormat.Little)]
    public void CopyToErrorsTest(UInt64 value, EndianFormat endianFormat)
    {
        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt48(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(-1, myBuffer, 1, EndianUInt48.Size);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt48(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(0, myBuffer, -1, EndianUInt48.Size);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt48(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(1, myBuffer, 0, EndianUInt48.Size);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt48(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(0, myBuffer, 17, EndianUInt48.Size);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            _ = new EndianUInt48(new byte[EndianUInt48.Size - 1], endianFormat);
        });
    }

    [DataTestMethod]
    [DataRow(EndianUInt48.Size + 1, EndianFormat.Big)]
    [DataRow(EndianUInt48.Size + 1, EndianFormat.Little)]
    [DataRow(EndianUInt48.Size - 1, EndianFormat.Big)]
    [DataRow(EndianUInt48.Size - 1, EndianFormat.Little)]
    [DataRow(0, EndianFormat.Big)]
    [DataRow(0, EndianFormat.Little)]
    [DataRow(1, EndianFormat.Big)]
    [DataRow(1, EndianFormat.Little)]
    public void ByteConstructionErrorsTest(int size, EndianFormat endianFormat)
    {
        Should.Throw<ArgumentOutOfRangeException>(() => {
            _ = new EndianUInt48(new byte[size], endianFormat);
        });
    }

    [DataTestMethod]
    [DataRow((UInt64)UInt64.MaxValue, EndianFormat.Big)]
    [DataRow((UInt64)UInt64.MaxValue, EndianFormat.Little)]
    [DataRow((UInt64)0x80123456789ABC, EndianFormat.Big)]
    [DataRow((UInt64)0x80123456789ABC, EndianFormat.Little)]
    public void IndexErrorsTest(UInt64 value, EndianFormat endianFormat)
    {
        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt48(value, endianFormat);
            _ = myEndianValue[EndianUInt48.Size];
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt48(value, endianFormat);
            _ = myEndianValue[-1];
        });
    }

    [DataTestMethod]
    [DataRow((UInt64)0x123456789ABC, EndianFormat.Big, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC })]
    [DataRow((UInt64)0x123456789ABC, EndianFormat.Little, new byte[] { 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12 })]
    public void FromExtensionTest(UInt64 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = value.AsEndianUInt48(endianFormat);
        endianValue.Count.ShouldBe(EndianUInt48.Size);
        endianValue.Value.ShouldBe(value);
        endianValue[0].ShouldBe(valueBuffer[0]);
        endianValue[1].ShouldBe(valueBuffer[1]);
        endianValue[2].ShouldBe(valueBuffer[2]);
        endianValue[3].ShouldBe(valueBuffer[3]);
        endianValue[4].ShouldBe(valueBuffer[4]);
        endianValue[5].ShouldBe(valueBuffer[5]);

        var endianBuffer = valueBuffer.AsEndianUInt48(endianFormat);
        endianBuffer[0].ShouldBe(valueBuffer[0]);
        endianBuffer[1].ShouldBe(valueBuffer[1]);
        endianBuffer[2].ShouldBe(valueBuffer[2]);
        endianBuffer[3].ShouldBe(valueBuffer[3]);
        endianBuffer[4].ShouldBe(valueBuffer[4]);
        endianBuffer[5].ShouldBe(valueBuffer[5]);

        endianBuffer = (new ArraySegment<byte>(valueBuffer, 0, EndianUInt48.Size)).AsEndianUInt48(endianFormat);
        endianBuffer[0].ShouldBe(valueBuffer[0]);
        endianBuffer[1].ShouldBe(valueBuffer[1]);
        endianBuffer[2].ShouldBe(valueBuffer[2]);
        endianBuffer[3].ShouldBe(valueBuffer[3]);
        endianBuffer[4].ShouldBe(valueBuffer[4]);
        endianBuffer[5].ShouldBe(valueBuffer[5]);

        valueBuffer.AsUInt48(endianFormat).ShouldBe(value);

        var buffer = new byte[20];
        Array.Fill(buffer, (byte)0xEE);
        value.CopyToBufferUInt48(endianFormat, 0, buffer, 1, EndianUInt48.Size);
        buffer[0].ShouldBe((byte)0xEE);
        buffer[1].ShouldBe(valueBuffer[0]);
        buffer[2].ShouldBe(valueBuffer[1]);
        buffer[3].ShouldBe(valueBuffer[2]);
        buffer[4].ShouldBe(valueBuffer[3]);
        buffer[5].ShouldBe(valueBuffer[4]);
        buffer[6].ShouldBe(valueBuffer[5]);
        buffer[7].ShouldBe((byte)0xEE);

        Array.Fill(buffer, (byte)0xEE);
        value.CopyToBufferUInt48(endianFormat, buffer, 1);
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
            builder.CurrentValueForIndex(EndianUInt48.Size);
        });
    }
}
