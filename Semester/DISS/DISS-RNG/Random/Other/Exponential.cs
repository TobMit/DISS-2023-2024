namespace DISS.Random.Other;


/// <summary>
/// Exponencialne rozdelenie pravdepodobnosti
/// </summary>
public class Exponential : ExtendedRandom<double>
{
    private double _mean;

    public Exponential(double pMean)
    {
        _mean = pMean;
    }
    
    public Exponential(double pMean, int pSeed) : base(pSeed)
    {
        _mean = pMean;
    }
    
    public override double Next()
    {
        var rndNumber = generator.NextDouble();
        return Math.Log(1.0 - rndNumber) * _mean;
    }
}