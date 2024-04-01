using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;

namespace DISS_Model_Elektrokomponenty.Eventy;

public class EventPrevzatieObjednavky : SimulationEvent<Person, DataStructure>
{
    private Person _person;

    public EventPrevzatieObjednavky(EventSimulationCore<Person, DataStructure> pCore, double eventTime, Person pPerson)
        : base(pCore, eventTime)
    {
        _person = pPerson;
    }

    public override void Execuete()
    {
        throw new NotImplementedException();
    }
}