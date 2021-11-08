using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Linq;

namespace ThreadFileReader
{
    class MonitorThreadController
    {
        public MonitorThreadController(Repo repo)
        {
            this.repo = repo;
            notify = new INotifySurface();
        }
        private static INotifySurface notify;
        private Repo repo;

        public static bool RetainThreading { get; set; }
        public void InitializeMonitors(List<LogFileInfo> list)
        {
            RetainThreading = true;
            int i = 0;

            foreach (var l in list)
            {
                Console.WriteLine("starting monitor thread");
                Thread newThread = new Thread(StartMonitor);
                newThread.Start(i);
                i++;
            }
        }

        public static void StartMonitor(object state)
        {
            // object array = state as object;
            try
            {
                int fileId = Convert.ToInt32(state);
                var lfi = Repo.lfiList.First(a => a.index == fileId);
                string logFileName = lfi.fullName;
                long fileLength = lfi.length;

                FileStream fs;
                var st = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine($"StartMonitor {st}");


                Console.WriteLine("Monitor started:");
                while (!Program.ready)
                {
                    Thread.Sleep(1);
                }

                fs = new FileStream(logFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                while (true)
                {

                    lock (Program.fileLock)
                    {
                        try
                        {
                            fs.Seek(fileLength, 0);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Exception seeking, {ex.Message}");
                            continue;
                        }

                        if (fileLength != fs.Length)
                        {
                            using (StreamReader sr = new StreamReader(fs))
                            {
                                bool hasData = true;
                                var lines = new List<string>();
                                while (hasData)
                                {
                                    var line = sr.ReadLine();
                                    if (line == null)
                                    {
                                        hasData = false;
                                        break;
                                    }
                                    else
                                    {
                                        lines.Add(line);
                                    }
                                }

                                Console.WriteLine("notifying change:");
                                // notify.Notify(fileId, lines.ToArray());
                                fileLength = fs.Length;
                            }
                        }
                        
                    }

                    Thread.Sleep(200);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"monitor exception {ex.Message}");
            }
        }
    }
}
