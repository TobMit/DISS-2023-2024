using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;

namespace DISS_Model_Elektrokomponenty;

public class Core : EventSimulationCore<Person, DataStructure>
{
    public RadaPredObsluznymMiestom _radaPredObsluznymMiestom;
    public ObsluzneMiestoManager _obsluzneMiestoManager;
    public Core(int numberOfReplications, int cutFirst) : base(numberOfReplications, cutFirst)
    {
        _radaPredObsluznymMiestom = new();
        _obsluzneMiestoManager = new();
    }

    public override void BeforeAllReplications()
    {
        _obsluzneMiestoManager.InitObsluzneMiesta();
    }

    public override void BeforeReplication()
    {
        _radaPredObsluznymMiestom.Clear();
        _obsluzneMiestoManager.Clear();
    }

    public override void AfterReplication()
    {
        throw new NotImplementedException();
    }

    public override void AfterAllReplications()
    {
        throw new NotImplementedException();
    }
}