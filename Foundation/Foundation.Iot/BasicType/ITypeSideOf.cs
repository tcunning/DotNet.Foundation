namespace Foundation.Iot.BasicType;

public interface ITypeSideOf
{
    public static abstract int TypeSizeOf { get; }
}

public class TypeSizeOfValue<TValue> : ITypeSideOf
{
    public static int TypeSizeOf { get; } = Unsafe.SizeOf<TValue>();
}

public class TypeSizeOfValue6Bytes : ITypeSideOf
{
    public static int TypeSizeOf { get; } = 6;
}