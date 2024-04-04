using DISS.Random;
using DISS.Random.Discrete;
using DISS.Random.Other;

namespace DISS_Model_Elektrokomponenty.RNG;

public class RNGPickPokladna : ExtendedRandom<double>
{
    List<ExtendedRandom<int>> _listOfRNGs;

    private int _pocetPokladni;
    
    public RNGPickPokladna(int pPocetPokladni)
    {
        _pocetPokladni = pPocetPokladni;
        _listOfRNGs = new ();
        _listOfRNGs.Add(new Deterministic<int>(0));
        for (int i = 1; i < _pocetPokladni; i++)
        {
            _listOfRNGs.Add(new Uniform(0, i));
        }
    }
    
    public RNGPickPokladna(int seed, int pPocetPokladni) : base(seed)
    {
        _pocetPokladni = pPocetPokladni;
        _listOfRNGs = new ();
        _listOfRNGs.Add(new Deterministic<int>(0));
        for (int i = 1; i < _pocetPokladni; i++)
        {
            _listOfRNGs.Add(new Uniform(0, i, NextSeed()));
        }
    }
    
    public override double Next()
    {
        throw new InvalidOperationException("[RNGPickPokladna] - toto sa nepouziva");
    }
    
    /// <summary>
    /// Vráti náhodné čislo podľa počtu pokladní
    /// </summary>
    /// <param name="pocetPokladni">počet pokladní</param>
    /// <returns></returns>
    public int Next(int pocetPokladni)
    {
        return _listOfRNGs[pocetPokladni - 1].Next();
    }
}