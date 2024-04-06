using DISS_HelperClasses.Statistic;

namespace DISS_Model_Elektrokomponenty.Entity;

public class RadaPredObsluznymMiestom
{
    /// <summary>
    /// Počet ľudí v rade
    /// </summary>
    public int Count => _basicPersons.Count + _zmluvnyPersons.Count + _onlinePersons.Count;

    public int CountOnline => _onlinePersons.Count;
    public int CountOstatne => _basicPersons.Count + _zmluvnyPersons.Count;
    public int CountBasic => _basicPersons.Count;

    private readonly Queue<Person> _basicPersons;
    private readonly Queue<Person> _zmluvnyPersons;
    private readonly Queue<Person> _onlinePersons;
    
    private Core _core;
    public WeightedAverage PriemernaDlzkaBasic { get; set; }
    public WeightedAverage PriemernaDlzkaZmluvny { get; set; }
    public WeightedAverage PriemernaDlzkaOnline { get; set; }

    public RadaPredObsluznymMiestom(Core pCore)
    {
        _basicPersons = new();
        _zmluvnyPersons = new Queue<Person>();
        _onlinePersons = new();
        _core = pCore;
        PriemernaDlzkaBasic = new ();
        PriemernaDlzkaZmluvny = new ();
        PriemernaDlzkaOnline = new ();
    }

    /// <summary>
    /// či sa nachádza v rade online človek
    /// </summary>
    public bool OnlineZakaznikInRow
    {
        get { return _onlinePersons.Count > 0; }
    }

    /// <summary>
    /// Pridanie do frontu ľudí
    /// </summary>
    /// <param name="pPerson">Človek pridaný do frontu</param>
    /// <exception cref="InvalidOperationException">Ak sa pridá človek s nepsrávnym typom, čo by nemalo nastať</exception>
    public void Enqueue(Person pPerson)
    {
        var person = pPerson;
        person.StavZakaznika = Constants.StavZakaznika.CakaVObchode;
        switch (person.TypZakaznika)
        {
            case Constants.TypZakaznika.Basic:
                _basicPersons.Enqueue(person);
                PriemernaDlzkaBasic.AddValue(_core.SimulationTime, _basicPersons.Count);
                break;
            case Constants.TypZakaznika.Zmluvny:
                PriemernaDlzkaZmluvny.AddValue(_core.SimulationTime, _zmluvnyPersons.Count);
                _zmluvnyPersons.Enqueue(person);
                break;
            case Constants.TypZakaznika.Online:
                PriemernaDlzkaOnline.AddValue(_core.SimulationTime, _onlinePersons.Count);
                _onlinePersons.Enqueue(person);
                break;
            default:
                // nemala by nikdy nastať
                throw new InvalidOperationException(
                    $"[RadaPredObsluznymMiestom - Enqueue] - Nesprávny typ zákazníka: {pPerson.TypZakaznika}");
        }
    }

    /// <summary>
    /// Vráti zákaznika z rady
    /// </summary>
    /// <param name="online">ak chceme online zákazníka tak ho vráti z radu</param>
    /// <returns>Zákaznika ktorý čaká</returns>
    /// <exception cref="ArgumentException">Ak sa berie z prázdného frontu</exception>
    public Person Dequeue(bool online = false)
    {
        if (online)
        {
            if (!OnlineZakaznikInRow)
            {
                throw new ArgumentException(
                    "[RadaPredObsluznymMiestom - Dequeue online] - V rade už nie je žiaden online zákazník");
            }
            PriemernaDlzkaOnline.AddValue(_core.SimulationTime, _onlinePersons.Count-1);
            return _onlinePersons.Dequeue();
        }

        if (_zmluvnyPersons.Count > 0)
        {
            PriemernaDlzkaZmluvny.AddValue(_core.SimulationTime, _zmluvnyPersons.Count-1);
            return _zmluvnyPersons.Dequeue();
        }

        if (_basicPersons.Count > 0)
        {
            PriemernaDlzkaBasic.AddValue(_core.SimulationTime, _basicPersons.Count-1);
            return _basicPersons.Dequeue();
        }

        throw new ArgumentException(
            "[RadaPredObsluznymMiestom - Dequeue basic/zmluvny] - V rade už nie je žiaden zákazník");
    }

    /// <summary>
    /// Vyčistenie frontu
    /// </summary>
    public void Clear()
    {
        _basicPersons.Clear();
        _zmluvnyPersons.Clear();
        _onlinePersons.Clear();
        PriemernaDlzkaBasic.Clear();
        PriemernaDlzkaZmluvny.Clear();
        PriemernaDlzkaOnline.Clear();
    }
}