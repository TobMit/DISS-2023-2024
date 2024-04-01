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
        
        // ak niekoho obsluhuje tak vyhodíme error to sa nemôže stať
        if (runCore.Automat.Obsadeny)
        {
            throw new InvalidOperationException($"[EventAutomatZaciatok] - v čase {_core.SimulationTime} automat je obsadený človekom {runCore.Automat.Person?.ID}!");
        }
        
        // obslúžime zákazníka
        runCore.Automat.Obsluz(_person);
        
        // naplánujeme event pre koniec obsluhy
        var newKoniecAutomat = runCore.RndTrvanieAutomatu.Next() + _core.SimulationTime;
        _core.TimeLine.Enqueue(new EventAutomatKoniec(runCore, newKoniecAutomat, _person), newKoniecAutomat);
    }
}