using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Classes
{
    class ElapsedGameTime
    {
        int TotalSeconds;
        TimeSpan NowSecondsTime;
        public ElapsedGameTime()
        {
            NowSecondsTime = DateTime.Now.TimeOfDay;
        }

        public int getTotalSeconds()
        {
            TotalSeconds = (DateTime.Now.TimeOfDay - NowSecondsTime).Milliseconds;
            NowSecondsTime = DateTime.Now.TimeOfDay;
            return TotalSeconds;
        }
    }
}
