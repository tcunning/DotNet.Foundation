using Foundation.Iot.Collection;
using Foundation.Iot.Endian;

namespace Foundation.Iot.Test.Endian;

[TestClass]
[ExcludeFromCodeCoverage]
public class EndianUInt64Test
{
    [DataTestMethod]
    [DataRow((UInt64)0x123456789ABCDEF0, EndianFormat.Big, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0})]
    [DataRow((UInt64)0x123456789ABCDEF0, EndianFormat.Little, new byte[] { 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12 })]
    public void FromValueTest(UInt64 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = new EndianUInt64(value, endianFormat);

        endianValue.Count.ShouldBe(EndianUInt64.Size);

        endianValue.Value.ShouldBe(value);
        endianValue[0].ShouldBe(valueBuffer[0]);
        endianValue[1].ShouldBe(valueBuffer[1]);
        endianValue[2].ShouldBe(valueBuffer[2]);
        endianValue[3].ShouldBe(valueBuffer[3]);
        endianValue[4].ShouldBe(valueBuffer[4]);
        endianValue[5].ShouldBe(valueBuffer[5]);
        endianValue[6].ShouldBe(valueBuffer[6]);
        endianValue[7].ShouldBe(valueBuffer[7]);

        var index = 0;
        foreach (var foundValue in endianValue)
            foundValue.ShouldBe(valueBuffer[index++]);
        
        endianValue.ToArray().ShouldBe(valueBuffer);
        endianValue.ToList().ShouldBe(valueBuffer);
    }

    [DataTestMethod]
    [DataRow((UInt64)0x123456789ABCDEF0, EndianFormat.Big, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 })]
    [DataRow((UInt64)0x123456789ABCDEF0, EndianFormat.Little, new byte[] { 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12 })]
    public void FromBufferTest(UInt64 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = new EndianUInt64(valueBuffer, endianFormat);

        endianValue.Count.ShouldBe(EndianUInt64.Size);

        endianValue.Value.ShouldBe(value);
        endianValue[0].ShouldBe(valueBuffer[0]);
        endianValue[1].ShouldBe(valueBuffer[1]);
        endianValue[2].ShouldBe(valueBuffer[2]);
        endianValue[3].ShouldBe(valueBuffer[3]);
        endianValue[4].ShouldBe(valueBuffer[4]);
        endianValue[5].ShouldBe(valueBuffer[5]);
        endianValue[6].ShouldBe(valueBuffer[6]);
        endianValue[7].ShouldBe(valueBuffer[7]);

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
    [DataRow((UInt64)0x123456789ABCDEF0, EndianFormat.Big)]
    [DataRow((UInt64)0x123456789ABCDEF0, EndianFormat.Little)]
    public void CopyToBufferSimpleTest(UInt64 value, EndianFormat endianFormat)
    {
        var endianValue = new EndianUInt64(value, endianFormat);
        var intoBuffer = new byte[20];

        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(intoBuffer, 0);
        intoBuffer[0].ShouldBe(endianValue[0]);
        intoBuffer[1].ShouldBe(endianValue[1]);
        intoBuffer[2].ShouldBe(endianValue[2]);
        intoBuffer[3].ShouldBe(endianValue[3]);
        intoBuffer[4].ShouldBe(endianValue[4]);
        intoBuffer[5].ShouldBe(endianValue[5]);
        intoBuffer[6].ShouldBe(endianValue[6]);
        intoBuffer[7].ShouldBe(endianValue[7]);
        intoBuffer[8].ShouldBe((byte)0xEE);

        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(intoBuffer, 1);
        intoBuffer[0].ShouldBe((byte)0xEE);
        intoBuffer[1].ShouldBe(endianValue[0]);
        intoBuffer[2].ShouldBe(endianValue[1]);
        intoBuffer[3].ShouldBe(endianValue[2]);
        intoBuffer[4].ShouldBe(endianValue[3]);
        intoBuffer[5].ShouldBe(endianValue[4]);
        intoBuffer[6].ShouldBe(endianValue[5]);
        intoBuffer[7].ShouldBe(endianValue[6]);
        intoBuffer[8].ShouldBe(endianValue[7]);
        intoBuffer[9].ShouldBe((byte)0xEE);
    }

    [DataTestMethod]
    [DataRow((UInt64)0x123456789ABCDEF0, EndianFormat.Big)]
    [DataRow((UInt64)0x123456789ABCDEF0, EndianFormat.Little)]
    public void CopyToBufferAdvancedTest(UInt64 value, EndianFormat endianFormat)
    {
        var endianValue = new EndianUInt64(value, endianFormat);
        var intoBuffer = new byte[20];

        // Fill from start of both buffers
        //
        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(0,intoBuffer, 0, EndianUInt64.Size);
        intoBuffer[0].ShouldBe(endianValue[0]);
        intoBuffer[1].ShouldBe(endianValue[1]);
        intoBuffer[2].ShouldBe(endianValue[2]);
        intoBuffer[3].ShouldBe(endianValue[3]);
        intoBuffer[4].ShouldBe(endianValue[4]);
        intoBuffer[5].ShouldBe(endianValue[5]);
        intoBuffer[6].ShouldBe(endianValue[6]);
        intoBuffer[7].ShouldBe(endianValue[7]);
        intoBuffer[8].ShouldBe((byte)0xEE);
        
        // Fill from start of source buffer but 1 into destination buffer
        //
        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(0, intoBuffer, 1, EndianUInt64.Size);
        intoBuffer[0].ShouldBe((byte)0xEE);
        intoBuffer[1].ShouldBe(endianValue[0]);
        intoBuffer[2].ShouldBe(endianValue[1]);
        intoBuffer[3].ShouldBe(endianValue[2]);
        intoBuffer[4].ShouldBe(endianValue[3]);
        intoBuffer[5].ShouldBe(endianValue[4]);
        intoBuffer[6].ShouldBe(endianValue[5]);
        intoBuffer[7].ShouldBe(endianValue[6]);
        intoBuffer[8].ShouldBe(endianValue[7]);
        intoBuffer[9].ShouldBe((byte)0xEE);

        // Fill from start of source buffer, but right at end of destination buffer
        //
        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(0, intoBuffer, intoBuffer.Length-EndianUInt64.Size, EndianUInt64.Size);
        intoBuffer[11].ShouldBe((byte)0xEE);
        intoBuffer[12].ShouldBe(endianValue[0]);
        intoBuffer[13].ShouldBe(endianValue[1]);
        intoBuffer[14].ShouldBe(endianValue[2]);
        intoBuffer[15].ShouldBe(endianValue[3]);
        intoBuffer[16].ShouldBe(endianValue[4]);
        intoBuffer[17].ShouldBe(endianValue[5]);
        intoBuffer[18].ShouldBe(endianValue[6]);
        intoBuffer[19].ShouldBe(endianValue[7]);

        // Fill from 1 into the source
        //
        Array.Fill(intoBuffer, (byte)0xEE);
        endianValue.CopyTo(1, intoBuffer, 0, EndianUInt64.Size-4);
        intoBuffer[0].ShouldBe(endianValue[1]);
        intoBuffer[1].ShouldBe(endianValue[2]);
        intoBuffer[2].ShouldBe(endianValue[3]);
        intoBuffer[3].ShouldBe(endianValue[4]);
        intoBuffer[4].ShouldBe((byte)0xEE);
    }

    [DataTestMethod]
    [DataRow((UInt64)0x123456789ABCDEF0, EndianFormat.Big)]
    [DataRow((UInt64)0x123456789ABCDEF0, EndianFormat.Little)]
    public void CopyToErrorsTest(UInt64 value, EndianFormat endianFormat)
    {
        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt64(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(-1, myBuffer, 1, EndianUInt64.Size);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt64(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(0, myBuffer, -1, EndianUInt64.Size);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt64(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(1, myBuffer, 0, EndianUInt64.Size);
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt64(value, endianFormat);
            var myBuffer = new byte[20];
            myEndianValue.CopyTo(0, myBuffer, 17, EndianUInt64.Size);
        });
    }

    [DataTestMethod]
    [DataRow(EndianUInt64.Size + 1, EndianFormat.Big)]
    [DataRow(EndianUInt64.Size + 1, EndianFormat.Little)]
    [DataRow(EndianUInt64.Size - 1, EndianFormat.Big)]
    [DataRow(EndianUInt64.Size - 1, EndianFormat.Little)]
    [DataRow(0, EndianFormat.Big)]
    [DataRow(0, EndianFormat.Little)]
    [DataRow(1, EndianFormat.Big)]
    [DataRow(1, EndianFormat.Little)]
    public void ByteConstructionErrorsTest(int size, EndianFormat endianFormat)
    {
        Should.Throw<ArgumentOutOfRangeException>(() => {
            _ = new EndianUInt64(new byte[size], endianFormat);
        });
    }

    [DataTestMethod]
    [DataRow((UInt64) 0x123456789ABCDEF0, EndianFormat.Big)]
    [DataRow((UInt64) 0x123456789ABCDEF0, EndianFormat.Little)]
    public void IndexErrorsTest(UInt64 value, EndianFormat endianFormat)
    {
        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt64(value, endianFormat);
            _ = myEndianValue[EndianUInt64.Size];
        });

        Should.Throw<ArgumentOutOfRangeException>(() => {
            var myEndianValue = new EndianUInt64(value, endianFormat);
            _ = myEndianValue[-1];
        });
    }

    [DataTestMethod]
    [DataRow((UInt64)0x123456789ABCDEF0, EndianFormat.Big, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 })]
    [DataRow((UInt64)0x123456789ABCDEF0, EndianFormat.Little, new byte[] { 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12 })]
    public void FromExtensionTest(UInt64 value, EndianFormat endianFormat, byte[] valueBuffer)
    {
        var endianValue = value.AsEndianUInt64(endianFormat);
        endianValue.Count.ShouldBe(EndianUInt64.Size);
        endianValue.Value.ShouldBe(value);
        endianValue[0].ShouldBe(valueBuffer[0]);
        endianValue[1].ShouldBe(valueBuffer[1]);
        endianValue[2].ShouldBe(valueBuffer[2]);
        endianValue[3].ShouldBe(valueBuffer[3]);
        endianValue[4].ShouldBe(valueBuffer[4]);
        endianValue[5].ShouldBe(valueBuffer[5]);
        endianValue[6].ShouldBe(valueBuffer[6]);
        endianValue[7].ShouldBe(valueBuffer[7]);

        var endianBuffer = valueBuffer.AsEndianUInt64(endianFormat);
        endianBuffer[0].ShouldBe(valueBuffer[0]);
        endianBuffer[1].ShouldBe(valueBuffer[1]);
        endianBuffer[2].ShouldBe(valueBuffer[2]);
        endianBuffer[3].ShouldBe(valueBuffer[3]);
        endianBuffer[4].ShouldBe(valueBuffer[4]);
        endianBuffer[5].ShouldBe(valueBuffer[5]);
        endianBuffer[6].ShouldBe(valueBuffer[6]);
        endianBuffer[7].ShouldBe(valueBuffer[7]);

        endianBuffer = (new ArraySegment<byte>(valueBuffer, 0, EndianUInt64.Size)).AsEndianUInt64(endianFormat);
        endianBuffer[0].ShouldBe(valueBuffer[0]);
        endianBuffer[1].ShouldBe(valueBuffer[1]);
        endianBuffer[2].ShouldBe(valueBuffer[2]);
        endianBuffer[3].ShouldBe(valueBuffer[3]);
        endianBuffer[4].ShouldBe(valueBuffer[4]);
        endianBuffer[5].ShouldBe(valueBuffer[5]);
        endianBuffer[6].ShouldBe(valueBuffer[6]);
        endianBuffer[7].ShouldBe(valueBuffer[7]);

        valueBuffer.AsUInt64(endianFormat).ShouldBe(value);

        var buffer = new byte[20];
        Array.Fill(buffer, (byte)0xEE);
        value.CopyToBufferUInt64(endianFormat, 0, buffer, 1, EndianUInt64.Size);
        buffer[0].ShouldBe((byte)0xEE);
        buffer[1].ShouldBe(valueBuffer[0]);
        buffer[2].ShouldBe(valueBuffer[1]);
        buffer[3].ShouldBe(valueBuffer[2]);
        buffer[4].ShouldBe(valueBuffer[3]);
        buffer[5].ShouldBe(valueBuffer[4]);
        buffer[6].ShouldBe(valueBuffer[5]);
        buffer[7].ShouldBe(valueBuffer[6]);
        buffer[8].ShouldBe(valueBuffer[7]);
        buffer[9].ShouldBe((byte)0xEE);

        Array.Fill(buffer, (byte)0xEE);
        value.CopyToBufferUInt64(endianFormat, buffer, 1);
        buffer[0].ShouldBe((byte)0xEE);
        buffer[1].ShouldBe(valueBuffer[0]);
        buffer[2].ShouldBe(valueBuffer[1]);
        buffer[3].ShouldBe(valueBuffer[2]);
        buffer[4].ShouldBe(valueBuffer[3]);
        buffer[5].ShouldBe(valueBuffer[4]);
        buffer[6].ShouldBe(valueBuffer[5]);
        buffer[7].ShouldBe(valueBuffer[6]);
        buffer[8].ShouldBe(valueBuffer[7]);
        buffer[9].ShouldBe((byte)0xEE);
    }

    [TestMethod]
    public void EnumeratorBuilderTest()
    {
        Should.Throw<InvalidOperationException>(() => {
            var builder = new EnumeratorForEight<byte>();
            builder.CurrentValueForIndex(EndianUInt64.Size);
        });
    }
}
