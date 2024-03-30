using DISS_EventSimulationCore;

namespace DISS_NovinovyStanok.Simulation.Events.ObsluhaEventy;

public class EventZaciatokObsluhy : SimulationEvent<Person, DataStructure>
{
    private Person _person;
    public EventZaciatokObsluhy(EventSimulationCore<Person, DataStructure> pCore, double eventTime, Person pPerson) : base(pCore, eventTime)
    {
        _person = pPerson;
    }

    public override void Execuete()
    {
        _person.State = 2;
        
        Core runCore = (Core)_core;
        runCore.AvgDlzkaRadu.AddValue(runCore.Queue.Count, _core.SimulationTime); // štatistika
        
        // naplánovanie obsluhy
        
    }
}