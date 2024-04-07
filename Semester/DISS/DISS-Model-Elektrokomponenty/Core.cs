using System.Text;
using DISS_EventSimulationCore;
using DISS_HelperClasses.Statistic;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;
using DISS_Model_Elektrokomponenty.Entity.Pokladna;
using DISS_Model_Elektrokomponenty.Eventy;
using DISS_Model_Elektrokomponenty.RNG;
using DISS.Random;
using DISS.Random.Continous;
using DISS.Random.Other;
using UniformD = DISS.Random.Discrete.Uniform;
using UniformC = DISS.Random.Continous.Uniform;


namespace DISS_Model_Elektrokomponenty;

public class Core : EventSimulationCore<Person, DataStructure>
{
    public bool BehZavislosti { get; set; }
    
    private int _pocetObsluznychMiest;
    private int _pocetPokladni;

    // Entity
    public Queue<Person> RadaPredAutomatom;
    public RadaPredObsluznymMiestom RadaPredObsluznymMiestom;
    public ObsluzneMiestoManager ObsluzneMiestoManager;
    public PokladnaManager PokladnaManager;
    public Automat Automat;

    public List<Person> Persons;

    // RNG
    public RNGPickPokladna RndPickPokladna;
    public Exponential RndPrichodZakaznika;
    public UniformC RndTypZakaznika;
    public UniformC RndTypNarocnostTovaru;
    public UniformC RndTypVelkostiNakladu;
    public UniformC RndTrvanieAutomatu;
    public UniformC RndTrvanieDiktovania;
    public Triangular RndTrvanieOnlinePripravaTovaru;
    public Empiric RndTrvaniePripravaSimple;
    public UniformC RndTrvaniePripravaNormal;
    public Empiric RndTrvaniePripravaHard;
    public DISS.Random.Discrete.Empiric RndTrvaniePladba;
    public UniformC RndTrvanieVyzdvyhnutieVelkehoTovaru;

    // štatistiky
    public Average StatPriemernyCasVObchode;
    public Average StatCasStravenyPredAutomatom;
    public WeightedAverage StatPriemednaDlzakaRaduAutomatu;

    // glob statistiky
    private Average _globPriemernyCasVObchode;
    private Average _globCasStravenyPredAutomatom;
    private Average _globPriemernaDlzkaRadu;
    private Average _globPriemernyOdchodPoslednehoZakaznika;
    private Average _globPriemernyPocetZakaznikov;
    private Average _globPriemernyPocetObsluzenychZakaznikov;
    private Average _globPriemerneVytazenieAutomatu;
    private Average _globPriemernaDlzkaRaduPredObsluhouBasic;
    private Average _globPriemernaDlzkaRaduPredObsluhouZmluvny;
    private Average _globPriemernaDlzkaRaduPredObsluhouOnline;
    private List<Average> _globPriemerneDlzkyRadovPredPokladnami;
    private List<Average> _globPriemerneVytazeniePokladni;
    private List<Average> _globPriemerneVytaznieObsluhyOnline;
    private List<Average> _globPriemerneVytaznieObsluhyOstatne;
    private IntervalSpolahlivosti _globIntervalSpolahlivostiCasuVSysteme;

