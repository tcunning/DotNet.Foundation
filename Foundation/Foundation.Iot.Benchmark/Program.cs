using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Foundation.Iot.BasicType;
using Foundation.Iot.Endian;

// See https://aka.ms/new-console-template for more information

var endianTest1 = new Endian<UInt16>(0x1234);
endianTest1.Test2();

var endianTest2 = new Endian<UInt32>(0x12345678);
endianTest2.Test2();

var endianTest3 = new Endian<UInt64>(0x123456789ABCDEF0);
endianTest3.Test2();


Console.WriteLine("Foundation.Iot Benchmark");
var ok = BenchmarkRunner.Run<BenchmarkRun>();
//var ok = BenchmarkRunner.Run<TypeWithBenchmarks>();

Test1B();
Test2A();
Test2B();
Test3();

void Test1B()
{
    Console.WriteLine($"=== {System.Reflection.MethodBase.GetCurrentMethod()!.Name} ===");
    UInt32 testValue = 0x12345678;
    var testValueDump = testValue.AsEndianUInt32(EndianFormat.Little);
    Console.WriteLine($"{testValue:X} = {testValueDump[0]:X}, {testValueDump[1]:X}, {testValueDump[2]:X}, {testValueDump[3]:X}");
    testValueDump.ToList().ForEach(x => Console.WriteLine($"{x:X}"));

    var buffer = new byte[] {0x12, 0x34, 0x56, 0xAB};
    var testValue2 = buffer.AsEndianUInt32(EndianFormat.Big);
    Console.WriteLine($"{testValue2.Value:X} = {testValue2[0]:X}, {testValue2[1]:X}, {testValue2[2]:X}, {testValue2[3]:X}");

    var testValue3 = buffer.AsEndianUInt32(EndianFormat.Little);
    Console.WriteLine($"{testValue3.Value:X} = {testValue3[0]:X}, {testValue3[1]:X}, {testValue3[2]:X}, {testValue3[3]:X}");
}

void Test2A()
{
    Console.WriteLine($"=== {System.Reflection.MethodBase.GetCurrentMethod()!.Name} ===");
    UInt32 testValue = 0x12345678;
    var testValueDump = testValue.AsEndianUInt32(EndianFormat.Big);
    Console.WriteLine($"{testValue:X} = {testValueDump[0]:X}, {testValueDump[1]:X}, {testValueDump[2]:X}, {testValueDump[3]:X}");
    testValueDump.ToList().ForEach(x => Console.WriteLine($"{x:X}"));
}

void Test2B()
{
    Console.WriteLine($"=== {System.Reflection.MethodBase.GetCurrentMethod()!.Name} ===");
    UInt16 testValue = 0x1234;
    var testValueDump = testValue.AsEndianUInt16(EndianFormat.Little);
    Console.WriteLine($"{testValue:X} = {testValueDump[0]:X}, {testValueDump[1]:X}");
    testValueDump.ToList().ForEach(x => Console.WriteLine($"{x:X}"));
}

void Test3()
{
    Console.WriteLine($"=== {System.Reflection.MethodBase.GetCurrentMethod()!.Name} ===");
    var buffer = new byte[16];

    UInt32 testValue = 0x12345678;
    var testValueDump = testValue.AsEndianUInt32(EndianFormat.Big);
    Console.WriteLine($"{testValue:X} = {testValueDump[0]:X}, {testValueDump[1]:X}");
    testValueDump.CopyTo(2, buffer, 4, 2);
    if( buffer[4] != 0x56 || buffer[5] != 0x78 )
        Console.WriteLine($"Buffer Check Failed {buffer[4]:X}/{buffer[5]:X}");

}
