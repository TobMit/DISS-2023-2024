using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DISS_S2_Elektroomponenty.Core;

public class ObservableObjects : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}