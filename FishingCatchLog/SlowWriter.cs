using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FishingCatchLog
{
    public class SlowWriter
    {
        public static void WriteLine(string text)
        {
            foreach (char letter in text)
            {
                Console.Write(letter);
                Thread.Sleep(10);
            }
            Console.WriteLine();
        }


        public static void Write(string text)
        {
            foreach (char letter in text)
            {
                Console.Write(letter);
                Thread.Sleep(10);
            }
        }
    }
}
