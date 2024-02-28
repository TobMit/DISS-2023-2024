namespace DISS.Random.Discrete;

/// <summary>
/// Diskrétne rovnomerné rozdelenie
/// </summary>
public class Uniform : ExtendedRandom<int>
{
    private readonly int _min;
    private readonly int _max;
    
    /// <summary>
    /// Vytvorí generátor diskrétneho rovnomerného rozdelenia
    /// </summary>
    /// <param name="pMin">minimalna hodnota</param>
    /// <param name="pMax">maximálna hodnota</param>
    public Uniform(int pMin, int pMax)
    {
        _min = pMin;
        _max = pMax;
    }

    /// <summary>
    /// Vytvorí generátor diskrétneho rovnomerného rozdelenia
    /// </summary>
    /// <param name="pMin">minimalna hodnota</param>
    /// <param name="pMax">maximálna hodnota</param>
    /// <param name="seed">Seed ktorý sa použije na generovanie</param>
    public Uniform(int pMin, int pMax, int seed): base(seed)
    {
        _min = pMin;
        _max = pMax;
    }
    
    public override int Next()
    {
        return _min + generator.Next(_max - _min + 1);
    }
}