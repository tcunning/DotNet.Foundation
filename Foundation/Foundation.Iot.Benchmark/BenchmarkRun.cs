using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using Foundation.Iot.Endian;


[Config(typeof(Config))]
[MemoryDiagnoser]
public class BenchmarkRun
{
    private class Config : ManualConfig
    {
        public Config()
        {
            //AddJob(Job.MediumRun
            //    .WithLaunchCount(1)
            //    .WithId("OutOfProc"));

            AddJob(Job.MediumRun
                .WithLaunchCount(1)
                .WithToolchain(InProcessEmitToolchain.Instance)
                .WithId("InProcess"));
        }
    }
    
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
        var testValueDump = new ReadOnlyListUInt32(testValue, Endian.Little);
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
        //var testValueDump = new Endian<uint>(testValue, Endian.Big, (val, numBits) => (byte)(val >> numBits));
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
        //var testValueDump = new Endian<uint>(testValue, Endian.Big, (val, numBits) => (byte)(val >> numBits));
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