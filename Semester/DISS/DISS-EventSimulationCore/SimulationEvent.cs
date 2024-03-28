namespace DISS_EventSimulationCore;

public abstract class SimulationEvent<T>
{
    private EventSimulationCore<T> _core;
    public double EventTime { get; private set; }

    public SimulationEvent(EventSimulationCore<T> pCore, double eventTime)
    {
        _core = pCore;
        EventTime = eventTime;
    }

    public abstract void Execuete();
}