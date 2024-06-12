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
        static int writingSpeed = 1;

        public static void WriteLine(string text)
        {
            foreach (char letter in text)
            {
                Console.Write(letter);
                Thread.Sleep(writingSpeed);
            }
            Console.WriteLine();
        }


        public static void Write(string text)
        {
            foreach (char letter in text)
            {
                Console.Write(letter);
                Thread.Sleep(writingSpeed);
            }
        }
    }
}
