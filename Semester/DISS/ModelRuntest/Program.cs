using DISS_Model_Elektrokomponenty;

class Program
{
    static void Main(string[] args)
    {
        Core core = new Core(10_000, 0, 3,2);
        core.SlowDown = false;
        core.SlowDownSpeed = 1;
        core.RunDebug();
    }
}