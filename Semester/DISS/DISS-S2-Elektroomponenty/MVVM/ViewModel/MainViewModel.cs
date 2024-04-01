using DISS_S2_Elektroomponenty.Core;

namespace DISS_S2_Elektroomponenty.MVVM.ViewModel;

public class MainViewModel : ObservableObjects
{
    private DISS_Model_Elektrokomponenty.Core? _core;
    
    public RelayCommand StartCommand { get; set; }
    public RelayCommand StopCommand { get; set; }
    public RelayCommand RychlostCommand { get; set; }
    public MainViewModel()
    {
        InicialiseButtons();
    }
    
    private void InicialiseButtons()
    {
        StartCommand = new RelayCommand(o => StartModel());
        StopCommand = new RelayCommand(o => StopModel());
        RychlostCommand = new RelayCommand(o => RychlostModel());
    }
    
    private void StartModel()
    {
        _core = new DISS_Model_Elektrokomponenty.Core(25_000, 0);
        _core.Run();
    }

    private void StopModel()
    {
        if (_core is not null)
        {
            _core.Stop();
        }
    }
    
    private void RychlostModel()
    {
        if (_core is not null)
        {
            _core.SlowDown = !_core.SlowDown;
        }
    }
}