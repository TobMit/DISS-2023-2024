namespace DISS_Model_Elektrokomponenty.Entity.Pokladna;

/// <summary>
/// Manažér pokladní
/// </summary>
public class PokladnaManager
{
    public List<Pokladna> ListPokladni { get; private set; }
    private int _pocetPokladni;
    private Core _core;

    public PokladnaManager(int pPocetPokladni, Core pCore)
    {
        _pocetPokladni = pPocetPokladni;
        ListPokladni = new();
        _core = pCore;
    }

    public void InitPokladne()
    {
        for (int i = 0; i < _pocetPokladni; i++)
        {
            ListPokladni.Add(new(i, _core));
        }
    }

    /// <summary>
    /// Vráti voľnú pokladňu ak nie nikto obsluhovaný a je voľný rad ak je voľných viac vyberie náhodne
    /// </summary>
    /// <returns>Pokladňu ak splna požiadavky inak null</returns>
    public Pokladna? GetVolnaPokladnaPrazdnyRad(Core core)
    {
        // vytvorí list kde je rad 0 a pokladňa nie je obsadená
        var listPokladni = ListPokladni
            .Where(p => !p.Obsadena && p.Queue.Count == 0)
            .OrderBy(g => g.ID)
            .ToList();

        if (listPokladni.Count > 0)
        {
            return listPokladni[core.RndPickPokladna.Next(listPokladni.Count)];
        }

        return null;
    }

    /// <summary>
    /// Priradí zákazníka do najkratšej rady, a keď majú viaceré rovnaké dĺžky tak náhodne vyberie jednu
    /// </summary>
    /// <param name="person">Človek ktorý sa pridáva do rady</param>
    /// <param name="core">Jadro simulácie</param>
    public void PriradZakaznikaDoRady(Person person, Core core)
    {
        // spraví list pokladní s najkratšími radami
        var listPokladni = ListPokladni.GroupBy(c => c.Queue.Count)
            .OrderBy(g => g.Key)
            .FirstOrDefault();

        if (listPokladni is not null)
        {
            var list = listPokladni.ToList();
            var pokladna = list[core.RndPickPokladna.Next(list.Count)];
            pokladna.Queue.Enqueue(person);
            pokladna.PriemernaDlzkaRadu.AddValue(_core.SimulationTime, pokladna.Queue.Count);
        }
    }

    /// <summary>
    /// Vyčistí pokladne
    /// </summary>
    public void Clear()
    {
        ListPokladni.Clear();
        InitPokladne();
    }

    /// <summary>
    /// Informácie na UI
    /// </summary>
    /// <returns>Informácie na UI</returns>
    public List<Pokladna> GetInfoNaUI()
    {
        return ListPokladni;
    }
}