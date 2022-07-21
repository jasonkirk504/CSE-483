//Name:Siyu Chen


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
using TimeDataDLL;
using System.Threading.Tasks;

// Sockets
using System.Net.Sockets;
using System.Net;
using System.IO;
// Threads
using System.Threading;
using System.ComponentModel;

namespace FinalProjectClock
{
    public partial class Model : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private int _remotePort = 5000;
        private string _remoteIPAddress = "127.0.0.1";
        UdpClient _dataSocket;

        private static Thread _receiveDataThread;

        public ObservableCollection<LED> LEDCollection;
        private bool is24Hour=false;
        private TimeData.StructTimeData alarm;
        private int alarmhour=-1;
        private int alarmminute=0;
        private int alarmsecond=0;
        private int currenthour=12;
        private int currentminute=59;
        private int currentsecond=55;
        int _numLED = 6;
        int alarmtimer = 0;
        bool alarmtriggered = false;
        public string AlarmTrigger
        {
            get
            {
                if (alarmtriggered) return "ALARM !!!";
                else return "";
            }
            set
            {
                OnPropertyChanged("AlarmTrigger");
            }
        }
        public string APM
        {
            get
            {
                if (is24Hour) return "";
                else
                {
                    if (currenthour > 12) return "PM";
                    else return "AM";
                }
            }
            set {
                OnPropertyChanged("APM"); }
        }

        private int _timer = 0;
        public int Timer
        {
            get { return _timer; }
            set
            {
                _timer = value;
                OnPropertyChanged("Timer");
            }
        }

        public void settimer()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

        }
        //timer

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            currentsecond++;
            if (currentsecond == 60)
            {
                currentminute++;
                currentsecond = 0;
            }
            if (currentminute == 60)
            {
                currenthour++;
                currentminute = 0;
            }
            if (currenthour == 25)
            {
                currenthour = 1;
                currentminute = 0;
                currentsecond = 0;
            }
            
            if (alarmtimer > 10)
            {
                alarmtriggered = false;
                alarmtimer = 0;
            }
            if (alarmhour == currenthour && alarmminute == currentminute && alarmsecond == currentsecond)
            {
                alarmtriggered = true;
            }
            if (alarmtriggered)
            {
                alarmtimer++;
            }
            setClock();
            APM = "";
            AlarmTrigger = "";
        }

        public Model()
        {
            _dataSocket = new UdpClient(5001);
            StartThread();
        }

        public void InitModel()
        {
            DateTime now=DateTime.Now;
            currenthour=now.Hour;
            currentminute=now.Minute;
            currentsecond=now.Second;
            settimer();
            LEDCollection = new ObservableCollection<LED>();
            for (int i = 0; i < 2; i++)
            {

                LED Led = new LED();
                Led.LEDTop = 50;
                Led.LEDLeft = 8+46*i;
                LEDCollection.Add(Led);
                    
            }
            for (int i = 2; i < 4; i++)
            {

                LED Led = new LED();
                Led.LEDTop =50;
                Led.LEDLeft = 16.5 + 46 * i;
                LEDCollection.Add(Led);

            }
            for (int i = 4; i < 6; i++)
            {

                LED Led = new LED();
                Led.LEDTop = 50;
                Led.LEDLeft = 29 + 46 * i;
                LEDCollection.Add(Led);

            }

        }

        public void setTimer(TimeData.StructTimeData timedate)
        {
            if (timedate.isAlarmTime) { alarm = timedate; 
                alarmhour=timedate.hour;
                alarmminute=timedate.minute;
                alarmsecond=timedate.second;
            }
            else
            {
                if (timedate.is24HourTime) is24Hour = true;
                else is24Hour = false;
                currenthour = timedate.hour;
                currentminute = timedate.minute;
                currentsecond = timedate.second;
                setClock();
            }
        }
        
        public void setClock()
        {
            int clockhour = currenthour;
            if (!is24Hour) if (currenthour > 12) clockhour -= 12;
            LEDCollection[0].LEDValue = (uint)clockhour / 10;
            LEDCollection[1].LEDValue = (uint)clockhour % 10;
            LEDCollection[2].LEDValue = (uint)currentminute / 10;
            LEDCollection[3].LEDValue = (uint)currentminute% 10;
            LEDCollection[4].LEDValue = (uint)currentsecond / 10;
            LEDCollection[5].LEDValue = (uint)currentsecond % 10;
        }

        public void CleanUp()
        {
            if (_dataSocket != null) _dataSocket.Close();
            if (_receiveDataThread != null) _receiveDataThread.Abort();
        }


        private void ReceiveThreadFunction()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0);
            while (true)
            {
                try
                {
                    // wait for data
                    // this is a blocking call
                    Byte[] receiveData = _dataSocket.Receive(ref endPoint);
                    if (receiveData.Length < 2)
                        continue;

                    TimeData.StructTimeData timeData;
                    BinaryFormatter formatter = new BinaryFormatter();
                    MemoryStream stream = new MemoryStream();

                    // deserialize data back into our GameData structure
                    stream = new System.IO.MemoryStream(receiveData);
                    timeData = (TimeData.StructTimeData)formatter.Deserialize(stream);
                    // convert byte array to a string
                    setTimer(timeData);
                }
                catch (SocketException ex)
                {
                    // got here because either the Receive failed, or more
                    // or more likely the socket was destroyed by 
                    // exiting from the JoystickPositionWindow form
                    return;
                }
            }
        }

        public void StartThread()
        {
            // start the thread to listen for data from other UDP peer
            ThreadStart threadFunction = new ThreadStart(ReceiveThreadFunction);
            _receiveDataThread = new Thread(threadFunction);
            _receiveDataThread.Start();
        }


    }
}
