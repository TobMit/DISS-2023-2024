namespace DISS_Model_Elektrokomponenty.Entity;

public class Automat
{
    public int CelkovyPocet { get; private set; }

    public bool Obsadeny { get; private set; }

    public Person? Person { get; private set; }

    public Automat()
    {
        CelkovyPocet = 0;
    }
    
    public void Obsluz(Person person)
    {
        Person = person;
        Obsadeny = true;
        Person.StavZakaznika = Constants.StavZakaznika.ObsluhujeAutomat;
    }
    
    public void Uvolni()
    {
        Obsadeny = false;
        Person = null;
    }

    public void Clear()
    {
        CelkovyPocet = 0;
        Person = null;
        Obsadeny = false;
    }

    public int GetId()
    {
        return CelkovyPocet++;
    }
    
    public override string ToString()
    {
        if (Person is null)
        {
            return $"Automat: \n\t- Voľný";
        }
        return $"Automat: \n\t- Stojí Person: {Person?.ID}";
    }
}