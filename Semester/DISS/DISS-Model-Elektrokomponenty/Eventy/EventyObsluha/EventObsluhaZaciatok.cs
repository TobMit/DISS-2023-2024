using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;

namespace DISS_Model_Elektrokomponenty.Eventy.EventyObsluha;

public class EventObsluhaZaciatok : SimulationEvent<Person, DataStructure>
{
    private Person _person;
    private ObsluzneMiesto _obsluzneMiesto;
    public EventObsluhaZaciatok(EventSimulationCore<Person, DataStructure> pCore, 
        double eventTime,
        Person pPerson,
        ObsluzneMiesto pObsluzneMiesto) : base(pCore, eventTime)
    {
        _person = pPerson;
        _obsluzneMiesto = pObsluzneMiesto;
    }

    public override void Execuete()
    {
        throw new NotImplementedException();
    }
}