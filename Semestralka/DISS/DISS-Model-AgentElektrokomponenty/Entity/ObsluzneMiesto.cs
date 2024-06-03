using DISS_HelperClasses.Statistic;
using OSPStat;
using simulation;

namespace DISS_Model_AgentElektrokomponenty.Entity;

/// <summary>
/// Obslužné miesto pre zákazníka
/// </summary>
public class ObsluzneMiesto
{
    public Person? Person { get; set; }
    public bool Obsadena { get; private set; }
    public int ID { get; private set; }
    public string Name { get; private set; }
    public bool Online { get; private set; }
    public bool Break { get; set; }

    private WStat _priemerneVytazenieOM;

    // public ObsluzneMiesto(Person pPerson, int id, Core pCore, bool online = false)
    public ObsluzneMiesto(Person pPerson, int id, WStat pStat, bool online = false)
    {
        Person = pPerson;
        Obsadena = false;
        ID = id;
        Name = online ? $"Online {id}." : $"Ostatné {id}.";
        Online = online;
        _priemerneVytazenieOM = pStat;
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
            throw new InvalidOperationException($"[Obslužné miesto {ID}] - už je obsluhovaný človek {Person.ID}");
        }

        Person = pPerson;
        Person.StavZakaznika = Constants.StavZakaznika.ObslužnomMieste_ZadávaObjednávku;
        Obsadena = true;
        _priemerneVytazenieOM.AddSample(1);
    }
    
    
    /// <summary>
    ///  Uvolni miesto keď je obsluha dokončená, v prípade veľkého nákladu keď je dokončená platba
    /// </summary>
    /// <param name="pZaznamenajStatistiku">Ak chcem uvolniť a pridať do do štatistiky, v prípade veľkého tovaru už je pokladník uvoľnený</param>
    /// <exception cref="InvalidOperationException">Ak je miesto už voľné</exception>
    public void Uvolni(bool pZaznamenajStatistiku = true)
    {
        if (Person is null)
        {
            throw new InvalidOperationException($"[Obslužné miesto {ID}] - je už volné");
        }

        Person = null;
        Obsadena = false;
        if (pZaznamenajStatistiku)
        {
            _priemerneVytazenieOM.AddSample(0);
        }
    }

    /// <summary>
    /// Uvolní predavača ale nie pokladňu, "predavač" je nečinný
    /// </summary>
    public void UvolniPredavaca()
    {
        _priemerneVytazenieOM.AddSample(0);
    }

    /// <summary>
    /// Informácie na UI
    /// </summary>
    /// <returns>Informácie na UI</returns>
    public override string ToString()
    {
        double vytaznie = 0;
        var confidenceVytazenie = new double[] { 0, 0 };
        if (_priemerneVytazenieOM.SampleSize > 1)
        {
            vytaznie = _priemerneVytazenieOM.Mean() * 100;
            confidenceVytazenie = _priemerneVytazenieOM.ConfidenceInterval95;
        }

        if (Break)
        {
            return $"OM {ID}: \n\t- Voľné\n\t- Pracovník: na pokladni\n\t- Vyťaženie: {vytaznie:0.00}% - [{confidenceVytazenie[0]*100:0.00}% - {confidenceVytazenie[1]*100:0.00}%]";
        }

        if (Person is null)
        {
            return $"OM {ID}: \n\t- Voľné\n\t- Pracovník: nečinný\n\t- Vyťaženie: {vytaznie:0.00}% - [{confidenceVytazenie[0]*100:0.00}% - {confidenceVytazenie[1]*100:0.00}%]";
        }
        else if (Person?.StavZakaznika > Constants.StavZakaznika.ObslužnomMieste_ČakáNaTovar)
        {
            return
                $"OM {ID}: \n\t- Obsadená Person: {Person?.ID} (veľký tovar) \n\t- Predavač: voľný\n\t- Vyťaženie: {vytaznie:0.00}% - [{confidenceVytazenie[0]*100:0.00}% - {confidenceVytazenie[1]*100:0.00}%]";
        }

        return
            $"OM {ID}: \n\t- Stojí Person: {Person?.ID} \n\t- Predavač: {(Person?.StavZakaznika == Constants.StavZakaznika.ObslužnomMieste_ZadávaObjednávku ? "zadáva objednávku" : "vybavuje objednávku")}\n\t- Vyťaženie: {vytaznie:0.00}% - [{confidenceVytazenie[0]*100:0.00}% - {confidenceVytazenie[1]*100:0.00}%]";
    }
    
    /// <summary>
    /// Clear the service place
    /// </summary>
    public void Clear()
    {
        Obsadena = false;
        Person = null;
        _priemerneVytazenieOM.Clear();
        Break = false;
    }
}