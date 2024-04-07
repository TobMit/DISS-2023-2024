using DISS_HelperClasses.Statistic;

namespace DISS_Model_Elektrokomponenty.Entity;

public class ObsluzneMiesto
{
    public Person? Person { get; set; }
    public bool Obsadena { get; private set; }
    public int ID { get; private set; }
    public string Name { get; private set; }
    
    private Core _core;
    public WorkLoadAverage PriemerneVytazenieOM { get; set; }

    public ObsluzneMiesto(Person pPerson, int id, Core pCore, bool online = false)
    {
        Person = pPerson;
        Obsadena = false;
        ID = id;
        Name = online ? $"Online {id}."  : $"Ostatné {id}.";
        PriemerneVytazenieOM = new();
        _core = pCore;
    }
    
    /// <summary>
    /// Keď je obsluhovaný človek na obslužnom mieste
    /// </summary>
    /// <param name="pPerson">Obsluhovaný človek</param>
    /// <exception cref="InvalidOperationException">Ak je miesto už obsadené</exception>
    public void Obsluz(Person pPerson)
    {
        if (Person is not null)
        {
            throw new InvalidOperationException($"[Obsluzne miesto {ID}] - už je obsluhovaný človek {Person.ID}");
        }
        Person = pPerson;
        Person.StavZakaznika = Constants.StavZakaznika.ObslužnomMieste_ZadávaObjednávku;
        Obsadena = true;
        PriemerneVytazenieOM.AddValue(_core.SimulationTime, true);
    }
    
    /// <summary>
    /// Uvolni miesto keď je obsluha dokončená, v prípade veľkého nákladu keď je dokončená pladba
    /// </summary>
    /// <exception cref="InvalidOperationException">Ak je miesto už voľné</exception>
    public void Uvolni()
    {
        if (Person is null)
        {
            throw new InvalidOperationException($"[Obsluzne miesto {ID}] - je už volné");
        }
        Person = null;
        Obsadena = false;
        PriemerneVytazenieOM.AddValue(_core.SimulationTime, false);
    }
    
    /// <summary>
    /// Informacie na vypis
    /// </summary>
    /// <returns>Informacie na vypis</returns>
    public override string ToString()
    {
        if (Person is null )
        {
            return $"OM {ID}: \n\t- Voľné\n\t- Pracovník: nečinný";
        }
        else if (Person?.StavZakaznika > Constants.StavZakaznika.ObslužnomMieste_ČakáNaTovar)
        {
            return $"OM {ID}: \n\t- Obsadená Person: {Person?.ID} (veľký tovar) \n\t- Predavač: voľný";
        }
        return $"OM {ID}: \n\t- Stojí Person: {Person?.ID} \n\t- Predavač: {(Person?.StavZakaznika == Constants.StavZakaznika.ObslužnomMieste_ZadávaObjednávku ? "zadáva objednávku" : "vybavuje objednávku")}";
    }
}