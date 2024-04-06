using DISS_EventSimulationCore;
using DISS_NovinovyStanok.Simulation.Events.ObsluhaEventy;

namespace DISS_NovinovyStanok.Simulation.Events;

public class EventPrichod : SimulationEvent<Person, DataStructure>
{
    public EventPrichod(EventSimulationCore<Person, DataStructure> pCore, double eventTime) : base(pCore, eventTime)
    {
    }

    public override void Execuete()
    {
        Core runCore = (Core)_core;
        Person tmpPerson = new Person(){TimeOfArrival = EventTime, State = 0, ID = runCore.CountPocetLudi};
        runCore.CountPocetLudi++;
        // skontrolujeme či je prázdna queue
        // ak ano nie tak tak pridáme do queue ak áno plánujeme hneď event začatia obsluhy
        runCore.AvgDlzkaRadu.AddValue(runCore.Queue.Count);
        if (runCore.Queue.Count >= 1)
        {
            runCore.Queue.Enqueue(tmpPerson);
        }
        else if (runCore.obsluhovanyClovek)
        {
            runCore.Queue.Enqueue(tmpPerson);
        }
        else
        {
            runCore.TimeLine.Enqueue(new EventZaciatokObsluhy(runCore, _core.SimulationTime, tmpPerson), _core.SimulationTime);
        }

        var newArrival = runCore.prichodLudi.Next() + _core.SimulationTime;
        if (newArrival < Constants.simulationEndTime)
        {
            EventPrichod newEvent = new EventPrichod(_core, newArrival);
            _core.TimeLine.Enqueue(newEvent, newArrival);
        }
        runCore.AvgDlzkaRadu.AddValue(runCore.Queue.Count);
    }
}