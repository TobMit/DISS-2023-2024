using DISS_EventSimulationCore;

namespace DISS_NovinovyStanok.Simulation.Events;

public class EventPrichod : SimulationEvent<Person, DataStructure>
{
    public EventPrichod(EventSimulationCore<Person, DataStructure> pCore, double eventTime) : base(pCore, eventTime)
    {
    }

    public override void Execuete()
    {
        Person tmpPerson = new Person(){TimeOfArrival = EventTime, State = 0};
        // skontrolujeme či je prázdna queue
        // ak ano nie tak tak pridáme do queue ak áno plánujeme hneď event začatia obsluhy
        Core runCore = (Core)_core;
        if (runCore.Queue.Count >= 1)
        {
            runCore.Queue.Enqueue(tmpPerson);
            runCore.AvgDlzkaRadu.AddValue(runCore.Queue.Count, _core.SimulationTime);
        }
        else
        {
            // naplánovanie obsluhy
        }

        var newArrival = runCore.prichodLudi.Next();
        if (newArrival < Constants.simulationEndTime)
        {
            EventPrichod newEvent = new EventPrichod(_core, newArrival);
            _core.TimeLine.Enqueue(newEvent, newArrival);
        }
    }
}