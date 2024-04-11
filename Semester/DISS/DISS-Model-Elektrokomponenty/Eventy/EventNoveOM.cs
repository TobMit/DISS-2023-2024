using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;

namespace DISS_Model_Elektrokomponenty.Eventy;

public class EventNoveOM : SimulationEvent<Person, DataStructure>
{
    public EventNoveOM(EventSimulationCore<Person, DataStructure> pCore, double eventTime) : base(pCore, eventTime)
    {
    }

    public override void Execuete()
    {
        Core runCore = (Core)_core;
        if (_core._eventData != null) _core._eventData.NewData = true;
        runCore.ObsluzneMiestoManager.ListObsluznychOnlineMiest.Add(new(null,
            runCore.ObsluzneMiestoManager.ListObsluznychOnlineMiest.Count, runCore, true));
    }
}