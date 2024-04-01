using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;

namespace DISS_Model_Elektrokomponenty.Eventy.EventyObsluha;

public class EventKoniecDiktovania : SimulationEvent<Person, DataStructure>
{
    private ObsluzneMiesto _obsluzneMiesto;
    private Person _person;

    public EventKoniecDiktovania(EventSimulationCore<Person, DataStructure> pCore, 
        double eventTime,
        Person pPerson,
        ObsluzneMiesto pObsluzneMiesto) : base(pCore, eventTime)
    {
        _obsluzneMiesto = pObsluzneMiesto;
        _person = pPerson;
    }

    public override void Execuete()
    {
        throw new NotImplementedException();
    }
}