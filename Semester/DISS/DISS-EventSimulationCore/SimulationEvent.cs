namespace DISS_EventSimulationCore;

/// <summary>
/// Udalosť v simulácií
/// </summary>
/// <typeparam name="T">sú triedy zákzaníkov</typeparam>
/// <typeparam name="TEvent">Je typ eventu</typeparam>
public abstract class SimulationEvent<T, TEvent> where TEvent : EventArgs
{
    private EventSimulationCore<T, TEvent> _core;
    public double EventTime { get; private set; }

    public SimulationEvent(EventSimulationCore<T, TEvent> pCore, double eventTime)
    {
        _core = pCore;
        EventTime = eventTime;
    }

    public abstract void Execuete();
}