namespace Foundation.Iot.BasicType;

public interface ITypeSideOf
{
    public static abstract int TypeSizeOf { get; }
}

//public class TypeSizeOfValue<TValue> : ITypeSideOf
//{
//    public static int TypeSizeOf { get; } = Unsafe.SizeOf<TValue>();
//}

public class TypeSizeOfValue2Bytes : ITypeSideOf
{
    public static int TypeSizeOf { get; } = 2;
}

public class TypeSizeOfValue4Bytes : ITypeSideOf
{
    public static int TypeSizeOf { get; } = 4;
}

public class TypeSizeOfValue6Bytes : ITypeSideOf
{
    public static int TypeSizeOf { get; } = 6;
}

public class TypeSizeOfValue8Bytes : ITypeSideOf
{
    public static int TypeSizeOf { get; } = 8;
}