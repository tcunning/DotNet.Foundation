using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Iot.Endian;

public class Endian<TValue>
    where TValue : IBinaryInteger<TValue>
{
    private TValue MyValue;

    public Endian(TValue value)
    {
        MyValue = value;

        var ok = value + value;
        ok = value << 3;
    }

    public void Test2()
    {
        Console.WriteLine($"Value = {MyValue:X}");
        Console.WriteLine($"Radix = {TValue.Radix}");
        Console.WriteLine($"GetByteCount = {MyValue.GetByteCount()}");

        Span<byte> test = stackalloc byte[MyValue.GetByteCount()];

        MyValue.WriteBigEndian(test);

        var output = "";
        for (int i = 0; i < test.Length; i++)
        {
            output += $"{test[i]:X2} ";
        }
        
        Console.WriteLine($"GetByteCount = {output}");

        Console.WriteLine($"--------");
    }
}

