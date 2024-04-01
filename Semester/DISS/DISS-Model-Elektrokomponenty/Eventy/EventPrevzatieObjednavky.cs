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
        if (_person.TypZakaznika == Constants.TypZakaznika.Online &&
            runCore.RadaPredObsluznymMiestom.CountOnline >= 1)
        {
            var person = runCore.RadaPredObsluznymMiestom.Dequeue(true);
            _person.ObsluzneMiesto.Obsluz(person);
            _core.TimeLine.Enqueue(new EventObsluhaZaciatok(runCore, _core.SimulationTime, person, _person.ObsluzneMiesto),
                _core.SimulationTime);
        }
        // to isté pre opačný ostatné typy zákazníka
        else if ((_person.TypZakaznika == Constants.TypZakaznika.Basic ||
                  _person.TypZakaznika == Constants.TypZakaznika.Zmluvny) &&
                 runCore.RadaPredObsluznymMiestom.CountOstatne >= 1)
        {
            var person = runCore.RadaPredObsluznymMiestom.Dequeue(false);
            _person.ObsluzneMiesto.Obsluz(person);
            _core.TimeLine.Enqueue(new EventObsluhaZaciatok(runCore, _core.SimulationTime, person, _person.ObsluzneMiesto),
                _core.SimulationTime);
        }
        // ak nie je nikto v rade tak tak nevytváram event
    }
}