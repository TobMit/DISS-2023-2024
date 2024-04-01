using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;
using DISS_Model_Elektrokomponenty.Eventy.EventyObsluha;

namespace DISS_Model_Elektrokomponenty.Eventy.EventyAutomat;

public class EventAutomatKoniec : SimulationEvent<Person, DataStructure>
{
    private Person _person;
    public EventAutomatKoniec(EventSimulationCore<Person, DataStructure> pCore, double eventTime, Person pPerson) : base(pCore, eventTime)
    {
        _person = pPerson;
    }

    public override void Execuete()
    {
        Core runCore = (Core)_core;
        
        // ak nikto nie je obsluhovaný v automate tak hodíme error to nemôže nastať
        if (!runCore.Automat.Obsadeny)
        {
            throw new InvalidOperationException($"[EventAutomatKoniec] - v čase {_core.SimulationTime} nikto nie je obsluhovaný!");
        }
        
        // ak obsluhuje iný človek ako ten ktorý mal byť obsluhovaný tak hodíme error
        if (runCore.Automat.Person.ID != _person.ID)
        {
            throw new InvalidOperationException($"[EventAutomatKoniec] - v čase {_core.SimulationTime} obsluhuje sa iný človek ({runCore.Automat.Person.ID}) ako mal byť obsluhovaný ({_person.ID})!");
        }
        
        // uvoľníme automat
        runCore.Automat.Uvolni();
        
        // naplánujeme evet pre začiatok automatu
        if (runCore.RadaPredAutomatom.Count >= 1)
        {
            var person = runCore.RadaPredAutomatom.Dequeue();
            _core.TimeLine.Enqueue(new EventAutomatZaciatok(runCore, _core.SimulationTime, person), _core.SimulationTime);
        }
        
        // naplánujeme event pre začiatok obsluhy alebo postavenie do rady
        // ak je rada pred obslužnym miestom väčšia ako 8 je to chyba hodíme error
        if (runCore.RadaPredObsluznymMiestom.Count > 8)
        {
            throw new InvalidOperationException($"[EventAutomatKoniec] - v čase {_core.SimulationTime} je rada pred obslužnym miestom väčšia ako 8!");
        }
        // ak sa nachádza niekto v rade tak pridáme človeka do radu
        if (runCore.RadaPredObsluznymMiestom.Count >= 1)
        {
            runCore.RadaPredObsluznymMiestom.Enqueue(_person);
        }
        // ak je typ zákzaníka online tak hľadáme neobslúženú obsluzne miesto pre online zákazníka
        else if (_person.TypZakaznika == Constants.TypZakaznika.Online)
        {
            var obsluzneMiesto = runCore.ObsluzneMiestoManager.GetVolneOnline();
            // ak je obsluzne miesto volne tak vytvoríme event pre obsluzne miesto
            if (obsluzneMiesto is not null)
            {
                _core.TimeLine.Enqueue(new EventObsluhaZaciatok(runCore, _core.SimulationTime, _person, obsluzneMiesto), _core.SimulationTime);
            }
            // inak pridáme do radu
            else
            {
                runCore.RadaPredObsluznymMiestom.Enqueue(_person);
            }
        }
        // to isté pre opačný typ zákazníka
        else if (_person.TypZakaznika == Constants.TypZakaznika.Basic || _person.TypZakaznika == Constants.TypZakaznika.Zmluvny)
        {
            var obsluzneMiesto = runCore.ObsluzneMiestoManager.GetVolneOstatne();
            // ak je obsluzne miesto volne tak vytvoríme event pre obsluzne miesto
            if (obsluzneMiesto is not null)
            {
                _core.TimeLine.Enqueue(new EventObsluhaZaciatok(runCore, _core.SimulationTime, _person, obsluzneMiesto), _core.SimulationTime);
            }
            // inak pridáme do radu
            else
            {
                runCore.RadaPredObsluznymMiestom.Enqueue(_person);
            }
        }
        // ak nastane niečo iné tak hodíme error
        else
        {
            throw new InvalidOperationException($"[EventAutomatKoniec] - v čase {_core.SimulationTime} nastala neočakávaná situácia!");
        }
    }
}