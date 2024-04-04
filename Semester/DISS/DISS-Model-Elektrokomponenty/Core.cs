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

    public Core(int numberOfReplications, int cutFirst, int pPocetObsluznychMiest, int pPocetPokladni) : base(numberOfReplications, cutFirst)
    {
        _pocetObsluznychMiest = pPocetObsluznychMiest;
        _pocetPokladni = pPocetPokladni;
        
        RadaPredAutomatom = new();
        RadaPredObsluznymMiestom = new();
        ObsluzneMiestoManager = new(_pocetObsluznychMiest);
        PokladnaManager = new(_pocetPokladni);
        Automat = new();
        
        Persons = new();

        StatPriemernyCasVObchode = new();
        StatCasStravenyPredAutomatom = new();
        StatPriemednaDlzakaRaduAutomatu = new();

        _globPriemernyCasVObchode = new();
        _globCasStravenyPredAutomatom = new();
        _globPriemernaDlzkaRadu = new();
        _globPriemernyOdchodPoslednehoZakaznika = new();
        _globPriemernyPocetZakaznikov = new();
    }

    public override void BeforeAllReplications()
    {
        END_OF_SIMULATION_TIME = Constants.END_SIMULATION_TIME;
        
        TimeLine = new();
        ObsluzneMiestoManager.InitObsluzneMiesta();
        PokladnaManager.InitPokladne();

        // Rozdelenia pravdepodobnosti
        RndPickPokladna = new(ExtendedRandom<double>.NextSeed(), _pocetObsluznychMiest);
        // (60*60) / 30 lebo je to 30 zakaznikov za hodinu ale systém beží v sekunádch tak preto ten prepočet
        RndPrichodZakaznika = new(((60.0 * 60.0) / 30.0), ExtendedRandom<double>.NextSeed());
        RndTypZakaznika = new(0, 1, ExtendedRandom<double>.NextSeed());
        RndTypNarocnostTovaru = new(0, 1.0, ExtendedRandom<double>.NextSeed());
        RndTypVelkostiNakladu = new(0, 1.0, ExtendedRandom<double>.NextSeed());
        RndTrvanieAutomatu = new(30.0, 180.0, ExtendedRandom<double>.NextSeed());
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
        _globPriemernaDlzkaRadu.AddValue(StatPriemednaDlzakaRaduAutomatu.Calucate());
        _globPriemernyOdchodPoslednehoZakaznika.AddValue(SimulationTime);
        _globPriemernyPocetZakaznikov.AddValue(Automat.CelkovyPocet);
        if (_currentReplication % 100 == 0)
        {
            Tick();
            OnUpdateData(_eventData);
        }
    }

    public override void AfterAllReplications()
    {
        Console.WriteLine($"Priemerny cas v obchode: {Double.Round(_globPriemernyCasVObchode.Calucate(), 4)}s / {TimeSpan.FromSeconds(_globPriemernyCasVObchode.Calucate()).ToString(@"hh\:mm\:ss")}");
        Console.WriteLine($"Cas straveny pred automatom: {Double.Round(_globCasStravenyPredAutomatom.Calucate(), 4)}s / {TimeSpan.FromSeconds(_globCasStravenyPredAutomatom.Calucate()).ToString(@"hh\:mm\:ss")}");
        Console.WriteLine($"Priemerna dlzka radu: {Double.Round(_globPriemernaDlzkaRadu.Calucate(), 4)}");
        Console.WriteLine($"Premerny odchod posledného zakaznika: {Double.Round(_globPriemernyOdchodPoslednehoZakaznika.Calucate(), 4)} / {TimeSpan.FromSeconds(Constants.START_DAY + _globPriemernyOdchodPoslednehoZakaznika.Calucate()).ToString(@"hh\:mm\:ss")}");
        Console.WriteLine($"Priemerny pocet zakaznikov: {Double.Round(_globPriemernyPocetZakaznikov.Calucate(), 4)}");
        Tick();
        OnUpdateData(_eventData);
    }

    protected override void Tick()
    {
        if (_eventData is null)
        {
            _eventData = new();
        }
        _eventData.ShallowUpdate = SlowDown;
        if (_eventData.ShallowUpdate)
        {
            _eventData.SimulationTime = $"{TimeSpan.FromSeconds(Constants.START_DAY + SimulationTime).ToString(@"hh\:mm\:ss")} / {TimeSpan.FromSeconds(Constants.START_DAY + END_OF_SIMULATION_TIME).ToString(@"hh\:mm\:ss")}";
            _eventData.People = new(Persons.Count);
            foreach (var person in Persons)
            {
                if (person.StavZakaznika == Constants.StavZakaznika.OdisielZPredajne)
                {
                    continue;
                }
                
                //todo odstrániť iba na zrýchlenie UI
                if (_eventData.People.Count > 25)
                {
                    break;
                }
                _eventData.People.Add(new(person));
            }
            _eventData.RadaPredAutomatom = $"Rada pred automatom: {RadaPredAutomatom.Count}";
            _eventData.Automat = Automat.ToString();
            _eventData.RadaPredObsluznimiMiestamiOnline = $"{RadaPredObsluznymMiestom.CountOnline}/{RadaPredObsluznymMiestom.Count}/{Constants.RADA_PRED_OBSLUZNYM_MIESTOM}";
            _eventData.RadaPredObsluznimiMiestamiBasic = $"{RadaPredObsluznymMiestom.CountBasic}/{RadaPredObsluznymMiestom.Count}/{Constants.RADA_PRED_OBSLUZNYM_MIESTOM}";
            _eventData.RadaPredObsluznimiMiestamiZmluvny = $"{RadaPredObsluznymMiestom.CountOstatne - RadaPredObsluznymMiestom.CountBasic}/{RadaPredObsluznymMiestom.Count}/{Constants.RADA_PRED_OBSLUZNYM_MIESTOM}";
            _eventData.ObsluzneMiestos = ObsluzneMiestoManager.GetInfoNaUI();
            _eventData.Pokladne = PokladnaManager.GetInfoNaUI();
        }

        _eventData.AktuaReplikacia = _currentReplication.ToString();
        
        if (_globPriemernyCasVObchode.Count > 0)
        {
            _eventData.PriemernyCasVObhchode = $"{Double.Round(_globPriemernyCasVObchode.Calucate(), 3)}s / {TimeSpan.FromSeconds(_globPriemernyCasVObchode.Calucate()).ToString(@"hh\:mm\:ss")}";
        }
        else
        {
            _eventData.PriemernyCasVObhchode = "-/- / -:-:-";
        }

        if (_globCasStravenyPredAutomatom.Count > 0)
        {
            _eventData.PriemernyCasPredAutomatom = $"{Double.Round(_globCasStravenyPredAutomatom.Calucate(), 3)}s / {TimeSpan.FromSeconds(_globCasStravenyPredAutomatom.Calucate()).ToString(@"hh\:mm\:ss")}";
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
            _eventData.PriemernyOdchodPoslednehoZakaznika = $"{Double.Round(_globPriemernyOdchodPoslednehoZakaznika.Calucate(), 3)} / {TimeSpan.FromSeconds(Constants.START_DAY + _globPriemernyOdchodPoslednehoZakaznika.Calucate()).ToString(@"hh\:mm\:ss")}";
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
    }
}