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
        Person.ID = CelkovyPocet++;
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
    
    public void PripocitajOdydenych(int countOdydenych)
    {
        CelkovyPocet += countOdydenych;
    }
}