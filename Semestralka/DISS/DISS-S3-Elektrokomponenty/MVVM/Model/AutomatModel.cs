using System;
using DISS_S3_Elektrokomponenty.Core;


public class AutomatModel : ObservableObjects
{
    private bool _obsadeny;
    private string _obsah;

    public bool Obsadeny
    {
        get => _obsadeny;
        set
        {
            if (value != Obsadeny)
            {
                _obsadeny = value;
                OnPropertyChanged();
            }
        }
    }

    public String Obsah
    {
        get => _obsah;
        set
        {
            if (String.Compare(value, _obsah, StringComparison.Ordinal) != 0)
            {
                _obsah = value;
                OnPropertyChanged();
            }
        }
    }

    public AutomatModel()
    {
        _obsadeny = false;
        _obsah = "-/-";
    }

    public void Update(string pObsah, bool pObsadeny)
    {
        Obsah = pObsah;
        Obsadeny = pObsadeny;
    }
}