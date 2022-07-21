using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectClock
{
    public class TimeData
    {
        // make GameData serializeable. Otherwise, we can't send it over 
        // a byte stream (e.g. socket)
        [Serializable]
        public struct StructTimeData
        {
            public int hour, minute, second;
            public bool isAlarmTime;
            public bool is24HourTime;

            public StructTimeData(int h = 12, int m = 0, int s = 0, bool a = false, bool t = true)
            {
                hour = h;
                minute = m;
                second = s;
                isAlarmTime = a;
                is24HourTime = t;
            }
        }
    }
}
