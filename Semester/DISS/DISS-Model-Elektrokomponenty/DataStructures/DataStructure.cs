using DISS_Model_Elektrokomponenty.Entity;

namespace DISS_Model_Elektrokomponenty.DataStructures;

public class DataStructure : EventArgs
{
    /// <summary>
    /// True - ak beží simulácia v plnej rýchlosti
    /// Fasle - ak beží simulácia v pomalom režime
    /// </summary>
    public bool ShallowUpdate { get; set; }

    public string SimulationTime { get; set; }
    public List<Person> People { get; set; }
    public string RadaPredAutomatom { get; set; }
    public string Automat { get; set; }
    public string RadaPredObsluznimiMiestami { get; set; }
    public List<string> ObsluzneMiestos { get; set; }
    public List<string> Pokladne { get; set; }

    public string AktuaReplikacia { get; set; }
    public string PriemernyCasVObhchode { get; set; }
    public string PriemernyCasPredAutomatom { get; set; }
    public string PriemernaDlzkaraduPredAutomatom { get; set; }
    public string PriemernyOdchodPoslednehoZakaznika { get; set; }
    public string PriemernyPocetZakaznikov { get; set; }
}