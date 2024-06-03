namespace DISS_EventSimulationCore;

public class EventSlowDown <T, TEvent> : SimulationEvent<T, TEvent> where TEvent : EventArgs
{
    public EventSlowDown(EventSimulationCore<T, TEvent> pCore, double eventTime) : base(pCore, eventTime)
    {
    }

    public override void Execuete()
    {
        // uspím vlákno
        Thread.Sleep(1000 / _core.POCET_UPDATOV_ZA_SEKUNDU);

        if (_core.SlowDown)
        {
            var newTime = _core.SlowDownSpeed / _core.POCET_UPDATOV_ZA_SEKUNDU;
            newTime += EventTime;
            if (_core.TimeLine.Count != 0)
            {
                _core.TimeLine.Enqueue(new EventSlowDown<T, TEvent>(_core, newTime),newTime);
            }
            
            //ak sú dáta tak ich dám dispacherovi
            if (_core._eventData is not null)
            {
                _core.OnUpdateData(_core._eventData);
            }
        }
    }
}