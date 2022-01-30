using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Foundation.Iot.BasicType;
using Foundation.Iot.Endian;

// See https://aka.ms/new-console-template for more information

Console.WriteLine("Foundation.Iot Benchmark");
var ok = BenchmarkRunner.Run<BenchmarkRun>();

Test1A();
Test1B();
Test2A();
Test2B();
Test3();

void Test1A()
{
    UInt32 testValue = 0x12345678;
    var testValueDump = testValue.AsList();
    Console.WriteLine($"{testValue:X} = {testValueDump[0]:X}, {testValueDump[1]:X}, {testValueDump[2]:X}, {testValueDump[3]:X}");
    testValueDump.ToList().ForEach(x => Console.WriteLine($"{x:X}"));
}

void Test1B()
{
    UInt32 testValue = 0x12345678;
    var testValueDump = testValue.AsList(EndianFormat.Little);
    Console.WriteLine($"{testValue:X} = {testValueDump[0]:X}, {testValueDump[1]:X}, {testValueDump[2]:X}, {testValueDump[3]:X}");
    testValueDump.ToList().ForEach(x => Console.WriteLine($"{x:X}"));
}

void Test2A()
{
    UInt32 testValue = 0x12345678;
    var testValueDump = testValue.AsEndian(EndianFormat.Big);
    Console.WriteLine($"{testValue:X} = {testValueDump[0]:X}, {testValueDump[1]:X}, {testValueDump[2]:X}, {testValueDump[3]:X}");
    testValueDump.ToList().ForEach(x => Console.WriteLine($"{x:X}"));
}

void Test2B()
{
    UInt32 testValue = 0x12345678;
    var testValueDump = testValue.AsEndian(EndianFormat.Little);
    Console.WriteLine($"{testValue:X} = {testValueDump[0]:X}, {testValueDump[1]:X}, {testValueDump[2]:X}, {testValueDump[3]:X}");
    testValueDump.ToList().ForEach(x => Console.WriteLine($"{x:X}"));
}

void Test3()
{
    UInt16 testValue = 0x1234;
    var testValueDump = testValue.AsEndian(EndianFormat.Little);
    Console.WriteLine($"{testValue:X} = {testValueDump[0]:X}, {testValueDump[1]:X}");
    testValueDump.ToList().ForEach(x => Console.WriteLine($"{x:X}"));
}
