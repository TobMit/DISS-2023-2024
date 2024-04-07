using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;

namespace DISS_Model_Elektrokomponenty.Eventy.EventyAutomat;

public class EventAutomatZaciatok : SimulationEvent<Person, DataStructure>
{
    private Person _person;
    public EventAutomatZaciatok(EventSimulationCore<Person, DataStructure> pCore, double eventTime, Person pPerson) : base(pCore, eventTime)
    {
        _person = pPerson;
    }

    public override void Execuete()
    {
        Core runCore = (Core)_core;
        if (_core._eventData != null) _core._eventData.NewData = true;
        // Automat musí byť obsadený
        if (!runCore.Automat.Obsadeny)
        {
            throw new InvalidOperationException($"[EventAutomatZačiatok] - v čase {_core.SimulationTime} automat nie je obsadený človekom!");
        }
        
        // obslúžime zákazníka
        runCore.StatCasStravenyPredAutomatom.AddValue(_core.SimulationTime - _person.TimeOfArrival);
        
        // naplánujeme event pre koniec obsluhy
        var newKoniecAutomat = runCore.RndTrvanieAutomatu.Next() + _core.SimulationTime;
        _core.TimeLine.Enqueue(new EventAutomatKoniec(runCore, newKoniecAutomat, _person), newKoniecAutomat);
    }
}