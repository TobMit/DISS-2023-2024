namespace DISS_EventSimulationCore;

public class EventSlowDown <T, TEvent> : SimulationEvent<T, TEvent> where TEvent : EventArgs
{
    public EventSlowDown(EventSimulationCore<T, TEvent> pCore, double eventTime) : base(pCore, eventTime)
    {
    }

    public override void Execuete()
    {
        // uspím vlákno
        Thread.Sleep(100);

        if (_core.SlowDown)
        {
            var newTime = 1 + (_core.SlowDownSpeed - 0) * (3600 - 1) / (1 - 0);
            newTime += EventTime;
            if (newTime < _core.END_OF_SIMULATION_TIME)
            {
                _core.TimeLine.Enqueue(new EventSlowDown<T, TEvent>(_core, newTime),newTime);
            }
        }
    }
}