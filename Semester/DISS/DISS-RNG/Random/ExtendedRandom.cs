namespace DISS.Random;

/// <summary>
/// Abstraktná trieda pre generátory náhodných čísel
/// </summary>
/// <typeparam name="T">Parameter je tu preto, lebo niekedy môžu byť double a inokedy int</typeparam>
public abstract class ExtendedRandom<T>
{ 
    protected int Seed { get; set; }

    protected System.Random generator { get; set; }

    private static System.Random seedGenerator;
    
    public ExtendedRandom()
    {
        Seed = NextSeed();
        generator = new System.Random(Seed);
    }

    public ExtendedRandom(int seed)
    {
        Seed = seed;
        generator = new System.Random(Seed);
    }
    
    /// <summary>
    /// Ďalšie náhodné číslo
    /// </summary>
    /// <returns>Vráti ďalšie náhodné číslo z rozdelenia</returns>
    public abstract T Next();

    /// <summary>
    /// Next seed for generator
    /// </summary>
    /// <returns>New seed</returns>
    public static int NextSeed()
    {
        if (seedGenerator is null)
        {
            seedGenerator = new System.Random();
        }
        return seedGenerator.Next(Int32.MaxValue);
    }
}