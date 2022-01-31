using BenchmarkDotNet.Attributes;
using Foundation.Iot.BasicType;
using Foundation.Iot.Endian;

[MemoryDiagnoser]
public class BenchmarkRun
{
    public BenchmarkRun()
    {
    }

    private int _ok;

    /*
    [Benchmark]
    public void Test1A()
    {
        uint testValue = 0x12345678;
        var testValueDump = testValue.AsList();
        var ok = testValueDump[0] + testValueDump[1] + testValueDump[2] + testValueDump[3];
        //foreach (var value in testValueDump)
        //    ok -= value;
    }

    [Benchmark]
    public void Test1B()
    {
        uint testValue = 0x12345678;
        var testValueDump = new ReadOnlyListUInt32(testValue, EndianFormat.Little);
        var ok = testValueDump[0] + testValueDump[1] + testValueDump[2] + testValueDump[3];
        //foreach (var value in testValueDump)
        //    ok -= value;
    }
    */

    private byte[] _buffer = new byte[8];

    [Benchmark]
    public void Test2A()
    {
        uint testValue = 0x12345678;
        //var testValueDump = new Endian<uint>(testValue, EndianFormat.Big, (val, numBits) => (byte)(val >> numBits));
        var testValueDump = new EndianUInt32(testValue, EndianFormat.Big);
        _ok = testValueDump[0] + testValueDump[1] + testValueDump[2] + testValueDump[3];
    }

    [Benchmark]
    public void Test2B()
    {
        uint testValue = 0x12345678;
        var testValueDump = new EndianUInt32(testValue, EndianFormat.Big);
        _ok = testValueDump[0] + testValueDump[1] + testValueDump[2] + testValueDump[3];
        foreach (var value in testValueDump)
            _ok -= value;
    }

    [Benchmark]
    public void Test2C()
    {
        uint testValue = 0x12345678;
        var testValueDump = new EndianUInt32(testValue, EndianFormat.Big);
        _ok = testValueDump[0] + testValueDump[1] + testValueDump[2] + testValueDump[3];
        _ = testValueDump.ToArray();
        _ = testValueDump.ToArray();
        _ = testValueDump.ToArray();
        _ = testValueDump.ToArray();
    }

    [Benchmark]
    public void Test2D()
    {
        uint testValue = 0x12345678;
        //var testValueDump = new Endian<uint>(testValue, EndianFormat.Big, (val, numBits) => (byte)(val >> numBits));
        var testValueDump = new EndianUInt32(testValue, EndianFormat.Big);
        testValueDump.CopyTo(_buffer, 0);
    }

    [Benchmark]
    public void Test4A()
    {
        uint testValue = 0x12345678;
        var testValueDump = BitConverter.GetBytes(testValue);
        _ok = testValueDump[0] + testValueDump[1] + testValueDump[2] + testValueDump[3];
        foreach (var value in testValueDump)
            _ok -= value;
    }

    [Benchmark]
    public void Test4B()
    {
        uint testValue = 0x12345678;
        var testValueDump = BitConverter.GetBytes(testValue);
        _ok = testValueDump[0] + testValueDump[1] + testValueDump[2] + testValueDump[3];
        foreach (var value in testValueDump)
            _ok -= value;
    }

    [Benchmark]
    public void Test4C()
    {
        uint testValue = 0x12345678;
        var testValueDump = BitConverter.GetBytes(testValue);
        _ok = testValueDump[0] + testValueDump[1] + testValueDump[2] + testValueDump[3];
        _ = testValueDump.ToArray();
        _ = testValueDump.ToArray();
        _ = testValueDump.ToArray();
        _ = testValueDump.ToArray();
    }

    [Benchmark]
    public void Test4D()
    {
        uint testValue = 0x12345678;
        var testValueDump = BitConverter.GetBytes(testValue);
        testValueDump.CopyTo(_buffer, 0);
    }

}