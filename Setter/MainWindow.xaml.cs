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
using TimeDataDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// observable collections
using System.Collections.ObjectModel;

// debug output
using System.Diagnostics;

// timer, sleep
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Runtime.Serialization.Formatters.Binary;
// Rectangle
// Must update References manually
using System.Drawing;

// INotifyPropertyChanged
using System.ComponentModel;

// Threading.Timer
using System.Windows.Threading;

// Timer.Timer
using System.Timers;
using System.Windows.Input;

using System.Threading.Tasks;

// Sockets
using System.Net.Sockets;
using System.Net;
using System.IO;
// Threads
using System.Threading;
using System.ComponentModel;
namespace Setter
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
            _model = new Model();
            this.DataContext = _model;
        }


        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _model = new Model();
            this.DataContext = _model;
        }
        private void SetTimeButton_Click(object sender, RoutedEventArgs e)
        {
            _model.SetTime(0);
        }

        private void SetAlarmButton_Click(object sender, RoutedEventArgs e)
        {
            _model.SetTime(1);
        }

        private void NowTimeButton_Click(object sender, RoutedEventArgs e)
        {
            _model.SetTime(2);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _model.CleanUp();
        }
    }
}
