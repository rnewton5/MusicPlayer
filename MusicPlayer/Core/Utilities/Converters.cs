using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Utilities
{
    public static class Converters
    {
        public static string TimeSpan_toString(TimeSpan time)
        {
            string duration = (new TimeSpan(time.Days, time.Hours, time.Minutes, time.Seconds)).ToString().TrimStart(new char[] { '0', ':' });
            if (duration.Length == 1)
            {
                duration = "0:0" + duration;
            }
            else if (duration.Length == 2)
            {
                duration = "0:" + duration;
            }
            duration = duration.Replace('.', ':');
            return duration;
        }
    }
}
