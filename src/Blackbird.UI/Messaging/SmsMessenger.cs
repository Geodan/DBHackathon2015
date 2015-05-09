using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blackbird.WPF.Messaging
{
    public class SmsMessenger
    {
        public static async Task<string> SendMessage(string from, string number, string message)
        {
            var service = Environment.GetEnvironmentVariable("smsservice");
            var url = string.Format(service, from, number, message).Replace(' ', '+');
            var client = new HttpClient();            
            var response = await client.GetAsync(url);
            var resp = await response.Content.ReadAsStringAsync();
            return resp;
        }
    }
}
