using DISS_HelperClasses.Statistic;
using DISS_Model_AgentElektrokomponenty.Entity;
using OSPStat;
using simulation;


/// <summary>
/// Pokladňa pre zákazníka
/// </summary>
public class Reklamacia
{
    public OSPDataStruct.SimQueue<MyMessage> Queue { get; private set; }
    public int ID { get; private set; }

    public bool Obsadena { get; private set; }
    public Person? Person { get; private set; }

    public bool Break { get; set; }
    public bool ObsluhujeOm { get; set; }

    public string Name { get; private set; }


    public Reklamacia(int id)
    {
        Queue = new();
        ID = id;
        Person = null;
        Name = $"Reklamacia {ID}.";
        Break = false;
        ObsluhujeOm = false;
    }

    /// <summary>
    /// Vyčistenie pokladne
    /// </summary>
    public void Clear()
    {
        Queue.Clear();
        Obsadena = false;
        Person = null;
        Break = false;
        ObsluhujeOm = false;
    }
    
    /// <summary>
    /// Obsadenie pokladne
    /// </summary>
    /// <param name="person">Človek ktorý obsadí pokladňu</param>
    public void ObsadReklamaciu(Person person)
    {
        if (Person is not null)
        {
            throw new InvalidOperationException($"Pokladna už je obsadená zákazníkom {person.ID}");
        }
        Person = person;
        Person.StavZakaznika = Constants.StavZakaznika.PokladňaPlatí;
        Obsadena = true;
    }
    
    /// <summary>
    /// Uvoľnenie pokladne
    /// </summary>
    public void UvolniReklamaciu()
    {
        Person = null;
        Obsadena = false;
    }
    
    /// <summary>
    /// Informácie na UI
    /// </summary>
    /// <returns>Informácie na UI</returns>
    public override string ToString()
    {
        
        if (Person is null)
        {
            return $"Reklamacia {ID}: \n\t- Voľná\n\t- Predavač: nečinný\n\t- Rada: {Queue.Count}";
        }
        return $"Reklamacia {ID}: \n\t- Stojí Person: {Person?.ID} \n\t- Predavač: vybavuje platbu\n\t- Rada: {Queue.Count}";
    }
}