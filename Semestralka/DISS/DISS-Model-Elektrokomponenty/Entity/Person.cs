namespace DISS_Model_Elektrokomponenty.Entity;

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

    public double TimeOfArrival { get; private set; }
    
    public Constants.TypZakaznika TypZakaznika { get; private set; }
    public Constants.TypNarocnostiTovaru TypNarocnostiTovaru { get; private set; }
    public Constants.TypVelkostiNakladu TypVelkostiNakladu { get; private set; }
    public Constants.StavZakaznika StavZakaznika { get; set; }

    public ObsluzneMiesto? ObsluzneMiesto { get; set; }

    public Person(double pTimeOfArrival,
        double prTypZakaznika,
        double prTypNarocnostTovaru,
        double prTypVelkostiNakladu, 
        int pId)
    {
        TimeOfArrival = pTimeOfArrival;
        SetTypZakanika(prTypZakaznika);
        SetTypNarocnostiTovaru(prTypNarocnostTovaru);
        SetTypVelkostiNakladu(prTypVelkostiNakladu);
        StavZakaznika = Constants.StavZakaznika.RadPredAutomatom;
        ID = pId;
    }
    
    private void SetTypZakanika(double prTypZakaznika)
    {
        if (prTypZakaznika < 0.5)
        {
            TypZakaznika = Constants.TypZakaznika.Basic;
        }
        else if (prTypZakaznika < 0.65)
        {
            TypZakaznika = Constants.TypZakaznika.Zmluvný;
        }
        else
        {
            TypZakaznika = Constants.TypZakaznika.Online;
        }
    }

    private void SetTypNarocnostiTovaru(double prTypNarocnostTovaru)
    {
        if (prTypNarocnostTovaru < 0.3)
        {
            TypNarocnostiTovaru = Constants.TypNarocnostiTovaru.Simple;
        }
        else if (prTypNarocnostTovaru < 0.7)
        {
            TypNarocnostiTovaru = Constants.TypNarocnostiTovaru.Normal;
        }
        else
        {
            TypNarocnostiTovaru = Constants.TypNarocnostiTovaru.Hard;
        }
    }

    private void SetTypVelkostiNakladu(double prTypVelkostiNakladu)
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
}