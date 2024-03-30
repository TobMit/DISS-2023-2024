using System.Transactions;
using DISS_EventSimulationCore;
using DISS_HelperClasses.Statistic;
using DISS.Random.Other;

namespace DISS_NovinovyStanok.Simulation;

public class Core : EventSimulationCore<Person, DataStructure>
{
    /// <summary>
    /// Rad pred predajňou
    /// </summary>
    public Queue<Person> Queue { get; set; }

    public Average AvgPocetLudi { get; set; }
    public Average AvgCasVObchode { get; set; }
    public WeightedAverage AvgDlzkaRadu { get; set; }
    
    // globalne
    public Average GlobAvgPocetLudi { get; set; }
    public Average GlobAvgCasVObchode { get; set; }
    public Average GlobAvgDlzkaRadu { get; set; }
    
    
    //Generátory
    public Deterministic<int> obsluha;
    public Exponential prichodLudi;
    
    public Core(int numberOfReplications, int cutFirst) : base(numberOfReplications, cutFirst)
    {
        Queue = new();
        TimeLine = new ();
        AvgPocetLudi = new();
        AvgCasVObchode = new();
        AvgDlzkaRadu = new();

        GlobAvgDlzkaRadu = new();
        GlobAvgPocetLudi = new();
        GlobAvgCasVObchode = new();
    }

    public override void BeforeAllReplications()
    {
        obsluha = new(4);
        prichodLudi = new((60 / 12));
    }

    public override void BeforeReplication()
    {
        // vyčistenie radu
        Queue.Clear();
        SimulationTime = 0.0;
        TimeLine.Clear();
        
        AvgDlzkaRadu.Clear();
        AvgCasVObchode.Clear();
        AvgPocetLudi.Clear();
    }

    public override void AfterReplication()
    {
        GlobAvgDlzkaRadu.AddValue(AvgDlzkaRadu.Calucate());
        GlobAvgPocetLudi.AddValue(AvgPocetLudi.Calucate());
        GlobAvgCasVObchode.AddValue(AvgCasVObchode.Calucate());
    }

    public override void AfterAllReplications()
    {
        Console.WriteLine($"PrimernaDlzaRadu: {GlobAvgDlzkaRadu.Calucate()}");
        Console.WriteLine($"PriemerCasVObchode: {GlobAvgCasVObchode.Calucate()}");
        Console.WriteLine($"PriemerPocetLudi: {GlobAvgPocetLudi.Calucate()}");
    }
}