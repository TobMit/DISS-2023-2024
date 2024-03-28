using DISS_MonteCarloCore.Core;

namespace DISS_EventSimulationCore;

/// <summary>
/// Simulačné jadro pre eventovú simuláciu
/// </summary>
/// <typeparam name="T">t tú triedy zákzaníkov</typeparam>
public abstract class EventSimulationCore<T> : MonteCarloCore
{
    
    public PriorityQueue<SimulationEvent<T>, double> TimeLine { get; set; }

    public double SimulationTime { get; set; }
    protected EventSimulationCore(int numberOfReplications, int cutFirst) : base(numberOfReplications, cutFirst)
    {
        TimeLine = new();
        
    }

    public override void Replication()
    {
        while (TimeLine.Count > 0)
        {
            var tmpEvent = TimeLine.Dequeue();
            if (tmpEvent.EventTime < SimulationTime)
            {
                throw new ApplicationException("[Event Sim Core] - Čas v evente je nižší ako simulačný čas. Toto sa nemalo stať");
            }

            SimulationTime = tmpEvent.EventTime;
            
            tmpEvent.Execuete();
        }
    }
}