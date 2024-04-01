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
    // Entity
    public Queue<Person> RadaPredAutomatom;
    public RadaPredObsluznymMiestom RadaPredObsluznymMiestom;
    public ObsluzneMiestoManager ObsluzneMiestoManager;
    public PokladnaManager PokladnaManager;
    public Automat Automat;

    private List<Person> _persons;

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

    public Core(int numberOfReplications, int cutFirst) : base(numberOfReplications, cutFirst)
    {
        RadaPredAutomatom = new();
        RadaPredObsluznymMiestom = new();
        ObsluzneMiestoManager = new();
        PokladnaManager = new();
        Automat = new();
        
        _persons = new();

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
        RndPickPokladna = new(ExtendedRandom<double>.NextSeed());
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
        
        _persons.Clear();

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
        
        OnUpdateData(_eventData);
    }

    public override void AfterAllReplications()
    {
        Console.WriteLine($"Priemerny cas v obchode: {_globPriemernyCasVObchode.Calucate()} / {_globPriemernyCasVObchode.Calucate()/60}");
        Console.WriteLine($"Cas straveny pred automatom: {_globCasStravenyPredAutomatom.Calucate()} / {_globCasStravenyPredAutomatom.Calucate()/60}");
        Console.WriteLine($"Priemerna dlzka radu: {_globPriemernaDlzkaRadu.Calucate()}");
        Console.WriteLine($"Premerny odchod posledného zakaznika: {_globPriemernyOdchodPoslednehoZakaznika.Calucate()} / {9.0 + _globPriemernyOdchodPoslednehoZakaznika.Calucate()/60/60}");
        Console.WriteLine($"Priemerny pocet zakaznikov: {_globPriemernyPocetZakaznikov.Calucate()}");
    }

    protected override void Tick()
    {
        _eventData = new DataStructure();
        _eventData.ShallowUpdate = !SlowDown;
        if (_eventData.ShallowUpdate)
        {
            _eventData.People = new(_persons.Count);
            foreach (var person in _persons)
            {
                _eventData.People.Add(new(person));
            }
            _eventData.RadaPredAutomatom = $"Rada pred automatom: {RadaPredAutomatom.Count}";
            _eventData.Automat = Automat.ToString();
            _eventData.RadaPredObsluznimiMiestami = $"Rada pred obslúžnymi miestami: {RadaPredObsluznymMiestom.Count}/8";
            _eventData.ObsluzneMiestos = ObsluzneMiestoManager.GetInfoNaUI();
            _eventData.Pokladne = PokladnaManager.GetInfoNaUI();
        }
        
        _eventData.PriemernyCasVObhchode = $"Priemerný čas v obchode: {StatPriemernyCasVObchode.Calucate()} / {StatPriemernyCasVObchode.Calucate()/60}";
        _eventData.PriemernyCasPredAutomatom = $"Priemerný čas pred automatom: {StatCasStravenyPredAutomatom.Calucate()} / {StatCasStravenyPredAutomatom.Calucate()/60}";
        _eventData.PriemernaDlzkaraduPredAutomatom = $"Priemerná dĺžka radu pred automatom: {StatPriemednaDlzakaRaduAutomatu.Calucate()}";
        _eventData.PriemernyOdchodPoslednehoZakaznika = $"Premerný odchod posledného zakaznika: {_globPriemernyOdchodPoslednehoZakaznika.Calucate()} / {9.0 + _globPriemernyOdchodPoslednehoZakaznika.Calucate()/60/60}";
        _eventData.PriemernyPocetZakaznikov = $"Priemerný počet zákazníkov: {_globPriemernyPocetZakaznikov.Calucate()}";
    }
}