using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DISS_JanModelPozicky.Core;

public class ObservableObjects : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}