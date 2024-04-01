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
}