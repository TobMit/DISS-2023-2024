namespace DISS.Random.Other;

/// <summary>
/// Deterministické rozdelenie pravdepodobnosti
/// </summary>
/// <typeparam name="T">typ vracanej hodnoty</typeparam>
public class Deterministic<T> :  ExtendedRandom<T> where T : struct // struct je tu preto aby "zaručilo" numerický typ
{
    private readonly T _value;
    public Deterministic(T pValue)
    {
        _value = pValue;
    }

    public override T Next()
    {
        return _value;
    }
}