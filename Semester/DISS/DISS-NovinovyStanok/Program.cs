using DISS_NovinovyStanok.Simulation;

class NovinovyStanok
{
    public static void Main()
    {
        Core core = new Core(10_000_000, 0);
        core.RunDebug();
    }
}