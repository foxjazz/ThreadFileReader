using System;

namespace ThreadFileReader
{
    class Program
    {
        static void Main(string[] args)
        {
            ready = false;
            var tr = new TestRepo();
            tr.start();
            var r = new Repo();
            r.start();
            Console.WriteLine("started app");
            Console.ReadKey();
        }
        public static readonly object fileLock = new object();
        public static bool ready;
    }
}
