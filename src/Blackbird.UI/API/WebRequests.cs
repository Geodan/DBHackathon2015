using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Blackbird.WPF.API.Models;
using Newtonsoft.Json;

namespace Blackbird.WPF.API
{
    public class WebRequests
    {
        public async Task<GeocodedResult> Geocode(string streckennetz, string kmpunkte)
        {            
            var client = new HttpClient();
            var url = string.Format("http://localhost:60687/api/geocode?railnumber={0}&kmpoint={1}", streckennetz, kmpunkte);
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<GeocodedResult>(json);
        }

        public async Task<PinPointResult> Pinpoint(string longitude, string latitude)
        {
            var client = new HttpClient();
            var url = string.Format("http://localhost:60687/api/pinpoint?longitude={0}&latitude={1}", longitude.ToString(CultureInfo.InvariantCulture), latitude.ToString(CultureInfo.InvariantCulture)).Replace(',', '.');
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PinPointResult>(json);
        }
    }
}