    public Core(int numberOfReplications, int cutFirst, int pPocetObsluznychMiest, int pPocetPokladni) : base(
        numberOfReplications, cutFirst)
    {
        _pocetObsluznychMiest = pPocetObsluznychMiest;
        _pocetPokladni = pPocetPokladni;

        RadaPredAutomatom = new();
        RadaPredObsluznymMiestom = new(this);
        ObsluzneMiestoManager = new(_pocetObsluznychMiest, this);
        PokladnaManager = new(_pocetPokladni, this);
        Automat = new(this);

        Persons = new();

        StatPriemernyCasVObchode = new();
        StatCasStravenyPredAutomatom = new();
        StatPriemednaDlzakaRaduAutomatu = new();

        _globPriemernyCasVObchode = new();
        _globCasStravenyPredAutomatom = new();
        _globPriemernaDlzkaRadu = new();
        _globPriemernyOdchodPoslednehoZakaznika = new();
        _globPriemernyPocetZakaznikov = new();
        _globPriemernyPocetObsluzenychZakaznikov = new();
        _globPriemerneVytazenieAutomatu = new();
        _globPriemernaDlzkaRaduPredObsluhouBasic = new();
        _globPriemernaDlzkaRaduPredObsluhouZmluvny = new();
        _globPriemernaDlzkaRaduPredObsluhouOnline = new();
        _globPriemerneDlzkyRadovPredPokladnami = new(_pocetPokladni);
        _globPriemerneVytazeniePokladni = new(_pocetPokladni);
        for (int i = 0; i < _pocetPokladni; i++)
        {
            _globPriemerneDlzkyRadovPredPokladnami.Add(new());
            _globPriemerneVytazeniePokladni.Add(new());
        }
        _globPriemerneVytaznieObsluhyOnline = new(ObsluzneMiestoManager.ListObsluznychOnlineMiest.Count);
        _globPriemerneVytaznieObsluhyOstatne = new(ObsluzneMiestoManager.ListObsluznychOstatnyMiest.Count);
        _globIntervalSpolahlivostiCasuVSysteme = new();
    }

    public override void BeforeAllReplications()
    {
        END_OF_SIMULATION_TIME = Constants.END_SIMULATION_TIME;

        TimeLine = new();
        ObsluzneMiestoManager.InitObsluzneMiesta();
        PokladnaManager.InitPokladne();
        // Po nainicializovani sa obsluznych miest sa nainicializujú aj statistiky
        for (int i = 0; i < ObsluzneMiestoManager.ListObsluznychOnlineMiest.Count; i++)
        {
            _globPriemerneVytaznieObsluhyOnline.Add(new());
        }
        for (int i = 0; i < ObsluzneMiestoManager.ListObsluznychOstatnyMiest.Count; i++)
        {
            _globPriemerneVytaznieObsluhyOstatne.Add(new());
        }

        // Rozdelenia pravdepodobnosti
        RndPickPokladna = new(ExtendedRandom<double>.NextSeed(), _pocetPokladni);
        // (60*60) / 30 lebo je to 30 zakaznikov za hodinu ale systém beží v sekunádch tak preto ten prepočet
        RndPrichodZakaznika = new(((60.0 * 60.0) / 30.0), ExtendedRandom<double>.NextSeed());
        RndTypZakaznika = new(0, 1, ExtendedRandom<double>.NextSeed());
        RndTypNarocnostTovaru = new(0, 1.0, ExtendedRandom<double>.NextSeed());
        RndTypVelkostiNakladu = new(0, 1.0, ExtendedRandom<double>.NextSeed());
        RndTrvanieAutomatu = new(30.0, 120.0, ExtendedRandom<double>.NextSeed());
        RndTrvanieDiktovania = new(60.0, 900.0, ExtendedRandom<double>.NextSeed());
        RndTrvanieOnlinePripravaTovaru = new(60.0, 120.0, 480.0, ExtendedRandom<double>.NextSeed());
        List<EmpiricBase<double>.EmpiricDataWithSeed<double>> listSimple = new();
        listSimple.Add(new(120.0, 300.0, 0.6, ExtendedRandom<double>.NextSeed()));
        listSimple.Add(new(300.0, 540.0, 0.4, ExtendedRandom<double>.NextSeed()));
        RndTrvaniePripravaSimple = new(listSimple, ExtendedRandom<double>.NextSeed());
        RndTrvaniePripravaNormal = new(540.0, 660.0, ExtendedRandom<double>.NextSeed());
        List<EmpiricBase<double>.EmpiricDataWithSeed<double>> listHard = new();
        listHard.Add(new(660.0, 720.0, 0.1, ExtendedRandom<double>.NextSeed()));
        listHard.Add(new(720.0, 1_200.0, 0.6, ExtendedRandom<double>.NextSeed()));
        listHard.Add(new(1_200.0, 1_500.0, 0.3, ExtendedRandom<double>.NextSeed()));
        RndTrvaniePripravaHard = new(listHard, ExtendedRandom<double>.NextSeed());
        List<EmpiricBase<int>.EmpiricDataWithSeed<int>> listPladba = new();
        listPladba.Add(new(180, 480, 0.4, ExtendedRandom<double>.NextSeed()));
        listPladba.Add(new(180, 360, 0.6, ExtendedRandom<double>.NextSeed()));
        RndTrvaniePladba = new(listPladba, ExtendedRandom<double>.NextSeed());
        RndTrvanieVyzdvyhnutieVelkehoTovaru = new(30.0, 70.0, ExtendedRandom<double>.NextSeed());
    }

