using System;
using System.IO;
using System.Text;

namespace BrowserNativeMessages
{
    class Program
    {
        static Stream stdin = Console.OpenStandardInput();
        static Stream stdout = Console.OpenStandardOutput();
        static Stream stderr = Console.OpenStandardError();

        static void Main(string[] args)
        {
            byte[] buff = Encoding.UTF8.GetBytes("Begin Browser Native Messages Sample");
            stderr.Write(buff, 0, buff.Length);
            stderr.Flush();
            string JoMessage = GetMessage();
            while (JoMessage != null)
            {
                Console.WriteLine("Received " + JoMessage);
                JoMessage = GetMessage();
            }
        }

        static string GetMessage()
        {
            int len;
            byte[] buff = new byte[4];
            if (stdin.Read(buff, 0, 4) != 4)
                return null;
            len = BitConverter.ToInt32(buff, 0);
            buff = new byte[len];
            if (stdin.Read(buff, 0, len) != len)
                return null;
            return Encoding.UTF8.GetString(buff);
        }
    }
}
