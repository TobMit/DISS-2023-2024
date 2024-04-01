namespace DISS_Model_Elektrokomponenty;

public class Constants
{
    public enum TypZakaznika
    {
        Basic = 0,
        Zmluvny = 1,
        Online = 2
    }
    
    public enum TypPladby
    {
        Hotovost = 0,
        Karta = 1
    }

    public enum TypVelkostiNakladu
    {
        Normalna = 0,
        Velka = 1
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
        CakaVObchode = 2,
        ObsluznomMiestoZadavaObjednavku = 3,
        ObsluznomMiestoCakaNaTovar = 4, //todo možo vymeniť s 3
        PokladnaCakaVrade = 5,
        PokladnaPlati = 6,
        ObsluzneMiestoVraciaSaPreVelkyTovar = 7,
        OdisielZPredajne = 8
    }
    
    public static int POCET_OBSLUZNYCH_MIEST = 3;
    public static int POCET_POKLADNI = 3;
    
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
            case StavZakaznika.CakaVObchode:
                return "Čaka v obchode";
            case StavZakaznika.ObsluznomMiestoZadavaObjednavku:
                return "Obslúžne miesto: Zadáva objednávku";
            case StavZakaznika.ObsluznomMiestoCakaNaTovar:
                return "Obslužné miesto: Čaká na tovar";
            case StavZakaznika.PokladnaCakaVrade:
                return "Pokladňa: Čaká v rade";
            case StavZakaznika.PokladnaPlati:
                return "Pokladňa: Platí";
            case StavZakaznika.ObsluzneMiestoVraciaSaPreVelkyTovar:
                return "Obslužné miesto: Vracia sa pre veľký tovar";
            case StavZakaznika.OdisielZPredajne:
                return "Odišiel z predajne";
            default:
                return "Neznámy stav";
        }
    }
}