    public override void BeforeReplication()
    {
        SimulationTime = Constants.START_ARRIVAL_SIMULATION_TIME;
        TimeLine.Clear();

        RadaPredAutomatom.Clear();
        RadaPredObsluznymMiestom.Clear();
        ObsluzneMiestoManager.Clear();
        PokladnaManager.Clear();
        Automat.Clear();

        Persons.Clear();

        StatPriemernyCasVObchode.Clear();
        StatCasStravenyPredAutomatom.Clear();
        StatPriemednaDlzakaRaduAutomatu.Clear();

        // rozbehnutie modelu
        var newArrival = RndPrichodZakaznika.Next() + SimulationTime;
        TimeLine.Enqueue(new EventPrichod(this, newArrival), newArrival);
    }

    public override void AfterReplication()
    {
        _globPriemernyCasVObchode.AddValue(StatPriemernyCasVObchode.Calucate());
        _globCasStravenyPredAutomatom.AddValue(StatCasStravenyPredAutomatom.Calucate());
        _globPriemernaDlzkaRadu.AddValue(StatPriemednaDlzakaRaduAutomatu.Calucate(Constants.END_ARRIVAL_SIMULATION_TIME));
        _globPriemernyOdchodPoslednehoZakaznika.AddValue(SimulationTime);
        _globPriemernyPocetZakaznikov.AddValue(Automat.CelkovyPocet);
        _globPriemernyPocetObsluzenychZakaznikov.AddValue(Automat.PocetObsluzenych);
        _globPriemerneVytazenieAutomatu.AddValue(Automat.StatVytazenieAutomatu.Calucate(Constants.END_ARRIVAL_SIMULATION_TIME));
        _globPriemernaDlzkaRaduPredObsluhouBasic.AddValue(RadaPredObsluznymMiestom.PriemernaDlzkaBasic.Calucate(SimulationTime));
        _globPriemernaDlzkaRaduPredObsluhouOnline.AddValue(RadaPredObsluznymMiestom.PriemernaDlzkaOnline.Calucate(SimulationTime));
        _globPriemernaDlzkaRaduPredObsluhouZmluvny.AddValue(RadaPredObsluznymMiestom.PriemernaDlzkaZmluvny.Calucate(SimulationTime));
        for (int i = 0; i < _pocetPokladni; i++)
        {
            _globPriemerneDlzkyRadovPredPokladnami[i].AddValue(PokladnaManager.ListPokladni[i].PriemernaDlzkaRadu.Calucate(SimulationTime));
            _globPriemerneVytazeniePokladni[i].AddValue(PokladnaManager.ListPokladni[i].PriemerneVytazeniePredajne.Calucate(SimulationTime));
        }
        for (int i = 0; i < ObsluzneMiestoManager.ListObsluznychOnlineMiest.Count; i++)
        {
            _globPriemerneVytaznieObsluhyOnline[i].AddValue(ObsluzneMiestoManager.ListObsluznychOnlineMiest[i].PriemerneVytazenieOM.Calucate(SimulationTime));
        }
        for (int i = 0; i < ObsluzneMiestoManager.ListObsluznychOstatnyMiest.Count; i++)
        {
            _globPriemerneVytaznieObsluhyOstatne[i].AddValue(ObsluzneMiestoManager.ListObsluznychOstatnyMiest[i].PriemerneVytazenieOM.Calucate(SimulationTime));
        }
        _globIntervalSpolahlivostiCasuVSysteme.AddValue(StatPriemernyCasVObchode.Calucate());
        
        if (BehZavislosti)
        {
            if (_currentReplication < _cutFirst)
            {
                return;
            }
            
            int stepSize = _numberOfReplications / Constants.POCET_DAT_V_GRAFE;
            if (stepSize >= 2)
            {
                if (_currentReplication % stepSize == 0)
                {
                    Tick();
                    OnUpdateData(_eventData);
                }    
            }
            return;
        }
        if (_currentReplication % 100 == 0)
        {
            if (_eventData is not null)
            {
                _eventData.NewData = true;
            }
            Tick();
            OnUpdateData(_eventData);
        }
    }

