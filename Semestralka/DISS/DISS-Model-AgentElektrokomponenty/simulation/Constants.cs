using DISS_Model_AgentElektrokomponenty.Entity;

namespace simulation;

public class Constants
{
    public enum TypZakaznika
    {
        Basic = 0,
        Zmluvný = 1,
        Online = 2
    }
    
    public enum TypVelkostiNakladu
    {
        Normálna = 0,
        Veľká = 1
    }
    
    public enum TypNarocnostiTovaru
    {
        Jednoduchá = 0,
        Normálna = 1,
        Zložitá = 2
    }
    
    public enum StavZakaznika
    {
        RadPredAutomatom = 0,
        ObsluhujeAutomat = 1,
        ČakáVObchode = 2,
        ObslužnomMieste_ZadávaObjednávku = 3,
        ObslužnomMieste_ČakáNaTovar = 4,
        PokladňaČakáVRade = 5,
        PokladňaPlatí = 6,
        ObslužnéMiestoVraciaSaPreVeľkýTovar = 7,
        OdišielZPredajne = 8
    }
    
    public static int POCET_UPDATOV_ZA_SEKUNDU = 4;

    public static int POCET_DAT_V_GRAFE = 100;
    public static int RADA_PRED_OBSLUZNYM_MIESTOM = 8; // v rátane keď počítam od 0
    
    public static double START_DAY = 9*60*60; // 9:00
    public static double START_ARRIVAL_SIMULATION_TIME = 0; //9:00
    public static double STAR_BREAK = 3*60*60; //3h od 9:00 do 12:00
    public static double BREAK_DURATION = 30*60; //30min
    public static double END_ARRIVAL_SIMULATION_TIME = 8*60*60; // 17:00 -> 8H -> 6*60*60s
    public static double END_SIMULATION_TIME = 8*60*60 + 30*60; // 17:30 -> 8:30H -> 6*60*60s + 30*60s

    public static bool DEBUG = false;
    public static int FILTER_ZAKAZNIK = -1;
    
    public static void Log(string pModul, double time, Person? pPerson, string message, LogType logType = LogType.DefaultLog)
    {
        if (pPerson is not null)
        {
            if (pPerson.ID != FILTER_ZAKAZNIK && FILTER_ZAKAZNIK != -1)
            {
                return;
            }
        }
        if (DEBUG)
        {
            switch (logType)
            {
                case LogType.DefaultLog:
                    break;
                case LogType.AgentLog:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogType.ManagerLog:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case LogType.ContinualAssistantLog:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogType.InstantAssistantLog:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
            }
            if (pPerson is null)
            {
                Console.WriteLine($"[{pModul}] ({TimeSpan.FromSeconds(time + START_DAY).ToString(@"hh\:mm\:ss")}) {message}");
            }
            else
            {
                Console.WriteLine(
                    $"[{pModul}] Zakaznik {pPerson.ID}: ({TimeSpan.FromSeconds(time + START_DAY).ToString(@"hh\:mm\:ss")}) {message}");
            }

            Console.ResetColor();
        }
    }
    
    public enum LogType
    {
        DefaultLog,
        AgentLog,
        ManagerLog,
        ContinualAssistantLog,
        InstantAssistantLog
        
    }
}