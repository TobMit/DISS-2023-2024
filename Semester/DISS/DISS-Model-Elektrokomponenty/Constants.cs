namespace DISS_Model_Elektrokomponenty;

public class Constants
{
    public enum TypZakaznika
    {
        Basic = 0,
        Zmluvný = 1,
        Online = 2
    }
    
    public enum TypPladby
    {
        Hotovost = 0,
        Karta = 1
    }

    public enum TypVelkostiNakladu
    {
        Normálna = 0,
        Veľká = 1
    }
    
    public enum TypNarocnostiTovaru
    {
        Simple = 0,
        Normal = 1,
        Hard = 2
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

    public static int POCET_DAT_V_GRAFE = 100;
    public static int RADA_PRED_OBSLUZNYM_MIESTOM = 9;
    
    public static double START_DAY = 9*60*60; // 9:00
    public static double START_ARRIVAL_SIMULATION_TIME = 0; //9:00
    public static double END_ARRIVAL_SIMULATION_TIME = 8*60*60; // 17:00 -> 8H -> 6*60*60s
    public static double END_SIMULATION_TIME = 8*60*60 + 30*60; // 17:30 -> 8:30H -> 6*60*60s + 30*60s


    public static string StavZakaznikaToString(StavZakaznika stavZakaznika)
    {
        switch (stavZakaznika)
        {
            case StavZakaznika.RadPredAutomatom:
                return "Stojí v rade pred automatom";
            case StavZakaznika.ObsluhujeAutomat:
                return "Obsluhuje automat";
            case StavZakaznika.ČakáVObchode:
                return "Čaka v obchode";
            case StavZakaznika.ObslužnomMieste_ZadávaObjednávku:
                return "Obslúžne miesto: Zadáva objednávku";
            case StavZakaznika.ObslužnomMieste_ČakáNaTovar:
                return "Obslužné miesto: Čaká na tovar";
            case StavZakaznika.PokladňaČakáVRade:
                return "Pokladňa: Čaká v rade";
            case StavZakaznika.PokladňaPlatí:
                return "Pokladňa: Platí";
            case StavZakaznika.ObslužnéMiestoVraciaSaPreVeľkýTovar:
                return "Obslužné miesto: Vracia sa pre veľký tovar";
            case StavZakaznika.OdišielZPredajne:
                return "Odišiel z predajne";
            default:
                return "Neznámy stav";
        }
    }
}