using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;
using DISS_Model_Elektrokomponenty.Eventy.EventyAutomat;

namespace DISS_Model_Elektrokomponenty.Eventy.EventyObsluha;

public class EventObsluhaZaciatok : SimulationEvent<Person, DataStructure>
{
    private Person _person;
    private ObsluzneMiesto _obsluzneMiesto;
    public EventObsluhaZaciatok(EventSimulationCore<Person, DataStructure> pCore, 
        double eventTime,
        Person pPerson,
        ObsluzneMiesto pObsluzneMiesto) : base(pCore, eventTime)
    {
        _person = pPerson;
        _obsluzneMiesto = pObsluzneMiesto;
    }

    public override void Execuete()
    {
        Core runCore = (Core)_core;
        // ak je obsluzne miesto obsadené tak hodíme error
        if (_obsluzneMiesto.Obsadena)
        {
            throw new InvalidOperationException($"[EventObsluhaZaciatok] - v čase {_core.SimulationTime} obsluzne miesto {_obsluzneMiesto.ID} je obsadené človekom ({_obsluzneMiesto.Person.ID})!");
        }
        
        // Event začiatok automatu
        // skontrolujeme či sa nachádza niekto v rade pred automatom a či je automat prázdny ak áno vytvárame event začiatku automatu
        if (runCore.RadaPredAutomatom.Count >= 1 && !runCore.Automat.Obsadeny)
        {
            var person = runCore.RadaPredAutomatom.Dequeue();
            _core.TimeLine.Enqueue(new EventAutomatZaciatok(_core, _core.SimulationTime, person), _core.SimulationTime);
        }
        
        // Obsluha
        // nastavíme obslužné miesto ako obsadené
        _obsluzneMiesto.Obsluz(_person);
        
        // Ak je online Tak naplánujeme eventKoniecObsluhy
        if (_person.TypZakaznika == Constants.TypZakaznika.Online)
        {
            var newKoniecObsluhy = runCore.RndTrvanieOnlinePripravaTovaru.Next() + _core.SimulationTime;
            _core.TimeLine.Enqueue(new EventObsluhaKoniec(_core, newKoniecObsluhy, _person, _obsluzneMiesto), newKoniecObsluhy);
        }
        
        // Ak je ostatné tak naplánujeme EventKoniecDiktovania
        else if (_person.TypZakaznika == Constants.TypZakaznika.Basic || _person.TypZakaznika == Constants.TypZakaznika.Zmluvny)
        {
            var newKoniecDiktovania = runCore.RndTrvanieDiktovania.Next() + _core.SimulationTime;
            _core.TimeLine.Enqueue(new EventKoniecDiktovania(_core, newKoniecDiktovania, _person, _obsluzneMiesto), newKoniecDiktovania);
        }
        // ak nastala iná situácia hodím error
        else
        {
            throw new InvalidOperationException($"[EventObsluhaZaciatok] - v čase {_core.SimulationTime} nastala neznáma situácia!");
        }
    }
}