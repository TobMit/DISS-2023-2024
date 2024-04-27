using DISS_HelperClasses.Statistic;
using OSPStat;
using simulation;

namespace DISS_Model_AgentElektrokomponenty.Entity;

/// <summary>
/// Rada pred obslužným miestom pre zákazníkov všetkých druhov
/// </summary>
public class RadaPredObsluznymMiestom
{
    /// <summary>
    /// Počet ľudí v rade
    /// </summary>
    public int Count => _basicPersons.Count + _zmluvnyPersons.Count + _onlinePersons.Count;

    public int CountOnline => _onlinePersons.Count;
    public int CountOstatne => _basicPersons.Count + _zmluvnyPersons.Count;
    public int CountBasic => _basicPersons.Count;

    private readonly OSPDataStruct.SimQueue<MyMessage> _basicPersons;
    private readonly OSPDataStruct.SimQueue<MyMessage> _zmluvnyPersons;
    private readonly OSPDataStruct.SimQueue<MyMessage> _onlinePersons;
    

    public RadaPredObsluznymMiestom(WStat pPriemernaDlzkaBasic, WStat pPriemernaDlzkaZmluvny, WStat pPriemernaDlzkaOnline)
    {
        _basicPersons = new(pPriemernaDlzkaBasic);
        _zmluvnyPersons = new (pPriemernaDlzkaZmluvny);
        _onlinePersons = new(pPriemernaDlzkaOnline);
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
    /// <param name="pSprava"></param>
    /// <exception cref="InvalidOperationException">Ak sa pridá človek s neprávnym typom, čo by nemalo nastať</exception>
    public void Enqueue(MyMessage pSprava)
    {
        var person = pSprava.Zakaznik;
        person.StavZakaznika = Constants.StavZakaznika.ČakáVObchode;
        switch (person.TypZakaznika)
        {
            case Constants.TypZakaznika.Basic:
                _basicPersons.Enqueue(pSprava);
                break;
            case Constants.TypZakaznika.Zmluvný:
                _zmluvnyPersons.Enqueue(pSprava);
                break;
            case Constants.TypZakaznika.Online:
                _onlinePersons.Enqueue(pSprava);
                break;
            default:
                // nemala by nikdy nastať
                throw new InvalidOperationException(
                    $"[RadaPredObslužnýmMiestom - Enqueue] - Nesprávny typ zákazníka: {person.TypZakaznika}");
        }
    }

    /// <summary>
    /// Vráti zákazníka z rady
    /// </summary>
    /// <param name="online">ak chceme online zákazníka tak ho vráti z radu</param>
    /// <returns>Zákazníka ktorý čaká</returns>
    /// <exception cref="ArgumentException">Ak sa berie z prázdneho frontu</exception>
    public MyMessage Dequeue(bool online = false)
    {
        if (online)
        {
            if (!OnlineZakaznikInRow)
            {
                throw new ArgumentException(
                    "[RadaPredObslužnýmMiestom - Dequeue online] - V rade už nie je žiaden online zákazník");
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
            "[RadaPredObslužnýmMiestom - Dequeue basic/zmluvný] - V rade už nie je žiaden zákazník");
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