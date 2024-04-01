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
        // ak je obslužne miesto prázdne hodíme error
        if (!_obsluzneMiesto.Obsadena)
        {
            throw new InvalidOperationException(
                $"[EventObsluhaKoniec] - v čase {_core.SimulationTime} obslužné miesto {_obsluzneMiesto.ID} nie je obsadené!");
        }

        // ak je obslužne miesto obsadené iným človekom
        if (_obsluzneMiesto.Person.ID != _person.ID)
        {
            throw new InvalidOperationException(
                $"[EventObsluhaKoniec] - v čase {_core.SimulationTime} obslužuje sa iný človek ({_obsluzneMiesto.Person.ID}) ako mal byť obsluhovaný ({_person.ID})!");
        }

        // uvolnime obslužné miesto ak je veľkosť tovaru normálna
        if (_person.TypVelkostiNakladu == Constants.TypVelkostiNakladu.Normalna)
        {
            _obsluzneMiesto.Uvolni();
            // ak je online zákazník a nachádza sa v rade tak ho zavoláme k obslúžnemu miestu
            if (_person.TypZakaznika == Constants.TypZakaznika.Online &&
                runCore.RadaPredObsluznymMiestom.CountOnline >= 1)
            {
                var person = runCore.RadaPredObsluznymMiestom.Dequeue(true);
                _core.TimeLine.Enqueue(new EventObsluhaZaciatok(runCore, _core.SimulationTime, person, _obsluzneMiesto),
                    _core.SimulationTime);
            }
            // to isté pre opačný ostatné typy zákazníka
            else if ((_person.TypZakaznika == Constants.TypZakaznika.Basic ||
                      _person.TypZakaznika == Constants.TypZakaznika.Zmluvny) &&
                     runCore.RadaPredObsluznymMiestom.CountOstatne >= 1)
            {
                var person = runCore.RadaPredObsluznymMiestom.Dequeue(false);
                _core.TimeLine.Enqueue(new EventObsluhaZaciatok(runCore, _core.SimulationTime, person, _obsluzneMiesto),
                    _core.SimulationTime);
            }
            // ak nie je nikto v rade tak tak nevytváram event pre začiatok obsluhy
        }
        else
        {
            // ak je veľký náklad tak neuvoľnujem pokľadňu
            _person.ObsluzneMiesto = _obsluzneMiesto;
        }

        // ak je prázdny rad tak priradíme človeka pokadni
        var pokladna = runCore.PokladnaManager.GetVolnaPokladnaPrazdnyRad(runCore);

        if (pokladna is not null)
        {
            // je pokladňa voľná naplánujem event začiatok pladby
            _core.TimeLine.Enqueue(new EventPladbaZaciatok(runCore, _core.SimulationTime, _person, pokladna),
                _core.SimulationTime);
        }
        else
        {
            // ak nie je pokladňa voľná pridáme do rady
            _person.StavZakaznika = Constants.StavZakaznika.PokladnaCakaVrade;
            runCore.PokladnaManager.PriradZakaznikaDoRady(_person, runCore);
        }
    }
}