    public override void AfterAllReplications()
    {
        Console.WriteLine(
            $"Priemerny cas v obchode: {Double.Round(_globPriemernyCasVObchode.Calucate(), 4)}s / {TimeSpan.FromSeconds(_globPriemernyCasVObchode.Calucate()).ToString(@"hh\:mm\:ss")}");
        Console.WriteLine(
            $"Cas straveny pred automatom: {Double.Round(_globCasStravenyPredAutomatom.Calucate(), 4)}s / {TimeSpan.FromSeconds(_globCasStravenyPredAutomatom.Calucate()).ToString(@"hh\:mm\:ss")}");
        Console.WriteLine($"Priemerna dlzka radu pred automatom: {Double.Round(_globPriemernaDlzkaRadu.Calucate(), 4)}");
        Console.WriteLine(
            $"Premerny odchod posledného zakaznika: {Double.Round(_globPriemernyOdchodPoslednehoZakaznika.Calucate(), 4)} / {TimeSpan.FromSeconds(Constants.START_DAY + _globPriemernyOdchodPoslednehoZakaznika.Calucate()).ToString(@"hh\:mm\:ss")}");
        Console.WriteLine($"Priemerny pocet zakaznikov: {Double.Round(_globPriemernyPocetZakaznikov.Calucate(), 4)}");
        Console.WriteLine($"Priemerny pocet obsluzenych zakaznikov: {Double.Round(_globPriemernyPocetObsluzenychZakaznikov.Calucate(), 4)}");
        Console.WriteLine($"Priemerne vytazenie automatu: {Double.Round(_globPriemerneVytazenieAutomatu.Calucate(), 4)*100}%");
        Console.WriteLine($"Priemerna dlzka radu pred obsluhou basic/zmluvny/online: {Double.Round(_globPriemernaDlzkaRaduPredObsluhouBasic.Calucate(), 4)}/{Double.Round(_globPriemernaDlzkaRaduPredObsluhouZmluvny.Calucate(), 4)}/{Double.Round(_globPriemernaDlzkaRaduPredObsluhouOnline.Calucate(), 4)}");
        StringBuilder sbPriemernaDlzkaRadu = new();
        StringBuilder sbPriemerneVytazeniePokladne = new();
        for (int i = 0; i < _pocetPokladni; i++)
        {
            sbPriemernaDlzkaRadu.Append($"[{Double.Round(_globPriemerneDlzkyRadovPredPokladnami[i].Calucate(), 4)}],");
            sbPriemerneVytazeniePokladne.Append($"[{Double.Round(_globPriemerneVytazeniePokladni[i].Calucate(), 4)*100}%],");
        }
        Console.WriteLine($"Priemerne dlzky radov pred pokladnami: {sbPriemernaDlzkaRadu.Remove(sbPriemernaDlzkaRadu.Length - 1, 1)}");
        Console.WriteLine($"Priemerne vytazenie pokladni: {sbPriemerneVytazeniePokladne.Remove(sbPriemerneVytazeniePokladne.Length - 1, 1)}");
        StringBuilder sbPriemerneVytazenieObsluhyOnline = new();
        foreach (var stat in _globPriemerneVytaznieObsluhyOnline)
        {
            sbPriemerneVytazenieObsluhyOnline.Append($"[{Double.Round(stat.Calucate(),4)*100:0.00}%],");
        }
        Console.WriteLine($"Priemerne vytazenie obsluhy online: {sbPriemerneVytazenieObsluhyOnline.Remove(sbPriemerneVytazenieObsluhyOnline.Length - 1, 1)}");
        StringBuilder sbPriemerneVytazenieObsluhyOstatne = new();
        foreach (var stat in _globPriemerneVytaznieObsluhyOstatne)
        {
            sbPriemerneVytazenieObsluhyOstatne.Append($"[{Double.Round(stat.Calucate(),4)*100:0.00}%],");
        }
        Console.WriteLine($"Priemerne vytazenie obsluhy ostatne: {sbPriemerneVytazenieObsluhyOstatne.Remove(sbPriemerneVytazenieObsluhyOstatne.Length - 1, 1)}");
        var interval = _globIntervalSpolahlivostiCasuVSysteme.Calculate();
        Console.WriteLine($"Interval spolahlivosti casu v systeme: [{interval.dolnaHranica} - {interval.hornaHranica}]");
        
        if (BehZavislosti)
        {
            Tick();
            OnUpdateData(_eventData);
            return;
        }
        Tick();
        _eventData.NewData = true;
        OnUpdateData(_eventData);
    }

