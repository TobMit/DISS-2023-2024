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
        // ak nie je pokladňa obsadená hodim error
        if (!_pokladna.Obsadena)
        {
            throw new InvalidOperationException($"[EventPladbaKoniec] - v čase {_core.SimulationTime} pokladňa {_pokladna.ID} nie je obsadená!");
        }
        
        // ak je obsadená nesprávnym človekom
        if (_pokladna.Person.ID != _person.ID)
        {
            throw new InvalidOperationException($"[EventPladbaKoniec] - v čase {_core.SimulationTime} pokladňa {_pokladna.ID} je obsadená iným človekom ({_pokladna.Person.ID}) ako mala byť obsadená ({_person.ID})!");
        }
        
        // uvolním pokladňu
        _pokladna.UvolniPokladnu();
        
        // Ak má zakazni veľu objednávku naplánujem vyzdvyhnutie
        if (_person.TypVelkostiNakladu == Constants.TypVelkostiNakladu.Velka)
        {
            _person.StavZakaznika = Constants.StavZakaznika.ObsluzneMiestoVraciaSaPreVelkyTovar;
            var newVyzvihnutie = runCore.RndTrvanieVyzdvyhnutieVelkehoTovaru.Next() + _core.SimulationTime;
            _core.TimeLine.Enqueue(new EventPrevzatieObjednavky(runCore, newVyzvihnutie, _person), newVyzvihnutie);
        }
        
        // ak je v rade daľší človek naplánujem začiatok pladby
        if (_pokladna.Queue.Count >= 1)
        {
            var person = _pokladna.Queue.Dequeue();
            _core.TimeLine.Enqueue(new EventPladbaZaciatok(runCore, _core.SimulationTime, person, _pokladna), _core.SimulationTime);
        }
        // ak nie je tak je koniec pladby a zákazník odzcádza
        _person.StavZakaznika = Constants.StavZakaznika.OdisielZPredajne;
    }
    
}