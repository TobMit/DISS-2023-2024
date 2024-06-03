using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DISS_S2_Elektroomponenty.MVVM.ViewModel;

namespace DISS_S2_Elektroomponenty
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UIElement_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var slider = (Slider)sender;
            var point = e.GetPosition(slider);
            var value = point.X / slider.ActualWidth * (slider.Maximum - slider.Minimum) + slider.Minimum;
            slider.Value = value;
        }

        private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.OnWindowClosing();
            }
        }
    }
}
