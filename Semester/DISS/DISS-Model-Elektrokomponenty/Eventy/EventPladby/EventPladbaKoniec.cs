using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;
using DISS_Model_Elektrokomponenty.Entity.Pokladna;

namespace DISS_Model_Elektrokomponenty.Eventy.EventPladby;

public class EventPladbaKoniec : SimulationEvent<Person, DataStructure>
{
    private Person _person;
    private Pokladna _pokladna;

    public EventPladbaKoniec(EventSimulationCore<Person, DataStructure> pCore, 
        double eventTime, 
        Person pPerson, 
        Pokladna pPokladna) : base(pCore, eventTime)
    {
        _person = pPerson;
        _pokladna = pPokladna;
    }

    public override void Execuete()
    {
        Core runCore = (Core)_core;
        if (_core._eventData != null) _core._eventData.NewData = true;
        // ak nie je pokladňa obsadená vyhodím error
        if (!_pokladna.Obsadena)
        {
            throw new InvalidOperationException($"[EventPlatbaKoniec] - v čase {_core.SimulationTime} pokladňa {_pokladna.ID} nie je obsadená!");
        }
        
        // ak je obsadená nesprávnym človekom
        if (_pokladna.Person.ID != _person.ID)
        {
            throw new InvalidOperationException($"[EventPlatbaKoniec] - v čase {_core.SimulationTime} pokladňa {_pokladna.ID} je obsadená iným človekom ({_pokladna.Person.ID}) ako mala byť obsadená ({_person.ID})!");
        }
        
        // uvoľním pokladňu
        _pokladna.UvolniPokladnu();
        
        // Ak má zákazník veľkú objednávku naplánujem vyzdvihnutie
        if (_person.TypVelkostiNakladu == Constants.TypVelkostiNakladu.Veľká)
        {
            _person.StavZakaznika = Constants.StavZakaznika.ObslužnéMiestoVraciaSaPreVeľkýTovar;
            var newVyzvihnutie = runCore.RndTrvanieVyzdvyhnutieVelkehoTovaru.Next() + _core.SimulationTime;
            _core.TimeLine.Enqueue(new EventPrevzatieObjednavky(runCore, newVyzvihnutie, _person), newVyzvihnutie);
        }
        else if (_person.TypVelkostiNakladu == Constants.TypVelkostiNakladu.Normálna)
        {
            // ak nie je tak je koniec platby a zákazník odchádza
            _person.StavZakaznika = Constants.StavZakaznika.OdišielZPredajne;
            runCore.StatPriemernyCasVObchode.AddValue(_core.SimulationTime - _person.TimeOfArrival);
        }
        
        // ak je v rade ďalší človek naplánujem začiatok platby
        if (_pokladna.Queue.Count >= 1)
        {
            var person = _pokladna.Queue.Dequeue();
            _pokladna.PriemernaDlzkaRadu.AddValue(runCore.SimulationTime, _pokladna.Queue.Count);
            _pokladna.ObsadPokladnu(person);
            _core.TimeLine.Enqueue(new EventPladbaZaciatok(runCore, _core.SimulationTime, person, _pokladna), _core.SimulationTime);
        }
    }
    
}