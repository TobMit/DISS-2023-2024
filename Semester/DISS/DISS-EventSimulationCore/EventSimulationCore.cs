using DISS_MonteCarloCore.Core;

namespace DISS_EventSimulationCore;

/// <summary>
/// Simulačné jadro pre eventovú simuláciu
/// </summary>
/// <typeparam name="T">sú triedy zákzaníkov</typeparam>
public abstract class EventSimulationCore<T, TEvent> : MonteCarloCore where TEvent : EventArgs
{
    public event EventHandler<TEvent> DataAvailable;
    public PriorityQueue<SimulationEvent<T, TEvent>, double> TimeLine { get; set; }

    public double SimulationTime { get; protected set; }

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
                throw new ApplicationException(
                    "[Event Sim Core] - Čas v evente je nižší ako simulačný čas. Toto sa nemalo stať");
            }

            SimulationTime = tmpEvent.EventTime;

            tmpEvent.Execuete();
        }
    }

    /// <summary>
    /// Update UI z Core
    /// </summary>
    /// <param name="pEventData"></param>
    protected virtual void OnUpdateData(TEvent pEventData)
    {
        // ? Skontroluje či sú dostupné dáta
        DataAvailable?.Invoke(this, pEventData);
    }
}