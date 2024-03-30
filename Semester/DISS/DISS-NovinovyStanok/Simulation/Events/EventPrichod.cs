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
        //Console.WriteLine($"[Clovek {tmpPerson.ID}]: cas: {runCore.SimulationTime} - vosiel do miestnosti");
        // skontrolujeme či je prázdna queue
        // ak ano nie tak tak pridáme do queue ak áno plánujeme hneď event začatia obsluhy
        if (runCore.Queue.Count >= 1)
        {
            runCore.AvgDlzkaRadu.AddValue(runCore.Queue.Count, _core.SimulationTime);
            runCore.Queue.Enqueue(tmpPerson);
            runCore.AvgDlzkaRadu.AddValue(runCore.Queue.Count, _core.SimulationTime);
            //Console.WriteLine($"[Clovek {tmpPerson.ID}]: cas: {runCore.SimulationTime} - Postavil sa do radu, pocet ludi v rade {runCore.Queue.Count}");
        }
        else if (runCore.obsluhovanyClovek)
        {
            runCore.AvgDlzkaRadu.AddValue(runCore.Queue.Count, _core.SimulationTime);
            runCore.Queue.Enqueue(tmpPerson);
            runCore.AvgDlzkaRadu.AddValue(runCore.Queue.Count, _core.SimulationTime);
            //Console.WriteLine($"[Clovek {tmpPerson.ID}]: cas: {runCore.SimulationTime} - Postavil sa do radu pretože bol niekto obsluhovany, pocet ludi v rade {runCore.Queue.Count}");
        }
        else
        {
            runCore.TimeLine.Enqueue(new EventZaciatokObsluhy(runCore, _core.SimulationTime, tmpPerson), _core.SimulationTime);
            //Console.WriteLine($"[Clovek {tmpPerson.ID}]: cas: {runCore.SimulationTime} - Postavil je pred pokladňou, nikto nebol obsluhovaný");
            //Console.WriteLine($"[Clovek {tmpPerson.ID}]: cas: {runCore.SimulationTime} - Prisiel pred pokladnu");
        }

        var newArrival = runCore.prichodLudi.Next() + _core.SimulationTime;
        if (newArrival < Constants.simulationEndTime)
        {
            EventPrichod newEvent = new EventPrichod(_core, newArrival);
            _core.TimeLine.Enqueue(newEvent, newArrival);
        }
    }
}