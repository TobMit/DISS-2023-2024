using DISS_MonteCarloCore.Core;

namespace DISS_EventSimulationCore;

/// <summary>
/// Simulačné jadro pre eventovú simuláciu
/// </summary>
/// <typeparam name="T">sú triedy zákzaníkov</typeparam>
/// <typeparam name="TEventDataStructure">Dátova štruktúra ktorá sa vracia v evente</typeparam>
public abstract class EventSimulationCore<T, TEventDataStructure> : MonteCarloCore where TEventDataStructure : EventArgs
{
    public event EventHandler<TEventDataStructure> DataAvailable;
    public PriorityQueue<SimulationEvent<T, TEventDataStructure>, double> TimeLine { get; set; }

    public double SimulationTime { get; protected set; }

    protected EventSimulationCore(int numberOfReplications, int cutFirst) : base(numberOfReplications, cutFirst)
    {
    }

    public override void Replication()
    {
        while (TimeLine.Count > 0)
        {
            var tmpEvent = TimeLine.Dequeue();
            if (tmpEvent.EventTime < SimulationTime)
            {
                throw new ApplicationException(
                    "[Event Sim Core] - Čas v evente je nižší ako simulačný čas. Toto sa nemalo stať");
            }
            // čas sa musí posúvať pred eventom
            SimulationTime = tmpEvent.EventTime;

            tmpEvent.Execuete();
        }
    }

    /// <summary>
    /// Update UI z Core
    /// </summary>
    /// <param name="pEventData"></param>
    protected virtual void OnUpdateData(TEventDataStructure pEventData)
    {
        // ? Skontroluje či sú dostupné dáta
        DataAvailable?.Invoke(this, pEventData);
    }
}