    protected override void Tick()
    {
        if (BehZavislosti)
        {
            if (_eventData is null)
            {
                _eventData = new();
            }
            _eventData.AktuaReplikacia = _currentReplication.ToString();
            if (_globPriemernaDlzkaRadu.Count > 0)
            {
                _eventData.BehZavislostiPriemernyPocetZakaznikovPredAutomatom = Double.Round(_globPriemernaDlzkaRadu.Calucate(), 3);
            }
            else
            {
                _eventData.BehZavislostiPriemernyPocetZakaznikovPredAutomatom = 0;
            }
            return;
        }
        
        if (_eventData is null)
        {
            _eventData = new();
        }
        
        // update sim času aj keď nie sú nové dáta
        if (_eventData.ShallowUpdate)
        {
            _eventData.SimulationTime =
                $"{TimeSpan.FromSeconds(Constants.START_DAY + SimulationTime).ToString(@"hh\:mm\:ss")} / {TimeSpan.FromSeconds(Constants.START_DAY + END_OF_SIMULATION_TIME).ToString(@"hh\:mm\:ss")}";
        }

        if (_eventData.NewData)
        {
            _eventData.ShallowUpdate = SlowDown;
            if (_eventData.ShallowUpdate)
            {
                _eventData.People = Persons.Where(o => o.StavZakaznika != Constants.StavZakaznika.OdisielZPredajne)
                    .ToList();
                _eventData.RadaPredAutomatom = $"Rada pred automatom: {RadaPredAutomatom.Count}";
                _eventData.Automat = Automat;
                _eventData.RadaPredObsluznimiMiestamiOnline =
                    $"{RadaPredObsluznymMiestom.CountOnline}/{RadaPredObsluznymMiestom.Count}/{Constants.RADA_PRED_OBSLUZNYM_MIESTOM}";
                _eventData.RadaPredObsluznimiMiestamiBasic =
                    $"{RadaPredObsluznymMiestom.CountBasic}/{RadaPredObsluznymMiestom.Count}/{Constants.RADA_PRED_OBSLUZNYM_MIESTOM}";
                _eventData.RadaPredObsluznimiMiestamiZmluvny =
                    $"{RadaPredObsluznymMiestom.CountOstatne - RadaPredObsluznymMiestom.CountBasic}/{RadaPredObsluznymMiestom.Count}/{Constants.RADA_PRED_OBSLUZNYM_MIESTOM}";
                _eventData.ObsluzneMiestos = ObsluzneMiestoManager.GetInfoNaUI();
                _eventData.Pokladne = PokladnaManager.GetInfoNaUI();
            }

            _eventData.AktuaReplikacia = _currentReplication.ToString();

            if (_globPriemernyCasVObchode.Count > 0)
            {
                _eventData.PriemernyCasVObhchode =
                    $"{Double.Round(_globPriemernyCasVObchode.Calucate(), 3)}s / {TimeSpan.FromSeconds(_globPriemernyCasVObchode.Calucate()).ToString(@"hh\:mm\:ss")}";
            }
            else
            {
                _eventData.PriemernyCasVObhchode = "-/- / -:-:-";
            }

            if (_globCasStravenyPredAutomatom.Count > 0)
            {
                _eventData.PriemernyCasPredAutomatom =
                    $"{Double.Round(_globCasStravenyPredAutomatom.Calucate(), 3)}s / {TimeSpan.FromSeconds(_globCasStravenyPredAutomatom.Calucate()).ToString(@"hh\:mm\:ss")}";
            }
            else
            {
                _eventData.PriemernyCasPredAutomatom = "-/- / -:-:-";
            }

            if (_globPriemernaDlzkaRadu.Count > 0)
            {
                _eventData.PriemernaDlzkaraduPredAutomatom = $"{Double.Round(_globPriemernaDlzkaRadu.Calucate(), 3)}";
            }
            else
            {
                _eventData.PriemernaDlzkaraduPredAutomatom = "-/-";
            }

            if (_globPriemernyOdchodPoslednehoZakaznika.Count > 0)
            {
                _eventData.PriemernyOdchodPoslednehoZakaznika =
                    $"{Double.Round(_globPriemernyOdchodPoslednehoZakaznika.Calucate(), 3)} / {TimeSpan.FromSeconds(Constants.START_DAY + _globPriemernyOdchodPoslednehoZakaznika.Calucate()).ToString(@"hh\:mm\:ss")}";
            }
            else
            {
                _eventData.PriemernyOdchodPoslednehoZakaznika = "-/- / -:-:-";
            }

            if (_globPriemernyPocetZakaznikov.Count > 0)
            {
                _eventData.PriemernyPocetZakaznikov = $"{Double.Round(_globPriemernyPocetZakaznikov.Calucate(), 3)}";
            }
            else
            {
                _eventData.PriemernyPocetZakaznikov = "-/-";
            }
            if (_globPriemernyPocetObsluzenychZakaznikov.Count > 0)
            {
                _eventData.PriemernyPocetObsluzenychZakaznikov = $"{Double.Round(_globPriemernyPocetObsluzenychZakaznikov.Calucate(), 3)}";
            }
            else
            {
                _eventData.PriemernyPocetObsluzenychZakaznikov = "-/-";
            }
            if (_globPriemerneVytazenieAutomatu.Count > 0)
            {
                _eventData.PriemerneVytazenieAutomatu = $"{Double.Round(_globPriemerneVytazenieAutomatu.Calucate(), 4)*100:0.00}%";
                
            }
            else
            {
                _eventData.PriemerneVytazenieAutomatu = "-/-";
            }
            if (_globPriemernaDlzkaRaduPredObsluhouBasic.Count > 0 && _globPriemernaDlzkaRaduPredObsluhouZmluvny.Count > 0 && _globPriemernaDlzkaRaduPredObsluhouOnline.Count > 0)
            {
                _eventData.PriemerneDlzkyRadovPredObsluhov = $"[{Double.Round(_globPriemernaDlzkaRaduPredObsluhouBasic.Calucate(), 3)}],[{Double.Round(_globPriemernaDlzkaRaduPredObsluhouZmluvny.Calucate(), 3)}],[{Double.Round(_globPriemernaDlzkaRaduPredObsluhouOnline.Calucate(), 3)}]";
                
            }
            else
            {
                _eventData.PriemerneDlzkyRadovPredObsluhov = "[-/-], [-/-], [-/-]";
            }
            StringBuilder sbPriemernaDlkaRadu = new();
            StringBuilder sbPriemerneVytazeniePokladni = new();
            for (int i = 0; i < _pocetPokladni; i++)
            {
                if (_globPriemerneDlzkyRadovPredPokladnami[i].Count <= 0)
                {
                    sbPriemernaDlkaRadu.Append("[-/-],");
                    sbPriemerneVytazeniePokladni.Append("[-/-],");
                }
                else
                {
                    sbPriemernaDlkaRadu.Append($"[{Double.Round(_globPriemerneDlzkyRadovPredPokladnami[i].Calucate(), 3):0.000}],");
                    sbPriemerneVytazeniePokladni.Append($"[{Double.Round(_globPriemerneVytazeniePokladni[i].Calucate(), 4)*100:0.00}%],");
                }
            }
            _eventData.PriemerneDlzkyRadovPredPokladnami = sbPriemernaDlkaRadu.Remove(sbPriemernaDlkaRadu.Length - 1, 1).ToString();
            _eventData.PriemerneVytazeniePokladni = sbPriemerneVytazeniePokladni.Remove(sbPriemerneVytazeniePokladni.Length - 1, 1).ToString();
            StringBuilder sbPriemerneVytazenieObsluhyOnline = new();
            for (int i = 0; i < ObsluzneMiestoManager.ListObsluznychOnlineMiest.Count; i++)
            {
                if (_globPriemerneVytaznieObsluhyOnline[i].Count <= 0 )
                {
                    sbPriemerneVytazenieObsluhyOnline.Append("[-/-],");
                }
                else
                {
                    sbPriemerneVytazenieObsluhyOnline.Append($"[{Double.Round(_globPriemerneVytaznieObsluhyOnline[i].Calucate(),4)*100:0.00}%],");
                }
            }
            _eventData.PriemerneVytazenieObsluhyOnline = sbPriemerneVytazenieObsluhyOnline.Remove(sbPriemerneVytazenieObsluhyOnline.Length - 1, 1).ToString();
            StringBuilder sbPriemerneVytazenieObsluhyOstatne = new();
            for (int i = 0; i < ObsluzneMiestoManager.ListObsluznychOstatnyMiest.Count; i++)
            {
                if (_globPriemerneVytaznieObsluhyOstatne[i].Count <= 0 )
                {
                    sbPriemerneVytazenieObsluhyOstatne.Append("[-/-],");
                }
                else
                {
                    sbPriemerneVytazenieObsluhyOstatne.Append($"[{Double.Round(_globPriemerneVytaznieObsluhyOstatne[i].Calucate(),4)*100:0.00}%],");
                }
            }
            _eventData.PriemerneVytazenieObsluhyOstatne = sbPriemerneVytazenieObsluhyOstatne.Remove(sbPriemerneVytazenieObsluhyOstatne.Length - 1, 1).ToString();
            if (_globIntervalSpolahlivostiCasuVSysteme.Count >= 2)
            {
                var interval = _globIntervalSpolahlivostiCasuVSysteme.Calculate();
                _eventData.IntervalSpolahlivstiCasuVsysteme = $"{Double.Round(interval.dolnaHranica,3)/60:0.000} - {Double.Round(interval.hornaHranica,3)/60:0.000} / {TimeSpan.FromSeconds(interval.dolnaHranica).ToString(@"hh\:mm\:ss")} - {TimeSpan.FromSeconds(interval.hornaHranica).ToString(@"hh\:mm\:ss")}";
            }
            else
            {
                _eventData.IntervalSpolahlivstiCasuVsysteme = "[-/-] - [-/-] / [-/-] - [-/-]";
            }
        }
    }
}