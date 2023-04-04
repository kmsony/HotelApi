using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        [HttpGet]
        public string Get(string hotelid, string ArrivalDate)
        {

            // Load hotel rates from the JSON file
            var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"Model\\{"hotelsrates.json"}");
            //JObject hotelRates = JObject.Parse(System.IO.File.ReadAllText(folderDetails));
            var JSON = JArray.Parse (System.IO.File.ReadAllText(folderDetails));
            //dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(JSON);

            dynamic row = new JObject();
            var objitem = JSON.OfType<dynamic>().FirstOrDefault(json => json.hotel.hotelID == hotelid);

            // Filter the hotel rates by HotelID and ArrivalDate
            //var filteredRates = hotelRates["HotelRates"]
            //    .Where(rate => (string)rate["HotelID"] == hotelid && (string)rate["targetDay"] == ArrivalDate)
            //    .ToList();

         

            if (objitem != null)
            {
                row.hotel = objitem.hotel;
                DateTime arrivalDateTime;
                if (DateTime.TryParse(ArrivalDate, out arrivalDateTime))
                {
                    var resultset = new JArray();
                    foreach (var hotelRate in objitem.hotelRates)
                    {
                        DateTime dateTime;
                        if (DateTime.TryParse(Convert.ToString(hotelRate.targetDay), out dateTime) && dateTime.Date.Equals(arrivalDateTime.Date))
                        {
                            resultset.Add(hotelRate);
                        }
                    }
                    row.hotelRates = resultset;
                }
            }
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(row);
            // Return the filtered hotel rates as a JSON string in the HTTP response
            return jsonString;
        }
    }
}
