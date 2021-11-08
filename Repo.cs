using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadFileReader
{
    public sealed class Repo
    {
        public Repo()
        {
            lfiList = new List<LogFileInfo>();


        }


        public void start()
        {

            PopulateLogFiles();
            threadController = new MonitorThreadController(this);
            threadController.InitializeMonitors(lfiList);
        }
        MonitorThreadController threadController;
        public static List<LogFileInfo> lfiList { get; set; }

        public void PopulateLogFiles()
        {
            var st = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"Repo {st}");


            lock (Program.fileLock)
            {
                StreamReader sr = new StreamReader(@"/config/LogFileNames.txt");
                while (true)
                {
                    var read = sr.ReadLine();
                    if (read == null)
                        break;
                    if (read[0] == '.')
                        read = Environment.CurrentDirectory + read.Substring(1);

                    var lfi = new LogFileInfo { fullName = read };
                    lfiList.Add(lfi);

                }
                sr.Dispose();
            }
        }
    }
}
