namespace DISS_Model_Elektrokomponenty.Entity;

public class RadaPredObsluznymMiestom
{
    /// <summary>
    /// Počet ľudí v rade
    /// </summary>
    public int Count => _basicPersons.Count + _zmluvnyPersons.Count + _onlinePersons.Count;

    public int CountOnline => _onlinePersons.Count;
    public int CountOstatne => _basicPersons.Count + _zmluvnyPersons.Count;

    private readonly Queue<Person> _basicPersons;
    private readonly Queue<Person> _zmluvnyPersons;
    private readonly Queue<Person> _onlinePersons;

    public RadaPredObsluznymMiestom()
    {
        _basicPersons = new();
        _zmluvnyPersons = new Queue<Person>();
        _onlinePersons = new();
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
                break;
            case Constants.TypZakaznika.Zmluvny:
                _zmluvnyPersons.Enqueue(person);
                break;
            case Constants.TypZakaznika.Online:
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

            return _onlinePersons.Dequeue();
        }

        if (_zmluvnyPersons.Count > 0)
        {
            return _zmluvnyPersons.Dequeue();
        }

        if (_basicPersons.Count > 0)
        {
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
    }
}