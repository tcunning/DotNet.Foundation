using BenchmarkDotNet.Attributes;
using Foundation.Iot.BasicType;
using Foundation.Iot.Endian;

[MemoryDiagnoser]
public class BenchmarkRun
{
    public BenchmarkRun()
    {
    }

    [Benchmark]
    public void Test1A()
    {
        uint testValue = 0x12345678;
        var testValueDump = testValue.AsList();
        var ok = testValueDump[0] + testValueDump[1] + testValueDump[2] + testValueDump[3];
        foreach (var value in testValueDump)
            ok -= value;
    }

    [Benchmark]
    public void Test1B()
    {
        uint testValue = 0x12345678;
        var testValueDump = new ReadOnlyListUInt32(testValue, EndianFormat.Little);
        var ok = testValueDump[0] + testValueDump[1] + testValueDump[2] + testValueDump[3];
        foreach (var value in testValueDump)
            ok -= value;
    }

    [Benchmark]
    public void Test2()
    {
        uint testValue = 0x12345678;
        var testValueDump = new Endian<uint>(testValue, EndianFormat.Big, (val, numBits) => (byte)(val >> numBits));
        var ok = testValueDump[0] + testValueDump[1] + testValueDump[2] + testValueDump[3];
        foreach (var value in testValueDump)
            ok -= value;
    }

    [Benchmark]
    public void Test4()
    {
        uint testValue = 0x12345678;
        var testValueDump = BitConverter.GetBytes(testValue);
        var ok = testValueDump[0] + testValueDump[1] + testValueDump[2] + testValueDump[3];
        foreach (var value in testValueDump)
            ok -= value;
    }
}