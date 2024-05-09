using simulation;
using Constants = simulation.Constants;

class Program
{
    static void Main(string[] args)
    {
        FileStream ostrm;
        StreamWriter writer;
        TextWriter oldOut = Console.Out;
        try
        {
            ostrm = new FileStream ("./Redirect.txt", FileMode.OpenOrCreate, FileAccess.Write);
            writer = new StreamWriter (ostrm);
        }
        catch (Exception e)
        {
            Console.WriteLine ("Cannot open Redirect.txt for writing");
            Console.WriteLine (e.Message);
            return;
        }
        //Console.SetOut (writer);
        
        // S2 - Eventová simulácia
        // Core core = new Core(50_000, 0, 13,4);
        // core.SlowDown = false;
        // core.SlowDownSpeed = 1;
        // core.RunDebug();
        
        //S3 - Agentová simulácia
        Console.WriteLine("-----Simulacia 13, 4----");
        MySimulation core = new(13,4);
        //Constants.DEBUG = true;
        core.Break = true;
        //core.ZvysenyTok = true;
        //Constants.FILTER_ZAKAZNIK = 27;
        core.Simulate(10_000);
        //core.Simulate(2);
        
        
        Console.WriteLine("-----Simulacia 14, 4----");
        core = new(14,4);
        core.Break = true;
        core.Simulate(10_000);
        Console.WriteLine("-----Simulacia 15, 4----");
        core = new(15,4);
        core.Break = true;
        core.Simulate(10_000);
        Console.WriteLine("-----Simulacia 14, 5----");
        core = new(14,5);
        core.Break = true;
        core.Simulate(10_000);
        Console.WriteLine("-----Simulacia 14, 7----");
        core = new(14,7);
        core.Break = true;
        core.Simulate(10_000);
        Console.WriteLine("-----Simulacia 17, 4----");
        core = new(17,4);
        core.Break = true;
        core.Simulate(10_000);
        Console.WriteLine("-----Simulacia 14, 7----");
        core = new(14,7);
        core.Break = true;
        core.Simulate(10_000);
        
        Console.WriteLine("----- Zvýšený tok ----");
        Console.WriteLine("-----Simulacia 14, 4----");
        core = new(14,4);
        core.Break = true;
        core.ZvysenyTok = true;
        core.Simulate(10_000);
        Console.WriteLine("-----Simulacia 15, 4----");
        core = new(15,4);
        core.Break = true;
        core.ZvysenyTok = true;
        core.Simulate(10_000);
        Console.WriteLine("-----Simulacia 14, 5----");
        core = new(14,5);
        core.Break = true;
        core.ZvysenyTok = true;
        core.Simulate(10_000);
        Console.WriteLine("-----Simulacia 14, 7----");
        core = new(14,7);
        core.Break = true;
        core.ZvysenyTok = true;
        core.Simulate(10_000);
        Console.WriteLine("-----Simulacia 17, 4----");
        core = new(17,4);
        core.Break = true;
        core.ZvysenyTok = true;
        core.Simulate(10_000);
        Console.WriteLine("-----Simulacia 100, 100----");
        core = new(100,100);
        core.Break = true;
        core.ZvysenyTok = true;
        core.Simulate(10_000);
        
        Console.SetOut (oldOut);
        writer.Close();
        ostrm.Close();
        Console.WriteLine ("Done");
    }
}