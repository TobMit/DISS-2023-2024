using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;

namespace DISS_Model_Elektrokomponenty.Eventy.EventyAutomat;

public class EventAutomatKoniec : SimulationEvent<Person, DataStructure>
{
    private Person _person;
    public EventAutomatKoniec(EventSimulationCore<Person, DataStructure> pCore, double eventTime, Person pPerson) : base(pCore, eventTime)
    {
        _person = pPerson;
    }

    public override void Execuete()
    {
        throw new NotImplementedException();
    }
}