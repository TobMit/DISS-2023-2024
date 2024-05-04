using DISS_HelperClasses.Statistic;
using DISS_Model_AgentElektrokomponenty.Entity;
using OSPStat;
using simulation;


/// <summary>
/// Pokladňa pre zákazníka
/// </summary>
public class Pokladna
{
    
    
    public OSPDataStruct.SimQueue<MyMessage> Queue { get; private set; }
    public int ID { get; private set; }

    public bool Obsadena { get; private set; }
    public Person? Person { get; private set; }

    public bool Prestavka { get; set; }

    public string Name { get; private set; }

    public WStat PriemerneVytazeniePredajne { get; set; }

    public Pokladna(int id, WStat pPriemernaDlzkaRadu, WStat pPriemerneVytazenie)
    {
        Queue = new(pPriemernaDlzkaRadu);
        ID = id;
        Person = null;
        Name = $"Pokladňa {ID}.";
        PriemerneVytazeniePredajne = pPriemerneVytazenie;
        Prestavka = false;
    }

    /// <summary>
    /// Vyčistenie pokladne
    /// </summary>
    public void Clear()
    {
        Queue.Clear();
        Obsadena = false;
        Person = null;
        PriemerneVytazeniePredajne.Clear();
        Prestavka = false;
    }
    
    /// <summary>
    /// Obsadenie pokladne
    /// </summary>
    /// <param name="person">Človek ktorý obsadí pokladňu</param>
    public void ObsadPokladnu(Person person)
    {
        Person = person;
        Person.StavZakaznika = Constants.StavZakaznika.PokladňaPlatí;
        Obsadena = true;
        PriemerneVytazeniePredajne.AddSample(1);
    }
    
    /// <summary>
    /// Uvoľnenie pokladne
    /// </summary>
    public void UvolniPokladnu()
    {
        Person = null;
        Obsadena = false;
        PriemerneVytazeniePredajne.AddSample(0);
    }
    
    /// <summary>
    /// Informácie na UI
    /// </summary>
    /// <returns>Informácie na UI</returns>
    public override string ToString()
    {
        double vytaznie = 0;
        if (PriemerneVytazeniePredajne.SampleSize > 0)
        {
            vytaznie = PriemerneVytazeniePredajne.Mean() * 100;
        }

        if (Prestavka)
        {
            return $"Pokladňa {ID}: \n\t- Voľná\n\t- Predavač: na prestávke\n\t- Rada: {Queue.Count}\n\t- Vyťaženie: {vytaznie:0.00}%";
        }
        if (Person is null)
        {
            return $"Pokladňa {ID}: \n\t- Voľná\n\t- Predavač: nečinný\n\t- Rada: {Queue.Count}\n\t- Vyťaženie: {vytaznie:0.00}%";
        }
        return $"Pokladňa {ID}: \n\t- Stojí Person: {Person?.ID} \n\t- Predavač: vybavuje platbu\n\t- Rada: {Queue.Count}\n\t- Vyťaženie: {vytaznie:0.00}%";
    }
}