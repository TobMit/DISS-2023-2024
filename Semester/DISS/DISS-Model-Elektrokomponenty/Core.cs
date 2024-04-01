using DISS_EventSimulationCore;
using DISS_HelperClasses.Statistic;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;
using DISS_Model_Elektrokomponenty.Entity.Pokladna;
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
    public RadaPredObsluznymMiestom RadaPredObsluznymMiestom;
    public ObsluzneMiestoManager ObsluzneMiestoManager;
    public PokladnaManager PokladnaManager;
    public Automat Automat;

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
    public UniformC RndTrvanieVyzdvyhnutieHardTovaru;

    // štatistiky
    public Average StatPriemernyCasVObchode;
    public Average StatCasStravenyPredAutomatom;
    public WeightedAverage StatPriemednaDlzakaRadu;

    // glob statistiky
    private Average GlobPriemernyCasVObchode;
    public Average GlobCasStravenyPredAutomatom;
    public Average GlobPriemernaDlzkaRadu;
    public Average GlobPremernyOdchodPoslednéhoZakaznika;

    public Core(int numberOfReplications, int cutFirst) : base(numberOfReplications, cutFirst)
    {
        RadaPredObsluznymMiestom = new();
        ObsluzneMiestoManager = new();
        PokladnaManager = new();
        Automat = new();

        StatPriemernyCasVObchode = new();
        StatCasStravenyPredAutomatom = new();
        StatPriemednaDlzakaRadu = new();

        GlobPriemernyCasVObchode = new();
        GlobCasStravenyPredAutomatom = new();
        GlobPriemernaDlzkaRadu = new();
        GlobPremernyOdchodPoslednéhoZakaznika = new();
    }

    public override void BeforeAllReplications()
    {
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
        RndTrvanieVyzdvyhnutieHardTovaru = new(30.0, 70.0, ExtendedRandom<double>.NextSeed());
    }

    public override void BeforeReplication()
    {
        SimulationTime = Constants.StartArrivalsSimulationTime;

        RadaPredObsluznymMiestom.Clear();
        ObsluzneMiestoManager.Clear();
        PokladnaManager.Clear();
        Automat.Clear();

        StatPriemernyCasVObchode.Clear();
        StatCasStravenyPredAutomatom.Clear();
        StatPriemednaDlzakaRadu.Clear();
    }

    public override void AfterReplication()
    {
        GlobPriemernyCasVObchode.AddValue(StatPriemernyCasVObchode.Calucate());
        GlobCasStravenyPredAutomatom.AddValue(StatCasStravenyPredAutomatom.Calucate());
        GlobPriemernaDlzkaRadu.AddValue(StatPriemednaDlzakaRadu.Calucate());
        GlobPremernyOdchodPoslednéhoZakaznika.AddValue(SimulationTime);
    }

    public override void AfterAllReplications()
    {
        Console.WriteLine($"Priemerny cas v obchode: {GlobPriemernyCasVObchode.Calucate()}");
        Console.WriteLine($"Cas straveny pred automatom: {GlobCasStravenyPredAutomatom.Calucate()}");
        Console.WriteLine($"Priemerna dlzka radu: {GlobPriemernaDlzkaRadu.Calucate()}");
        Console.WriteLine($"Premerny odchod posledného zakaznika: {GlobPremernyOdchodPoslednéhoZakaznika.Calucate()}");
    }
}