using System.Transactions;
using DISS_EventSimulationCore;
using DISS_HelperClasses.Statistic;
using DISS_NovinovyStanok.Simulation.Events;
using DISS.Random;
using DISS.Random.Other;

namespace DISS_NovinovyStanok.Simulation;

public class Core : EventSimulationCore<Person, DataStructure>
{
    /// <summary>
    /// Rad pred predajňou
    /// </summary>
    public Queue<Person> Queue { get; set; }

    public int CountPocetLudi {
        get;
        set;
    }
    public Average AvgCasVObchode { get; set; }
    public WeightedAverage AvgDlzkaRadu { get; set; }
    
    // globalne
    public Average GlobAvgPocetLudi { get; set; }
    public Average GlobAvgCasVObchode { get; set; }
    public Average GlobAvgDlzkaRadu { get; set; }

    public bool obsluhovanyClovek { get; set; }
    
    
    //Generátory
    public Deterministic<int> obsluha;
    public Exponential prichodLudi;
    
    public Core(int numberOfReplications, int cutFirst) : base(numberOfReplications, cutFirst)
    {
        Queue = new();
        TimeLine = new ();
        CountPocetLudi = new();
        AvgCasVObchode = new();
        AvgDlzkaRadu = new();

        GlobAvgDlzkaRadu = new();
        GlobAvgPocetLudi = new();
        GlobAvgCasVObchode = new();
    }

    public override void BeforeAllReplications()
    {
        obsluha = new(4);
        prichodLudi = new((60 / 12), ExtendedRandom<double>.NextSeed());
    }

    public override void BeforeReplication()
    {
        // vyčistenie radu
        Queue.Clear();
        SimulationTime = 0.0;
        TimeLine.Clear();
        
        AvgDlzkaRadu.Clear();
        AvgCasVObchode.Clear();
        CountPocetLudi = 0;

        obsluhovanyClovek = false;
        
        // Naplánovanie prvého príchodu
        var FirstArrival = prichodLudi.Next() + SimulationTime;
        TimeLine.Enqueue(new EventPrichod(this, FirstArrival), FirstArrival);
    }

    public override void AfterReplication()
    {
        GlobAvgPocetLudi.AddValue(CountPocetLudi);
        GlobAvgDlzkaRadu.AddValue(AvgDlzkaRadu.Calucate());
        GlobAvgCasVObchode.AddValue(AvgCasVObchode.Calucate());
    }

    public override void AfterAllReplications()
    {
        Console.WriteLine($"PrimernaDlzaRadu: {GlobAvgDlzkaRadu.Calucate()}");
        Console.WriteLine($"PriemerCasVObchode: {GlobAvgCasVObchode.Calucate()}");
        Console.WriteLine($"PriemerPocetLudi: {GlobAvgPocetLudi.Calucate()}");
    }
}