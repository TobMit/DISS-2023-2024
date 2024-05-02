



using DISS_Model_AgentElektrokomponenty.Entity;

public class DataStructure : EventArgs
{
    /// <summary>
    /// True - ak beží simulácia v plnej rýchlosti
    /// False - ak beží simulácia v pomalom režime
    /// </summary>
    public bool ShallowUpdate { get; set; }
    public bool NewData { get; set; }
    public string SimulationTime { get; set; }
    public List<String> People { get; set; }
    public string RadaPredAutomatom { get; set; }
    //public Automat Automat { get; set; } //todo resolve
    public string RadaPredObsluznimiMiestamiOnline { get; set; }
    public string RadaPredObsluznimiMiestamiBasic { get; set; }
    public string RadaPredObsluznimiMiestamiZmluvny { get; set; }
    public List<ObsluzneMiesto> ObsluzneMiestos { get; set; }
    public List<Pokladna> Pokladne { get; set; }

    public string AktuaReplikacia { get; set; }
    public string PriemernyCasVObhchode { get; set; }
    public string PriemernyCasPredAutomatom { get; set; }
    public string PriemernaDlzkaraduPredAutomatom { get; set; }
    public string PriemernyOdchodPoslednehoZakaznika { get; set; }
    public string PriemernyPocetZakaznikov { get; set; }
    public string PriemernyPocetObsluzenychZakaznikov { get; set; }
    public string PriemerneVytazenieAutomatu { get; set; }
    public string PriemerneDlzkyRadovPredObsluhov { get; set; }
    public string PriemerneDlzkyRadovPredPokladnami { get; set; }
    public string PriemerneVytazeniePokladni { get; set; }
    public string PriemerneVytazenieObsluhyOnline { get; set; }
    public string PriemerneVytazenieObsluhyOstatne { get; set; }
    public string IntervalSpolahlivstiCasuVsysteme { get; set; }

    public double BehZavislostiPriemernyPocetZakaznikovPredAutomatom { get; set; }

    public DataStructure()
    {
        NewData = true;
    }
}