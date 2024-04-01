namespace DISS_Model_Elektrokomponenty.Entity.Pokladna;

public class Pokladna
{
    public Queue<Person> Queue { get; private set; }
    public int ID { get; private set; }

    public bool Obsadena { get; private set; }
    public Person? Person { get; private set; }

    public Pokladna(int id)
    {
        Queue = new();
        ID = id;
        Person = null;
    }

    /// <summary>
    /// Vycistenie pokladne
    /// </summary>
    public void Clear()
    {
        Queue.Clear();
        Obsadena = false;
        Person = null;
    }
    
    /// <summary>
    /// Obsadenie pokladne
    /// </summary>
    /// <param name="person">Človek ktorý obsádza pokladňu</param>
    public void ObsadPokladnu(Person person)
    {
        Person = person;
        Person.StavZakaznika = Constants.StavZakaznika.PokladnaPlati;
        Obsadena = true;
    }
    
    /// <summary>
    /// Uvoľnenie pokladne
    /// </summary>
    public void UvolniPokladnu()
    {
        Person = null;
        Obsadena = false;
    }
    
    /// <summary>
    /// Informacie na vypis
    /// </summary>
    /// <returns>Informacie na vypis</returns>
    public override string ToString()
    {
        if (Person is null)
        {
            return $"Pokladna {ID}: \n\t- Voľná\n\t- Predavač: nečinný\n\t- Rada: {Queue.Count}";
        }
        return $"Pokladna {ID}: \n\t- Stojí Person: {Person?.ID} \n\t- Predavač: vybavuje pladbu\n\t- Rada: {Queue.Count}";
    }
}