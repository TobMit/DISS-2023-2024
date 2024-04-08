using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;

namespace DISS_Model_Elektrokomponenty.Eventy;

public class EventKoniecRady : SimulationEvent<Person, DataStructure>
{
    public EventKoniecRady(EventSimulationCore<Person, DataStructure> pCore, double eventTime) : base(pCore, eventTime)
    {
    }

    public override void Execuete()
    {
        Core runCore = (Core)_core;
        if (_core._eventData != null) _core._eventData.NewData = true;
        // ak je po, tak ľudia pred automatom odídu
        runCore.Automat.PocetObsluzenych = runCore.Automat.CelkovyPocet - runCore.RadaPredAutomatom.Count;
        while (runCore.RadaPredAutomatom.Count >= 1)
        {
            var leavePerson = runCore.RadaPredAutomatom.Dequeue();
            leavePerson.StavZakaznika = Constants.StavZakaznika.OdišielZPredajne;
        }
    }
}