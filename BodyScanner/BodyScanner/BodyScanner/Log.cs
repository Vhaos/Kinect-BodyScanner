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
        public enum Tag { INFO, VERBOSE, ERROR, WARNING, WTF, IMP }

        public static void Write(Object log)
        {
            Write(Tag.VERBOSE, log);
        }

        public static void Write(Tag tag, Object log)
        {

            try
            {
                if (DEBUG == false) return; // Dont print anything if not in DEBUG mode

                StringBuilder sb = new StringBuilder();
                DateTime now = DateTime.Now;
                sb.Append(now + " ");
                sb.Append("[" + Enum.GetName(typeof(Tag), tag) + "] ");
                sb.Append(log.ToString());

                if (tag.Equals(Tag.ERROR)){
                    Console.WriteLine("\n====================ERROR====================");
                    Console.WriteLine(sb.ToString());
                    Console.WriteLine("=============================================\n");
                }
                else if (tag.Equals(Tag.IMP))
                {
                    Console.WriteLine("\n====================IMPORTANT====================");
                    Console.WriteLine(sb.ToString());
                    Console.WriteLine("==================================================\n");
                }
                else 
                {
                  Console.WriteLine(sb.ToString());
                }

            }
            catch (NullReferenceException e)
            {
                Log.Write(Log.Tag.ERROR, e.ToString());
            }
        }

    }
}
