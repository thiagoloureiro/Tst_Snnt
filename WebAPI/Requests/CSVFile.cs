using Newtonsoft.Json;

namespace WebAPI.Requests
{
    public class CSVFile
    {
        [JsonProperty(PropertyName = "userId", NullValueHandling = NullValueHandling.Ignore)]
        public int userId { get; set; }
    }
}