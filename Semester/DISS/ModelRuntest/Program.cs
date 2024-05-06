using DISS_Model_Elektrokomponenty;
using simulation;
using Constants = simulation.Constants;

class Program
{
    static void Main(string[] args)
    {
        
        // S2 - Eventová simulácia
        // Core core = new Core(50_000, 0, 13,4);
        // core.SlowDown = false;
        // core.SlowDownSpeed = 1;
        // core.RunDebug();
        
        //S3 - Agentová simulácia
        MySimulation core = new(14,6);
        //Constants.DEBUG = true;
        core.Break = true;
        core.ZvysenyTok = true;
        //Constants.FILTER_ZAKAZNIK = 27;
        core.Simulate(5_000);
        //core.Simulate(2);
    }
}