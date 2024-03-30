using DISS_NovinovyStanok.Simulation;

class NovinovyStanok
{
    public static void Main()
    {
        Core core = new Core(100_000_000, 0);
        core.RunDebug();
    }
}