using System.Runtime.CompilerServices;

public static class MathUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Multiplay(this (int, int) value)
    {
        return value.Item1 * value.Item2;
    }
}
