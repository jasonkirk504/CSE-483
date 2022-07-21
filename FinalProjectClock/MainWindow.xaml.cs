using System;
using System.Collections.Generic;
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
using System.Diagnostics;

namespace FinalProjectClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private Model _model;
        public MainWindow()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
        }
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _model = new Model();
            this.DataContext = _model;

            _model.InitModel();

            Debug.WriteLine("This is C#");
            // create an observable collection. this collection
            // contains the tiles the represent the Tic Tac Toe grid
            SevenSegmentLED.ItemsSource = _model.LEDCollection;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _model.CleanUp();
        }
    }
}
