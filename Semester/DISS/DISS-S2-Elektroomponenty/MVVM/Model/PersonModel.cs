using DISS_Model_Elektrokomponenty;
using DISS_Model_Elektrokomponenty.Entity;
using DISS_S2_Elektroomponenty.Core;

namespace DISS_S2_Elektroomponenty.MVVM.Model;

public class PersonModel : ObservableObjects
{
    private int _ID;
    private double _timeOfArrival;
    private Constants.TypZakaznika _typZakaznika;
    private Constants.TypNarocnostiTovaru _typNarocnostiTovaru;
    private Constants.TypVelkostiNakladu _typVelkostiNakladu;
    private Constants.StavZakaznika _stavZakaznika;

    public int ID
    {
        get => _ID;
        set
        {
            _ID = value;
            // OnPropertyChanged();
        }
    }

    public double TimeOfArrival
    {
        get => _timeOfArrival;
        set
        {
            _timeOfArrival = value;
            // OnPropertyChanged();
        }
    }

    public Constants.TypZakaznika TypZakaznika
    {
        get => _typZakaznika;
        set
        {
            _typZakaznika = value;
            // OnPropertyChanged();
        }
    }

    public Constants.TypNarocnostiTovaru TypNarocnostiTovaru
    {
        get => _typNarocnostiTovaru;
        set
        {
            _typNarocnostiTovaru = value;
            // OnPropertyChanged();
        }
    }

    public Constants.TypVelkostiNakladu TypVelkostiNakladu
    {
        get => _typVelkostiNakladu;
        set
        {
            _typVelkostiNakladu = value;
            // OnPropertyChanged();
        }
    }

    public Constants.StavZakaznika StavZakaznika
    {
        get => _stavZakaznika;
        set
        {
            _stavZakaznika = value;
            // OnPropertyChanged();
        }
    }

    public PersonModel(Person person)
    {
        ID = person.ID;
        TimeOfArrival = person.TimeOfArrival;
        TypZakaznika = person.TypZakaznika;
        TypNarocnostiTovaru = person.TypNarocnostiTovaru;
        TypVelkostiNakladu = person.TypVelkostiNakladu;
        StavZakaznika = person.StavZakaznika;
    }
}