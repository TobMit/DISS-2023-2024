namespace DISS.Random;

/// <summary>
/// Abstraktná trieda pre generátory náhodných čisel
/// </summary>
/// <typeparam name="T">Parameter je tu preto, lebo niekedy môžubyť double a inokedy int</typeparam>
public abstract class ExtendedRandom<T>
{ 
    protected int Seed { get; set; }

    public System.Random generator { get; set; }
    
    public ExtendedRandom()
    {
        System.Random seedGenerator = new System.Random();
        Seed = seedGenerator.Next();
        generator = new System.Random(Seed);
    }

    public ExtendedRandom(int seed)
    {
        Seed = seed;
        generator = new System.Random(Seed);
    }
    
    /// <summary>
    /// Daľšie náhodné číslo
    /// </summary>
    /// <returns>Vráti daľšie náhodné čislo z rozdelenia</returns>
    public abstract T Next();
}