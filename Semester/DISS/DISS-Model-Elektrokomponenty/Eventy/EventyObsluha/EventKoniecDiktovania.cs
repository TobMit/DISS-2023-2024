using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;

namespace DISS_Model_Elektrokomponenty.Eventy.EventyObsluha;

public class EventKoniecDiktovania : SimulationEvent<Person, DataStructure>
{
    private ObsluzneMiesto _obsluzneMiesto;
    private Person _person;

    public EventKoniecDiktovania(EventSimulationCore<Person, DataStructure> pCore, 
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
        // ak obslužné miesto nie je obsadené tak hodíme error
        if (!_obsluzneMiesto.Obsadena)
        {
            throw new InvalidOperationException($"[EventKoniecDiktovania] - v čase {_core.SimulationTime} obslužné miesto {_obsluzneMiesto.ID} nie je obsadené!");
        }
        
        // ak je obslužne miesto obsadené iným človekom
        if (_obsluzneMiesto.Person.ID != _person.ID)
        {
            throw new InvalidOperationException($"[EventKoniecDiktovania] - v čase {_core.SimulationTime} obslužuje sa iný človek ({_obsluzneMiesto.Person.ID}) ako mal byť obsluhovaný ({_person.ID})!");
        }

        _person.StavZakaznika = Constants.StavZakaznika.ObslužnomMieste_ČakáNaTovar;
        // naplánovanie objednávky podľa zložitosti objednávky
        if (_person.TypNarocnostiTovaru == Constants.TypNarocnostiTovaru.Simple)
        {
            var newKoniecObjednavky = runCore.RndTrvaniePripravaSimple.Next() + _core.SimulationTime;
            _core.TimeLine.Enqueue(new EventObsluhaKoniec(runCore, newKoniecObjednavky, _person, _obsluzneMiesto), newKoniecObjednavky);
        }
        else if (_person.TypNarocnostiTovaru == Constants.TypNarocnostiTovaru.Normal)
        {
            var newKoniecObjednavky = runCore.RndTrvaniePripravaNormal.Next() + _core.SimulationTime;
            _core.TimeLine.Enqueue(new EventObsluhaKoniec(runCore, newKoniecObjednavky, _person, _obsluzneMiesto), newKoniecObjednavky);
        }
        else if (_person.TypNarocnostiTovaru == Constants.TypNarocnostiTovaru.Hard)
        {
            var newKoniecObjednavky = runCore.RndTrvaniePripravaHard.Next() + _core.SimulationTime;
            _core.TimeLine.Enqueue(new EventObsluhaKoniec(runCore, newKoniecObjednavky, _person, _obsluzneMiesto), newKoniecObjednavky);
        }
        else
        {
            throw new InvalidOperationException($"[EventKoniecDiktovania] - v čase {_core.SimulationTime} nastala neznáma situácia!");
        }
    }
}