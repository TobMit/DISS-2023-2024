using DISS_EventSimulationCore;

namespace DISS_NovinovyStanok.Simulation.Events.ObsluhaEventy;

public class EventKoniecObsluhy : SimulationEvent<Person, DataStructure>
{
    private Person _person;
    public EventKoniecObsluhy(EventSimulationCore<Person, DataStructure> pCore, double eventTime, Person pPerson) : base(pCore, eventTime)
    {
        _person = pPerson;
    }

    public override void Execuete()
    {
        Core runCore = (Core)_core;
        runCore.AvgCasVObchode.AddValue(runCore.SimulationTime - _person.TimeOfArrival);
        _person.State = 3;
        //Console.WriteLine($"[Clovek {_person.ID}]: cas: {runCore.SimulationTime} - Je obslúženy a odchádza");

        if (runCore.Queue.Count >= 1)
        {
            runCore.AvgDlzkaRadu.AddValue(runCore.Queue.Count);
            var tmpPerson = runCore.Queue.Dequeue();
            runCore.AvgDlzkaRadu.AddValue(runCore.Queue.Count);
            runCore.TimeLine.Enqueue(new EventZaciatokObsluhy(runCore,_core.SimulationTime, tmpPerson), _core.SimulationTime);
        }

        runCore.obsluhovanyClovek = false;
    }
}