using TimeDataDLL;
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

using System.Threading.Tasks;

// Sockets
using System.Net.Sockets;
using System.Net;
using System.IO;
// Threads
using System.Threading;
using System.ComponentModel;
// Message Box in Console app
// Don't forget to add Reference to System.Windows.Forms

// Message Box
// don't forget to add Reference System.Windows.Forms


namespace Setter
    {
        public class Model : INotifyPropertyChanged
        {
            private int _remotePort = 5000;
            private string _remoteIPAddress = "127.0.0.1";
            UdpClient _dataSocket;

            private static Thread _receiveDataThread;



            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

        private bool tfhour;
        public bool tfHour
        {
            get { return tfhour; }
            set
            {
                tfhour = value;
                OnPropertyChanged("tfHour");
            }
        }

        private String _hourText;
        public String HourText
        {
            get { return _hourText; }
            set
            {
                _hourText = value;
                OnPropertyChanged("HourText");
            }
        }

        private String _minuteText;
        public String MinuteText
        {
            get { return _minuteText; }
            set
            {
                _minuteText = value;
                OnPropertyChanged("MinuteText");
            }
        }

        private String _secondText;
        public String SecondText
        {
            get { return _secondText; }
            set
            {
                _secondText = value;
                OnPropertyChanged("SecondText");
            }
        }
        public Model()
            {
                _dataSocket = new UdpClient(5000);
            }

            public void SendMessage(TimeData.StructTimeData gData)
            {
                TimeData.StructTimeData gameData=gData;

                // formatter used for serialization of data
                BinaryFormatter formatter = new BinaryFormatter();

                // stream needed for serialization
                MemoryStream stream = new MemoryStream();

                // Byte array needed to send data over a socket
                Byte[] sendBytes;

                // check to make sure boxes have something in them to send


                // we make sure that the data in the boxes is in the correct format return;
                

                // serialize the gameData structure to a stream
                formatter.Serialize(stream, gameData);

                // retrieve a Byte array from the stream
                sendBytes = stream.ToArray();

                // send the serialized data
                IPEndPoint remoteHost = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)5001);
                try
                {
                    _dataSocket.Send(sendBytes, sendBytes.Length, remoteHost);
                }
                catch (SocketException)
                {
                    return;
                }
            }

        public void CleanUp()
        {
            if (_dataSocket != null) _dataSocket.Close();
            if (_receiveDataThread != null) _receiveDataThread.Abort();
        }
        public void SetTime(int status)
        {
            TimeData.StructTimeData gameData = new TimeData.StructTimeData();
            if (status != 2)
            {
                gameData.hour = int.Parse(HourText);
                gameData.minute = int.Parse(MinuteText);
                gameData.second = int.Parse(SecondText);
            }
            else
            {
                DateTime now=DateTime.Now;
                gameData.hour = now.Hour;
                gameData.minute = now.Minute;
                gameData.second = now.Second;
            }
            if (status == 1) gameData.isAlarmTime = true;
            else gameData.isAlarmTime = false;
            gameData.is24HourTime = tfHour;

            // formatter used for serialization of data
            BinaryFormatter formatter = new BinaryFormatter();

            // stream needed for serialization
            MemoryStream stream = new MemoryStream();

            // Byte array needed to send data over a socket
            Byte[] sendBytes;

            // check to make sure boxes have something in them to send


            // we make sure that the data in the boxes is in the correct format return;


            // serialize the gameData structure to a stream
            formatter.Serialize(stream, gameData);

            // retrieve a Byte array from the stream
            sendBytes = stream.ToArray();

            // send the serialized data
            IPEndPoint remoteHost = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)5001);
            try
            {
                _dataSocket.Send(sendBytes, sendBytes.Length, remoteHost);
            }
            catch (SocketException)
            {
                return;
            }
        }




    }

    }

