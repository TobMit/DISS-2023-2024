namespace DISS_Model_Elektrokomponenty.Entity;

public class ObsluzneMiesto
{
    public Person? Person { get; set; }
    public bool Obsadena { get; private set; }
    public int ID { get; private set; }

    public ObsluzneMiesto(Person pPerson, int ID)
    {
        Person = pPerson;
        Obsadena = false;
        this.ID = ID;
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
        Obsadena = true;
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
    }
}