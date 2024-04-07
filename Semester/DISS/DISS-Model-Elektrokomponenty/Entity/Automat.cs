using DISS_HelperClasses.Statistic;

namespace DISS_Model_Elektrokomponenty.Entity;

public class Automat
{
    public int CelkovyPocet { get; private set; }

    public bool Obsadeny { get; private set; }

    public Person? Person { get; private set; }

    public int PocetObsluzenych { get; set; }
    private Core _core;

    public WorkLoadAverage StatVytazenieAutomatu { get; set; }

    public Automat(Core pCore)
    {
        CelkovyPocet = 0;
        _core = pCore;
        StatVytazenieAutomatu = new();
    }
    
    public void Obsluz(Person person)
    {
        Person = person;
        Obsadeny = true;
        Person.StavZakaznika = Constants.StavZakaznika.ObsluhujeAutomat;
        StatVytazenieAutomatu.AddValue(_core.SimulationTime, true);
    }
    
    public void Uvolni()
    {
        Obsadeny = false;
        Person = null;
        StatVytazenieAutomatu.AddValue(_core.SimulationTime, false);
    }

    public void Clear()
    {
        CelkovyPocet = 0;
        Person = null;
        Obsadeny = false;
        PocetObsluzenych = 0;
        StatVytazenieAutomatu.Clear();
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