using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            JObject JoMessage = GetMessage();
            while (JoMessage != null)
            {
                SendMessage(JoMessage.ToString());
                JoMessage = GetMessage();
            }
        }

        static JObject GetMessage()
        {
            int len;
            byte[] buff = new byte[4];
            if (stdin.Read(buff, 0, 4) != 4)
                return null;
            len = BitConverter.ToInt32(buff, 0);
            buff = new byte[len];
            if (stdin.Read(buff, 0, len) != len)
                return null;
            string buffer = Encoding.UTF8.GetString(buff);
            return (JObject)JsonConvert.DeserializeObject<JObject>(buffer);
        }

        static void SendMessage(JToken data)
        {
            var json = new JObject();
            json["received"] = data;
            string EncodedMessage = json.ToString(Formatting.None);
            byte[] buff = Encoding.UTF8.GetBytes(EncodedMessage);
            stdout.Write(BitConverter.GetBytes(buff.Length), 0, 4);
            stdout.Write(buff, 0, buff.Length);
            stdout.Flush();
        }
    }
}
