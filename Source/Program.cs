using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace FileRenameToDate
{
    class Program
    {

        static void Main(string[] args)
        {
            string dateformat = "yyyy-MM-dd-HH-mm-ss";
            WriteLine("FileRenameToDate by pftq ~ www.pftq.com ~ Jan. 2023");
            WriteLine("This program renames files in the same directory to the date created, along with optional time shift.");
            WriteLine("File name format will be '" + dateformat+"'.");
            WriteLine("Running from: " + Process.GetCurrentProcess().MainModule.FileName);
            WriteLine("");
            WriteLine("Enter the extension of files to rename (jpg, wav, etc): ");
            string extension = Console.ReadLine().Trim();
            WriteLine("Enter the number of days to add to the date created: ");
            int days = 0;
            try { days = Convert.ToInt32(Console.ReadLine().Trim()); }
            catch { WriteLine("Invalid number, defaulting to 0."); }
            WriteLine("Enter the number of hours to add to the date created: ");
            int hours = 0;
            try { hours = Convert.ToInt32(Console.ReadLine().Trim()); }
            catch { WriteLine("Invalid number, defaulting to 0."); }
            WriteLine("Enter the number of minutes to add to the date created: ");
            int minutes = 0;
            try { minutes = Convert.ToInt32(Console.ReadLine().Trim()); }
            catch { WriteLine("Invalid number, defaulting to 0."); }
            WriteLine("Enter the number of seconds to add to the date created: ");
            int seconds = 0;
            try { seconds = Convert.ToInt32(Console.ReadLine().Trim()); }
            catch { WriteLine("Invalid number, defaulting to 0."); }
            WriteLine("Press enter to start.");
            Console.ReadLine();
            Thread.Sleep(1000);


            Dictionary<string, DateTime> unresponsive = new Dictionary<string, DateTime>();
            if (true)
            {
                Stack<string> nextDir = new Stack<string>();
                nextDir.Push(Path.GetFullPath(Directory.GetCurrentDirectory()));
                while(nextDir.Any())
                {
                    
                    string dir = nextDir.Pop();
                    foreach (string d in Directory.GetDirectories(dir))
                    {
                        nextDir.Push(d);
                    }

                    WriteLine("Checking folder " + dir, false);
                    foreach(string f in Directory.GetFiles(dir, "*."+extension))
                    {
                        if (f == Process.GetCurrentProcess().MainModule.FileName) continue;
                        string file = Path.GetFileName(f);
                        if (file.Contains("driver.exe")) continue;
                        string process = Path.GetFileNameWithoutExtension(f);
                        DateTime filedate = File.GetCreationTime(f);
                        DateTime newDate = filedate.AddDays(days).AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);
                        string oldFile = Path.Combine(Path.GetDirectoryName(f), filedate.ToString(dateformat)+"."+extension);
                        string newFile = Path.Combine(Path.GetDirectoryName(f), newDate.ToString(dateformat) + "." + extension);
                        WriteLine(" - Renaming file " + file + " => " + (days+hours+minutes+seconds == 0 ? oldFile : filedate.ToString(dateformat)+" (+" + seconds + ") => " + newFile));
                        
                        try
                        {
                            System.IO.File.Move(f, newFile);
                        }
                        catch (Exception e)
                        {
                            WriteLine("Error for "+f+":\n" + e);
                        }
                    }
                }


                WriteLine("Done!");
                Console.Read();
            }
        }
        static void WriteLine(string s, bool log=true)
        {
            s = DateTime.Now + ": " + s;
            Console.WriteLine(s);
        }
    }
}
