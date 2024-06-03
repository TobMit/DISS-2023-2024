namespace DISS.Random.Other;

/// <summary>
/// Trojuholníkové rozdelenie pravdepodobnosti
/// </summary>
public class Triangular : ExtendedRandom<double>
{
    private double _min;
    private double _max;
    private double _peek;

    public Triangular(double pMin, double pPeek, double pMax)
    {
        _min = pMin;
        _max = pMax;
        _peek = pPeek;
    }
    
    public Triangular(double pMin, double pPeek, double pMax, int pSeed) : base(pSeed)
    {
        _min = pMin;
        _max = pMax;
        _peek = pPeek;
    }

    public override double Next()
    {
        var rndNum1 = generator.NextDouble();
        var rndNum2 = 1.0 - rndNum1;

        return rndNum1 <= (_peek - _min) / (_max - _min)
            ? _min + Math.Sqrt((_max - _min) * (_peek - _min) * rndNum1)
            : _max - Math.Sqrt((_max - _min) * (_max - _peek) * rndNum2);
    }
}