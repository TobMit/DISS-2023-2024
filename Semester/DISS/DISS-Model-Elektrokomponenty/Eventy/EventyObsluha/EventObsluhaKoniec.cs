using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;
using DISS_Model_Elektrokomponenty.Eventy.EventPladby;

namespace DISS_Model_Elektrokomponenty.Eventy.EventyObsluha;

public class EventObsluhaKoniec : SimulationEvent<Person, DataStructure>
{
    private ObsluzneMiesto _obsluzneMiesto;
    private Person _person;

    public EventObsluhaKoniec(EventSimulationCore<Person, DataStructure> pCore,
        double eventTime,
        Person pPerson,
        ObsluzneMiesto pObsluzneMiesto) : base(pCore, eventTime)
    {
        _obsluzneMiesto = pObsluzneMiesto;
        _person = pPerson;
    }

    public override void Execuete()
    {
        Core runCore = (Core)_core;
        if (_core._eventData != null) _core._eventData.NewData = true;
        // ak je obslužné miesto prázdne hodíme error
        if (!_obsluzneMiesto.Obsadena)
        {
            throw new InvalidOperationException(
                $"[EventObsluhaKoniec] - v čase {_core.SimulationTime} obslužné miesto {_obsluzneMiesto.ID} nie je obsadené!");
        }

        // ak je obslužné miesto obsadené iným človekom
        if (_obsluzneMiesto.Person.ID != _person.ID)
        {
            throw new InvalidOperationException(
                $"[EventObsluhaKoniec] - v čase {_core.SimulationTime} sa obsluhuje sa iný človek ({_obsluzneMiesto.Person.ID}) ako mal byť obsluhovaný ({_person.ID})!");
        }

        // uvoľnime obslužné miesto ak je veľkosť tovaru normálna
        if (_person.TypVelkostiNakladu == Constants.TypVelkostiNakladu.Normálna)
        {
            _obsluzneMiesto.Uvolni();
            // ak je online zákazník a nachádza sa v rade tak ho zavoláme k obslúžnemu miestu
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
            // ak nie je nikto v rade tak tak nevytváram event pre začiatok obsluhy
        }
        else
        {
            // ak je veľký náklad tak neuvoľňujem pokladňu
            _person.ObsluzneMiesto = _obsluzneMiesto;
        }

        // ak je prázdny rad tak priradíme človeka pokladni
        var pokladna = runCore.PokladnaManager.GetVolnaPokladnaPrazdnyRad(runCore);

        if (pokladna is not null)
        {
            // je pokladňa voľná naplánujem event začiatok platby
            pokladna.ObsadPokladnu(_person);
            _core.TimeLine.Enqueue(new EventPladbaZaciatok(runCore, _core.SimulationTime, _person, pokladna),
                _core.SimulationTime);
        }
        else
        {
            // ak nie je pokladňa voľná pridáme do rady
            _person.StavZakaznika = Constants.StavZakaznika.PokladňaČakáVRade;
            runCore.PokladnaManager.PriradZakaznikaDoRady(_person, runCore);
        }
    }
}