using DISS_HelperClasses.Statistic;

namespace DISS_Model_Elektrokomponenty.Entity.Pokladna;

/// <summary>
/// Pokladňa pre zákazníka
/// </summary>
public class Pokladna
{
    private Core _core;
    
    public Queue<Person> Queue { get; private set; }
    public int ID { get; private set; }

    public bool Obsadena { get; private set; }
    public Person? Person { get; private set; }

    public string Name { get; private set; }

    public WeightedAverage PriemernaDlzkaRadu { get; set; }
    public WorkLoadAverage PriemerneVytazeniePredajne { get; set; }

    public Pokladna(int id, Core pCore)
    {
        Queue = new();
        ID = id;
        Person = null;
        Name = $"Pokladňa {ID}.";
        PriemernaDlzkaRadu = new ();
        PriemerneVytazeniePredajne = new ();
        _core = pCore;
    }

    /// <summary>
    /// Vyčistenie pokladne
    /// </summary>
    public void Clear()
    {
        Queue.Clear();
        Obsadena = false;
        Person = null;
        PriemernaDlzkaRadu.Clear();
        PriemerneVytazeniePredajne.Clear();
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
        PriemerneVytazeniePredajne.AddValue(_core.SimulationTime, true);
    }
    
    /// <summary>
    /// Uvoľnenie pokladne
    /// </summary>
    public void UvolniPokladnu()
    {
        Person = null;
        Obsadena = false;
        PriemerneVytazeniePredajne.AddValue(_core.SimulationTime, false);
    }
    
    /// <summary>
    /// Informácie na UI
    /// </summary>
    /// <returns>Informácie na UI</returns>
    public override string ToString()
    {
        double vytaznie = 0;
        if (PriemerneVytazeniePredajne.Count > 2)
        {
            vytaznie = PriemerneVytazeniePredajne.Calucate(_core.SimulationTime) * 100;
        }
        if (Person is null)
        {
            return $"Pokladňa {ID}: \n\t- Voľná\n\t- Predavač: nečinný\n\t- Rada: {Queue.Count}\n\t- Vyťaženie: {vytaznie:0.00}%";
        }
        return $"Pokladňa {ID}: \n\t- Stojí Person: {Person?.ID} \n\t- Predavač: vybavuje platbu\n\t- Rada: {Queue.Count}\n\t- Vyťaženie: {vytaznie:0.00}%";
    }
}