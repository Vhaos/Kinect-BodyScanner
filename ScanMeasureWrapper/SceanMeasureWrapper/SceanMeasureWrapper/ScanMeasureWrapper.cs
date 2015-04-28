using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SceanMeasureWrapper
{
    /// <summary>
    /// This is the ScanMeasureWrapper Program
    /// This was created to act as a wrapper for a bug
    /// found in ScanMeasure. 
    /// </summary>
    class ScanMeasureWrapper
    {
        static void Main(string[] args)
        {
             /* Arguments:
             * Path for output file
             * MKF{1/2} = female/male
             */

            String path = "";
            String gender = "";
            try
            {
                //Get the arguments from Console
                path = args[0];
                gender = args[1];
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("It should be: SceanMeasureWrapper <file> MKF{1/2}");
                System.IO.File.WriteAllText("C:/ScanMeasureWraopper_log.txt", "It should be: SceanMeasureWrapper <file> MKF{1/2}");
                return;
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Make sure you put the full file path in quotations.");
                return;
            }
            

            if(!path.StartsWith("\"")){
                path = "\"" + path + "\"";
            }

            Console.WriteLine(System.DateTime.Now.ToShortTimeString());

            Console.WriteLine(path);
            Console.WriteLine(gender);

           // Start Dr. Tony Rutos software as a seperate process
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "C:\\Program Files (x86)\\Tony Ruto\\Home Scanner Tools\\ScanMeasureCmd.exe";
            startInfo.Arguments = path + " " +gender;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            Console.WriteLine("Please Wait...");
            Process process = Process.Start(startInfo);
            process.Start();
            String log = "";
            while (!process.StandardOutput.EndOfStream) {
                string line = process.StandardOutput.ReadLine();
                log = log + "\n"+ line;
            }
            

            /*
             *Parse the console from the Scan Measure software 
             */
            String height_string = Regex.Match(log,@"Height[ \t]*[0-9]*").Groups[0].Value;
            String bust_string = Regex.Match(log, @"Bust[ \t]*[0-9]*").Groups[0].Value;
            String waist_string = Regex.Match(log, @"Waist[ \t]*[0-9]*").Groups[0].Value;
            String hip_string = Regex.Match(log, @"Hip[ \t]*[0-9]*").Groups[0].Value;
            String insideleg_string = Regex.Match(log, @"Inside Leg[ \t]*[0-9]*").Groups[0].Value;


            int height = int.Parse(Regex.Match(height_string, @"([0-9]+)").Groups[0].Value);
            int bust = int.Parse(Regex.Match(bust_string, @"([0-9]+)").Groups[0].Value);
            int waist = int.Parse(Regex.Match(waist_string, @"([0-9]+)").Groups[0].Value);
            int hip = int.Parse(Regex.Match(hip_string, @"([0-9]+)").Groups[0].Value);
            int inside_leg = int.Parse(Regex.Match(insideleg_string, @"([0-9]+)").Groups[0].Value);

            Thread.Sleep(2000);//Sleep to give oppurtunity to Scanmeasure to print its xml before overwriting it
            
            /*
             * Generate the XML with the corrected heights
             */
            String xml = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>"+ "\n" +
                        "<?xml-stylesheet type=\"text/xsl\" href=\"./MSV_Stylesheet.xslt\"?>"+ "\n" +
                        "<MSV_Measures>" + "\n" +
                        "<file name=\"pointcloud\"/>" + "\n" +
                        "<measure id=\"MSV_Height\" name=\"Height\" valid=\"true\" unit=\"cm\">"+ height +"</measure>" + "\n" +
                        "<measure id=\"MSV_Hip\" name=\"Hip\" valid=\"true\" unit=\"cm\">" + hip + "</measure>" + "\n" +
                        "<measure id=\"MSV_Chest\" name=\"Chest | Bust\" valid=\"true\" unit=\"cm\">"+ bust +"</measure>" + "\n" +
                        "<measure id=\"MSV_Waist\" name=\"Waist\" valid=\"true\" unit=\"cm\">"+ waist +"</measure>" + "\n" +
                        "<measure id=\"MSV_InsideLeg\" name=\"Inside Leg\" valid=\"true\" unit=\"cm\">" + inside_leg + "</measure>" + "\n" +
                        "</MSV_Measures>"
                        ;

            Console.WriteLine("Finished. Now Writing the XMl");

            /*
             * Write to pointcloud_fixed.xml file in the same directory
             */

            String parentDirectory = GetParentDirectory((path.Remove(path.Length - 1)).Substring(1), 1);
                
            System.IO.File.WriteAllText(parentDirectory+"/" + "pointcloud_fixed.xml", xml);
            System.IO.File.WriteAllText(parentDirectory + "/log.txt", log);
            Console.WriteLine(System.DateTime.Now.ToShortTimeString());
            Console.ReadLine();
          
        }

        public static string GetParentDirectory(string path, int parentCount)
        {
            if (string.IsNullOrEmpty(path) || parentCount < 1)
                return path;

            string parent = System.IO.Path.GetDirectoryName(path);

            if (--parentCount > 0)
                return GetParentDirectory(parent, parentCount);

            return parent;
        }

    }
}
