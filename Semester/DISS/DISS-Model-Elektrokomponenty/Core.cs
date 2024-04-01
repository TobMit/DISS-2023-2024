using DISS_EventSimulationCore;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;
using DISS_Model_Elektrokomponenty.Entity.Pokladna;
using DISS_Model_Elektrokomponenty.RNG;
using UniformD = DISS.Random.Discrete.Uniform;
using UniformC = DISS.Random.Continous.Uniform;


namespace DISS_Model_Elektrokomponenty;

public class Core : EventSimulationCore<Person, DataStructure>
{
    public RadaPredObsluznymMiestom _radaPredObsluznymMiestom;
    public ObsluzneMiestoManager _obsluzneMiestoManager;
    public PokladnaManager _pokladnaManager;
    
    public RNGPickPokladna rndPickPokladna;
    public Core(int numberOfReplications, int cutFirst) : base(numberOfReplications, cutFirst)
    {
        _radaPredObsluznymMiestom = new();
        _obsluzneMiestoManager = new();
        _pokladnaManager = new();
    }

    public override void BeforeAllReplications()
    {
        _obsluzneMiestoManager.InitObsluzneMiesta();

        rndPickPokladna = new();
    }

    public override void BeforeReplication()
    {
        _radaPredObsluznymMiestom.Clear();
        _obsluzneMiestoManager.Clear();
        _pokladnaManager.Clear();
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