using DISS_HelperClasses.Statistic;

namespace DISS_Model_Elektrokomponenty.Entity;

/// <summary>
/// Automat ktorý vydáva lístky
/// </summary>
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
    
    /// <summary>
    /// Obslúži osobu na automate
    /// </summary>
    /// <param name="person">Osoba na automate</param>
    public void Obsluz(Person person)
    {
        Person = person;
        Obsadeny = true;
        Person.StavZakaznika = Constants.StavZakaznika.ObsluhujeAutomat;
        StatVytazenieAutomatu.AddValue(_core.SimulationTime, true);
    }
    
    /// <summary>
    /// Uvolni automat
    /// </summary>
    public void Uvolni()
    {
        Obsadeny = false;
        Person = null;
        StatVytazenieAutomatu.AddValue(_core.SimulationTime, false);
    }

    /// <summary>
    /// Vyčistí automat
    /// </summary>
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