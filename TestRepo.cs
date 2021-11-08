using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace ThreadFileReader
{
    class TestRepo
    {
        public TestRepo()
        {

        }

        public void start()
        {
            Thread newThread = new Thread(TestRepo.beginWriting);
            newThread.Start(42);
            //ThreadPool.QueueUserWorkItem(beginWriting, 1);
        }
        public static void beginWriting(object o)
        {
            
            var st = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"{st}");
            var fn = Environment.CurrentDirectory + "\\Test\\testFile5.txt";
            if(File.Exists(fn))
                File.Delete(fn);
            var x = File.Create(fn);
            x.Close();


            Console.WriteLine($"file created: {fn}");
            int xcnt = 0;
            int sleepX = 100;
            while (true)
            {
                Thread.Sleep(sleepX);
                lock (Program.fileLock)
                {
                    try
                    {
                        using (var fs = new FileStream(fn, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                        {

                            fs.Write($"write {DateTime.Now} \r\n".ToBytes());
                            fs.Flush();



                            if (xcnt >= 10)
                            {
                                sleepX = 2000;
                                Program.ready = true;
                            }
                            Console.WriteLine($"writing test file {xcnt++}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"error writing to file: {ex.Message}");
                        
                    }
                }
            }



        }
    }
}
