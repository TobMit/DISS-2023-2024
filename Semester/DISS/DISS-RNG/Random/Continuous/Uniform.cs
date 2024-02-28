namespace DISS.Random.Continous;

/// <summary>
/// Spojité rovnomerné rozdelenie
/// </summary>
public class Uniform : ExtendedRandom<double>
{
    private readonly double _min;
    private readonly double _max;

    public Uniform(double pMin, double pMax)
    {
        _min = pMin;
        _max = pMax;
    }

    public Uniform(double pMin, double pMax, int seed) : base(seed)
    {
        _min = pMin;
        _max = pMax;
    }

    public override double Next()
    {
        // DaZZ normalizácia vzorec transformovaný na interval A, B
        return _min + (_max - _min) * generator.NextDouble();
    }
}