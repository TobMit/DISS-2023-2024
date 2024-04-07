namespace DISS_Model_Elektrokomponenty.Entity;

/// <summary>
/// Manažér obslúžnych miest
/// </summary>
public class ObsluzneMiestoManager
{
    public List<ObsluzneMiesto> ListObsluznychOnlineMiest { get; private set; }
    public List<ObsluzneMiesto> ListObsluznychOstatnyMiest { get; private set; }

    private int _pocetObsluznychMiest;
    private Core _core;

    public ObsluzneMiestoManager(int pPocetObsluznychMiest, Core pCore)
    {
        _pocetObsluznychMiest = pPocetObsluznychMiest;
        ListObsluznychOnlineMiest = new();
        ListObsluznychOstatnyMiest = new();
        _core = pCore;
    }

    public void InitObsluzneMiesta()
    {
        int pocetOnlineMiest = _pocetObsluznychMiest / 3;
        int pocetOstatnych = _pocetObsluznychMiest - pocetOnlineMiest;

        for (int i = 0; i < pocetOnlineMiest; i++)
        {
            ListObsluznychOnlineMiest.Add(new(null, i, _core, true));
        }

        for (int i = 0; i < pocetOstatnych; i++)
        {
            ListObsluznychOstatnyMiest.Add(new(null, i, _core));
        }
    }

    /// <summary>
    /// Vyčistí obslužné miesta
    /// </summary>
    public void Clear()
    {
        ListObsluznychOnlineMiest.Clear();
        ListObsluznychOstatnyMiest.Clear();
        InitObsluzneMiesta();
    }

    /// <summary>
    /// Vráti voľne obsluzne miesto pre online zákzaníkov
    /// </summary>
    /// <returns>Ak je volne vrati Obsluzne miesto inak null</returns>
    public ObsluzneMiesto? GetVolneOnline()
    {
        foreach (ObsluzneMiesto miesto in ListObsluznychOnlineMiest)
        {
            if (!miesto.Obsadena)
            {
                return miesto;
            }
        }

        return null;
    }

    /// <summary>
    /// Vráti voľne obsluzne miesto pre ostatných zákazníkov
    /// </summary>
    /// <returns>Ak je volne vrati Obsluzne miesto inak null</returns>
    public ObsluzneMiesto? GetVolneOstatne()
    {
        foreach (ObsluzneMiesto miesto in ListObsluznychOstatnyMiest)
        {
            if (!miesto.Obsadena)
            {
                return miesto;
            }
        }

        return null;
    }

    /// <summary>
    /// Informácie na obrazovku
    /// </summary>
    /// <returns>Vráti informácie na obrazovku</returns>
    public List<ObsluzneMiesto> GetInfoNaUI()
    {
        return ListObsluznychOnlineMiest.Concat(ListObsluznychOstatnyMiest).ToList();
    }
}