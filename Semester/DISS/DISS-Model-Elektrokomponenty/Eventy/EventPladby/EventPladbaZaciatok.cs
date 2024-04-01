using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;
using DISS_Model_Elektrokomponenty.Entity.Pokladna;

namespace DISS_Model_Elektrokomponenty.Eventy.EventPladby;

public class EventPladbaZaciatok : SimulationEvent<Person, DataStructure>
{
    private Person _person;
    private Pokladna _pokladna;

    public EventPladbaZaciatok(EventSimulationCore<Person, DataStructure> pCore, 
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
        // ak je pokladňa obsadená hodim error
        if (_pokladna.Obsadena)
        {
            throw new InvalidOperationException($"[EventPladbaKoniec] - v čase {_core.SimulationTime} pokladňa {_pokladna.ID} je obsadená človekom ({_pokladna.Person?.ID})!");
        }
        
        // obsadim pokladňu
        _pokladna.ObsadPokladnu(_person);
        
        // naplánujem koniec pladby
        var newKoniecPladby = runCore.RndTrvaniePladba.Next() + _core.SimulationTime;
        _core.TimeLine.Enqueue(new EventPladbaKoniec(runCore, newKoniecPladby, _person, _pokladna), newKoniecPladby);
    }
}