namespace DISS_HelperClasses;

/// <summary>
/// Pair class to hold two values of different types
/// </summary>
/// <typeparam name="TFirst">First type</typeparam>
/// <typeparam name="TSecond">Second type</typeparam>
public class Pair<TFirst, TSecond>
{
    public TFirst First { get; set; }
    public TSecond Second { get; set; }

    public Pair(TFirst first, TSecond second)
    {
        First = first;
        Second = second;
    }
}