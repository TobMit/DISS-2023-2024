using System.Transactions;
using DISS_EventSimulationCore;

namespace DISS_NovinovyStanok.Simulation;

public class Core : EventSimulationCore<Person, DataStructure>
{
    /// <summary>
    /// Rad pred predajňou
    /// </summary>
    public Queue<Person> Queue { get; set; }
    
    public Core(int numberOfReplications, int cutFirst) : base(numberOfReplications, cutFirst)
    {
        Queue = new();
        TimeLine = new();
    }

    public override void BeforeAllReplications()
    {
        throw new NotImplementedException();
    }

    public override void BeforeReplication()
    {
        // vyčistenie radu
        Queue.Clear();
        SimulationTime = 0.0;
        TimeLine.Clear();
    }

    public override void AfterReplication()
    {
        throw new NotImplementedException();
    }

    public override void AfterAllReplications()
    {
        throw new NotImplementedException();
    }
}