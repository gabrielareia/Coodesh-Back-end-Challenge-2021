using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace CoodeshPharmaIncAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Gender { get; set; }
        public Name Name { get; set; }
        public Contact Contact { get; set; }
        public Login Login { get; set; }

        [JsonProperty("dob")]
        public UserDate Birthday { get; set; }
        public UserDate Registered { get; set; }

        public Picture Picture { get; set; }

        public Location Location { get; set; }
        [JsonProperty("nat")]
        public string Nationality { get; set; }

        public DateTime Imported_T { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public UserStatus Status { get; set; } = UserStatus.Published;

    }

}
