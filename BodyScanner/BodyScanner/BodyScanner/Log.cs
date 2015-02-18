using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyScanner
{
    /// <summary>
    /// This class gives structure to logs
    /// Also, it centralises all loggings so it can
    /// be turned off.
    /// </summary>
    static class Log
    {
        public static bool DEBUG = true;// Set this to false to stop printing logs
        public enum Tag { INFO, VERBOSE, ERROR, WARNING, WTF }

        public static void Write(Object log)
        {
            Write(Tag.VERBOSE, log);
        }

        public static void Write(Tag tag, Object log)
        {

            if (DEBUG == false) return; // Dont print anything if not in DEBUG mode

            StringBuilder sb = new StringBuilder();
            DateTime now = DateTime.Now;
            sb.Append(now + " ");
            sb.Append("[" + Enum.GetName(typeof(Tag), tag) + "] ");
            sb.Append(log.ToString());
            Console.WriteLine(sb.ToString());
        }

    }
}
