using Newtonsoft.Json;

namespace CoodeshPharmaIncAPI.Models
{
    public class Location
    {
        public int Id { get; set; }

        [JsonIgnore]
        public string Street { get; set; }
        [JsonIgnore]
        public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public UserTimeZone TimeZone { get; set; }
    }


}
