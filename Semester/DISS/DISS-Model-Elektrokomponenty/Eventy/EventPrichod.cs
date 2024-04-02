using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;
using DISS_Model_Elektrokomponenty.Eventy.EventyAutomat;

namespace DISS_Model_Elektrokomponenty.Eventy;

public class EventPrichod : SimulationEvent<Person, DataStructure>
{
    public EventPrichod(EventSimulationCore<Person, DataStructure> pCore, double eventTime) : base(pCore, eventTime)
    {
    }

    public override void Execuete()
    {
        Core runCore = (Core)_core;
        Person tmpPerson = new Person(_core.SimulationTime, 
            runCore.RndTypZakaznika.Next(),
            runCore.RndTypNarocnostTovaru.Next(),
            runCore.RndTypVelkostiNakladu.Next(), 
            runCore.Automat.GetId());
        runCore.Persons.Add(tmpPerson);
        
        // skontrolujeme či je sú v rade pred automatom ľudia
        if (runCore.RadaPredAutomatom.Count >= 1)
        {
            //runCore.StatPriemednaDlzakaRaduAutomatu.AddValue(runCore.RadaPredAutomatom.Count, _core.SimulationTime);
            runCore.RadaPredAutomatom.Enqueue(tmpPerson);
            runCore.StatPriemednaDlzakaRaduAutomatu.AddValue(runCore.RadaPredAutomatom.Count, _core.SimulationTime);
        }
        // ak je vpredajni viac ako 8 ľudí tak musíme čakať pred predajňou
        else if (runCore.RadaPredObsluznymMiestom.Count >= 8)
        {
            //runCore.StatPriemednaDlzakaRaduAutomatu.AddValue(runCore.RadaPredAutomatom.Count, _core.SimulationTime);
            runCore.RadaPredAutomatom.Enqueue(tmpPerson);
            runCore.StatPriemednaDlzakaRaduAutomatu.AddValue(runCore.RadaPredAutomatom.Count, _core.SimulationTime);
        }
        // ak je automat obsadený a v rade nie je nikto tak pridáme do rady pred automatom
        else if (runCore.Automat.Obsadeny && runCore.RadaPredAutomatom.Count < 1)
        {
            //runCore.StatPriemednaDlzakaRaduAutomatu.AddValue(runCore.RadaPredAutomatom.Count, _core.SimulationTime);
            runCore.RadaPredAutomatom.Enqueue(tmpPerson);
            runCore.StatPriemednaDlzakaRaduAutomatu.AddValue(runCore.RadaPredAutomatom.Count, _core.SimulationTime);
        }
        // mali by sme mať pokryté všetky prípady, nikto nie je v rade pred automatom a automat nie je obsadený
        else if (!runCore.Automat.Obsadeny)
        {
            // vytvoríme event pre začiatok obsluhy automatu
            runCore.Automat.Obsluz(tmpPerson);
            runCore.TimeLine.Enqueue(new EventAutomatZaciatok(runCore, _core.SimulationTime, tmpPerson), _core.SimulationTime);
        }
        // ak by nastala neplánována situácia - zistenie chyby
        else
        {
            throw new InvalidOperationException($"[Event príchod] - v čase {_core.SimulationTime} nastala neočakávaná situácia!");
        }
        
        // naplánujeme daľší príchod zákazníka ak je stále otvorené
        var newArrival = runCore.RndPrichodZakaznika.Next() + _core.SimulationTime;
        if (newArrival < Constants.END_ARRIVAL_SIMULATION_TIME)
        {
            EventPrichod newEvent = new EventPrichod(_core, newArrival);
            _core.TimeLine.Enqueue(newEvent, newArrival);
        }
        else
        {
            // ak je po, tak ľudia pred automatom odýdu
            //runCore.StatPriemednaDlzakaRaduAutomatu.AddValue(runCore.RadaPredAutomatom.Count, _core.SimulationTime);
            while (runCore.RadaPredAutomatom.Count >= 1)
            {
                var leavePerson = runCore.RadaPredAutomatom.Dequeue();
                leavePerson.StavZakaznika = Constants.StavZakaznika.OdisielZPredajne;
                //runCore.StatPriemernyCasVObchode.AddValue(_core.SimulationTime - leavePerson.TimeOfArrival);
            }
            runCore.StatPriemednaDlzakaRaduAutomatu.AddValue(runCore.RadaPredAutomatom.Count, _core.SimulationTime);
        }
    }
}