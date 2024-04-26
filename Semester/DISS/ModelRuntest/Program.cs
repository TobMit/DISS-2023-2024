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
        MySimulation core = new();
        // Constants.DEBUG = true;
        core.Simulate(10_000,Constants.END_SIMULATION_TIME);
        // core.Simulate(1,Constants.END_SIMULATION_TIME);
        
    }
}