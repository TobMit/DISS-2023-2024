namespace DISS_Model_Elektrokomponenty.Entity;

public class Automat
{
    public int CelkovyPocet { get; private set; }

    public Automat()
    {
        CelkovyPocet = 0;
    }

    public int GetId()
    {
        return CelkovyPocet++;
    }

    public void Clear()
    {
        CelkovyPocet = 0;
    }
}