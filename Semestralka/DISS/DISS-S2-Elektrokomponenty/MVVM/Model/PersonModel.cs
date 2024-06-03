using DISS_Model_Elektrokomponenty;
using DISS_Model_Elektrokomponenty.Entity;
using DISS_S2_Elektroomponenty.Core;
using System;

namespace DISS_S2_Elektroomponenty.MVVM.Model;

public class PersonModel : ObservableObjects
{
    private int _ID;
    private string _timeOfArrival;
    private Constants.TypZakaznika _typZakaznika;
    private Constants.TypNarocnostiTovaru _typNarocnostiTovaru;
    private Constants.TypVelkostiNakladu _typVelkostiNakladu;
    private Constants.StavZakaznika _stavZakaznika;

    public int ID
    {
        get => _ID;
        set
        {
            if (value != _ID)
            {
                _ID = value;
                OnPropertyChanged();
            }
        }
    }

    public string TimeOfArrival
    {
        get => _timeOfArrival;
        set
        {
            if (String.Compare(value, _timeOfArrival, StringComparison.Ordinal) != 0)
            {
                _timeOfArrival = value;
                OnPropertyChanged();
            }
        }
    }

    public Constants.TypZakaznika TypZakaznika
    {
        get => _typZakaznika;
        set
        {
            if (value != _typZakaznika)
            {
                _typZakaznika = value;
                OnPropertyChanged();
            }
        }
    }

    public Constants.TypNarocnostiTovaru TypNarocnostiTovaru
    {
        get => _typNarocnostiTovaru;
        set
        {
            if (value != _typNarocnostiTovaru)
            {
                _typNarocnostiTovaru = value;
                OnPropertyChanged();
            }
        }
    }

    public Constants.TypVelkostiNakladu TypVelkostiNakladu
    {
        get => _typVelkostiNakladu;
        set
        {
            if (value != _typVelkostiNakladu)
            {
                _typVelkostiNakladu = value;
                OnPropertyChanged();
            }
        }
    }

    public Constants.StavZakaznika StavZakaznika
    {
        get => _stavZakaznika;
        set
        {
            if (value != _stavZakaznika)
            {
                _stavZakaznika = value;
                OnPropertyChanged();
            }
        }
    }

    public PersonModel(Person person)
    {
        ID = person.ID;
        TimeOfArrival = TimeSpan.FromSeconds(Constants.START_DAY + person.TimeOfArrival).ToString(@"hh\:mm\:ss");
        TypZakaznika = person.TypZakaznika;
        TypNarocnostiTovaru = person.TypNarocnostiTovaru;
        TypVelkostiNakladu = person.TypVelkostiNakladu;
        StavZakaznika = person.StavZakaznika;
    }

    public void Update(Person pPerson)
    {
        ID = pPerson.ID;
        TimeOfArrival = TimeSpan.FromSeconds(Constants.START_DAY + pPerson.TimeOfArrival).ToString(@"hh\:mm\:ss");
        TypZakaznika = pPerson.TypZakaznika;
        TypNarocnostiTovaru = pPerson.TypNarocnostiTovaru;
        TypVelkostiNakladu = pPerson.TypVelkostiNakladu;
        StavZakaznika = pPerson.StavZakaznika;
    }
}