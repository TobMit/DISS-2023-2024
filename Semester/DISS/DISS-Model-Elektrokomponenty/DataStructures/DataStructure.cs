using DISS_Model_Elektrokomponenty.Entity;
using DISS_Model_Elektrokomponenty.Entity.Pokladna;

namespace DISS_Model_Elektrokomponenty.DataStructures;

public class DataStructure : EventArgs
{
    /// <summary>
    /// True - ak beží simulácia v plnej rýchlosti
    /// Fasle - ak beží simulácia v pomalom režime
    /// </summary>
    public bool ShallowUpdate { get; set; }
    public bool NewData { get; set; }
    public string SimulationTime { get; set; }
    public List<Person> People { get; set; }
    public string RadaPredAutomatom { get; set; }
    public Automat Automat { get; set; }
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

    public double BehZavislostiPriemernyPocetZakaznikovPredAutomatom { get; set; }

    public DataStructure()
    {
        NewData = true;
    }
}