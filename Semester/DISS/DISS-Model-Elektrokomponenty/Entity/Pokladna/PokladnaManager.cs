namespace DISS_Model_Elektrokomponenty.Entity.Pokladna;

public class PokladnaManager
{
    public List<Pokladna> ListPokladni { get; private set; }
    private int _pocetPokladni;

    public PokladnaManager(int pPocetPokladni)
    {
        _pocetPokladni = pPocetPokladni;
        ListPokladni = new();
    }

    public void InitPokladne()
    {
        for (int i = 0; i < _pocetPokladni; i++)
        {
            ListPokladni.Add(new(i));
        }
    }

    /// <summary>
    /// Vráti volnu pokdľadňu ak nie nikto obsluhovaný a je voľný rad ak je voľných viac vyberie náhodne
    /// </summary>
    /// <returns>Pokladnu ak splna poziadavky inak null</returns>
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
    /// Priradí zákaznika do najkratšej rady, a keď majú viaceré rovnaké dĺžky tak náhodne vyberie jednu
    /// </summary>
    /// <param name="person">Clovek ktorý sa pridáva do rady</param>
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
        }
    }

    /// <summary>
    /// Vybcistí pokladne
    /// </summary>
    public void Clear()
    {
        ListPokladni.Clear();
        InitPokladne();
    }

    /// <summary>
    /// Informacie na vypis
    /// </summary>
    /// <returns>Informacie na vypis</returns>
    public List<string> GetInfoNaUI()
    {
        List<string> list = new();
        foreach (Pokladna pokladna in ListPokladni)
        {
            list.Add(pokladna.ToString());
        }

        return list;
    }
}