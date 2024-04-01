using DISS_Model_Elektrokomponenty;

class Program
{
    static void Main(string[] args)
    {
        Core core = new Core(1_000_000, 0);
        core.RunDebug();
    }
}