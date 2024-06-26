namespace DISS_EventSimulationCore;

/// <summary>
/// Udalosť v simulácií
/// </summary>
/// <typeparam name="T">sú triedy zákazníkov</typeparam>
/// <typeparam name="TEvent">Je typ eventu</typeparam>
public abstract class SimulationEvent<T, TEvent> where TEvent : EventArgs 
{
    protected EventSimulationCore<T, TEvent> _core;
    public double EventTime { get; private set; }

    public SimulationEvent(EventSimulationCore<T, TEvent> pCore, double eventTime)
    {
        _core = pCore;
        EventTime = eventTime;
    }

    /// <summary>
    /// Vykonanie eventu
    /// </summary>
    public abstract void Execuete();
}