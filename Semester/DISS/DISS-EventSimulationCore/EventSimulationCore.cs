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
    public TEventDataStructure? _eventData;
    
    public PriorityQueue<SimulationEvent<T, TEventDataStructure>, double> TimeLine { get; set; }
    public double SimulationTime { get; protected set; }
    public double END_OF_SIMULATION_TIME { get; set; }

    public bool SlowDown { get; set; }
    private bool generateSlowDownEvent;

    public bool Pause { get; set; }
    
    /// <summary>
    /// Interval spomalenia <0,1> kde 0 je maximálne spomalenie (1s) a 1 je 1H hodina
    /// </summary>
    public double SlowDownSpeed { get; set; }

    protected EventSimulationCore(int numberOfReplications, int cutFirst) : base(numberOfReplications, cutFirst)
    {
        SlowDown = false;
        SlowDownSpeed = 0.01;
        generateSlowDownEvent = SlowDown;
        _eventData = null;
        Pause = false;
    }

    public override void Replication()
    {
        while (TimeLine.Count > 0 && !_stop)
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

            // uplny update po každom evente
            if (SlowDown)
            {
                Tick();
            }
            
            // na generovanie spomalenia
            if (SlowDown && !generateSlowDownEvent)
            {
                generateSlowDownEvent = true;
                var newTime = 1 + (SlowDownSpeed - 0) * (3600 - 1) / (1 - 0);
                newTime += SimulationTime;
                TimeLine.Enqueue(new EventSlowDown<T, TEventDataStructure>(this, newTime), newTime);
            } 
            else if (!SlowDown && generateSlowDownEvent)
            {
                generateSlowDownEvent = false;
                
            }

            if (Pause)
            {
                Tick();
                if (_eventData != null) OnUpdateData(_eventData);
                while (Pause)
                {
                    Thread.Sleep(200);
                }
            }
        }
    }

    /// <summary>
    /// Update UI z Core
    /// </summary>
    /// <param name="pEventData"></param>
    public virtual void OnUpdateData(TEventDataStructure pEventData)
    {
        // ? Skontroluje či je listener na event
        DataAvailable?.Invoke(this, pEventData);
    }

    /// <summary>
    /// To Update data after event execution, (for data update)
    /// </summary>
    protected virtual void Tick()
    {
    }
}