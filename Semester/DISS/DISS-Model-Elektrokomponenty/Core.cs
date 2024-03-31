using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;

namespace DISS_Model_Elektrokomponenty;

public class Core : EventSimulationCore<Person, DataStructure>
{
    public RadaPredObsluznymMiestom _radaPredObsluznymMiestom;
    public Core(int numberOfReplications, int cutFirst) : base(numberOfReplications, cutFirst)
    {
        _radaPredObsluznymMiestom = new();
    }

    public override void BeforeAllReplications()
    {
        throw new NotImplementedException();
    }

    public override void BeforeReplication()
    {
        _radaPredObsluznymMiestom.Clear();
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