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
}