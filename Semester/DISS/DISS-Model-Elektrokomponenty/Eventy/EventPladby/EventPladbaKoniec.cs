using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;
using DISS_Model_Elektrokomponenty.Entity.Pokladna;

namespace DISS_Model_Elektrokomponenty.Eventy.EventPladby;

public class EventPladbaKoniec : SimulationEvent<Person, DataStructure>
{
    private Person _person;
    private Pokladna _pokladna;

    public EventPladbaKoniec(EventSimulationCore<Person, DataStructure> pCore, 
        double eventTime, 
        Person pPerson, 
        Pokladna pPokladna) : base(pCore, eventTime)
    {
        _person = pPerson;
        _pokladna = pPokladna;
    }

    public override void Execuete()
    {
throw new NotImplementedException();
    }
    
}