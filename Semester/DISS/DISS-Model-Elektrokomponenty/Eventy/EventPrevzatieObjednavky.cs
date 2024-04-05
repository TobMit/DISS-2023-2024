using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;
using DISS_Model_Elektrokomponenty.Eventy.EventyObsluha;

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
        Core runCore = (Core)_core;
        if (_core._eventData != null) _core._eventData.NewData = true;
        // ak zákazník nemá veľkú objednávku tak vyhodím error
        if (_person.TypVelkostiNakladu != Constants.TypVelkostiNakladu.Velka)
        {
            throw new InvalidOperationException($"[EventPrevzatieObjednavky] - v čase {_core.SimulationTime} zákazník {_person.ID} nemá veľkú objednávku!");
        }
        
        // ak zákazník nemá obslužné miesto hodím error
        if (_person.ObsluzneMiesto == null)
        {
            throw new InvalidOperationException($"[EventPrevzatieObjednavky] - v čase {_core.SimulationTime} zákazník {_person.ID} nemá obslužné miesto!");
        }
        
        // uvolním oblužné miesto
        _person.ObsluzneMiesto.Uvolni();
        _person.StavZakaznika = Constants.StavZakaznika.OdisielZPredajne;
        runCore.StatPriemernyCasVObchode.AddValue(_core.SimulationTime - _person.TimeOfArrival);
        
        // planovanie začiatku obsluhy
        // ak bol zákazník online a v rade pre online sú ludia, tak naplánujem začiatok obsluhy
        ObsluzneMiesto? tmpObsluzneMiesto = runCore.ObsluzneMiestoManager.GetVolneOnline();
        
        while (runCore.RadaPredObsluznymMiestom.CountOnline >= 1 && tmpObsluzneMiesto is not null)
        {
            var person = runCore.RadaPredObsluznymMiestom.Dequeue(true);
            tmpObsluzneMiesto.Obsluz(person);
            _core.TimeLine.Enqueue(new EventObsluhaZaciatok(runCore, _core.SimulationTime, person, tmpObsluzneMiesto),
                _core.SimulationTime);
            tmpObsluzneMiesto = runCore.ObsluzneMiestoManager.GetVolneOnline();
        }
        // to isté pre opačný ostatné typy zákazníka
        tmpObsluzneMiesto = runCore.ObsluzneMiestoManager.GetVolneOstatne();
        while (runCore.RadaPredObsluznymMiestom.CountOstatne >= 1 && tmpObsluzneMiesto is not null)
        {
            var person = runCore.RadaPredObsluznymMiestom.Dequeue(false);
            tmpObsluzneMiesto.Obsluz(person);
            _core.TimeLine.Enqueue(new EventObsluhaZaciatok(runCore, _core.SimulationTime, person, tmpObsluzneMiesto),
                _core.SimulationTime);
            tmpObsluzneMiesto = runCore.ObsluzneMiestoManager.GetVolneOstatne();
        }
        // ak nie je nikto v rade tak tak nevytváram event
    }
}