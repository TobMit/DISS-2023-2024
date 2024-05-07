

using simulation;

namespace DISS_Model_AgentElektrokomponenty.Entity;

/// <summary>
/// Zákazník v systéme
/// </summary>
public class Person
{
    public int ID { get; set; }
    /// <summary>
    /// Kedy sa zákazník vytvoril v systéme
    /// </summary>
    public double VstupDoPredajne { get; set; }
    public double VstupDoRadyPredAutomatom { get; set; }
    public double VstupDoRadyPredObsluhov { get; set; }
    public double VstupDoRadyPredPokladnov { get; set; }

    public double TimeOfArrival { get; set; }
    
    public Constants.TypZakaznika TypZakaznika { get; set; }
    public Constants.TypNarocnostiTovaru TypNarocnostiTovaru { get; private set; }
    public Constants.TypVelkostiNakladu TypVelkostiNakladu { get; private set; }
    public Constants.StavZakaznika StavZakaznika { get; set; }
    public ObsluzneMiesto? ObsluzneMiesto { get; set; }

    public Person(double pTimeOfArrival = 0,
        Constants.TypZakaznika prTypZakaznika = 0,
        double prTypNarocnostTovaru = 0,
        double prTypVelkostiNakladu = 0, 
        int pId = 0)
    {
        TimeOfArrival = pTimeOfArrival;
        TypZakaznika = prTypZakaznika;
        SetTypNarocnostiTovaru(prTypNarocnostTovaru);
        SetTypVelkostiNakladu(prTypVelkostiNakladu);
        StavZakaznika = Constants.StavZakaznika.RadPredAutomatom;
        ID = pId;
    }
    

    public void SetTypNarocnostiTovaru(double prTypNarocnostTovaru)
    {
        if (prTypNarocnostTovaru < 0.3)
        {
            TypNarocnostiTovaru = Constants.TypNarocnostiTovaru.Jednoduchá;
        }
        else if (prTypNarocnostTovaru < 0.7)
        {
            TypNarocnostiTovaru = Constants.TypNarocnostiTovaru.Normálna;
        }
        else
        {
            TypNarocnostiTovaru = Constants.TypNarocnostiTovaru.Zložitá;
        }
    }

    public void SetTypVelkostiNakladu(double prTypVelkostiNakladu)
    {
        if (prTypVelkostiNakladu < 0.6)
        {
            TypVelkostiNakladu = Constants.TypVelkostiNakladu.Veľká;
        }
        else
        {
            TypVelkostiNakladu = Constants.TypVelkostiNakladu.Normálna;
        }
    }
    
    public Person(Person person)
    {
        ID = person.ID;
        VstupDoPredajne = person.VstupDoPredajne;
        VstupDoRadyPredAutomatom = person.VstupDoRadyPredAutomatom;
        VstupDoRadyPredObsluhov = person.VstupDoRadyPredObsluhov;
        VstupDoRadyPredPokladnov = person.VstupDoRadyPredPokladnov;
        TimeOfArrival = person.TimeOfArrival;
        TypZakaznika = person.TypZakaznika;
        TypNarocnostiTovaru = person.TypNarocnostiTovaru;
        TypVelkostiNakladu = person.TypVelkostiNakladu;
        StavZakaznika = person.StavZakaznika;
    }
    
    public override string ToString()
    {
        return $"{ID}\t  | {TypZakaznika, -10}\t | {TypNarocnostiTovaru, -10}\t | {TypVelkostiNakladu, -8}\t | {StavZakaznika, -34}";
    }
}