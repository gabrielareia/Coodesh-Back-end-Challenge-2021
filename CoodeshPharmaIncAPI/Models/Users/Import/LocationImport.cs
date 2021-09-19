using Newtonsoft.Json;

namespace CoodeshPharmaIncAPI.Models.Import
{
    /// <summary>
    /// This class is supposed to be used only when importing data from RandomUserAPI, use Client otherwise.
    /// </summary>
    public class LocationImport : Location
    {
        [JsonProperty("street")]
        private Street StreetAddress { get; set; }

        public Location ToBaseLocation()
        {
            //This LocationImport class is used only for mapping from the RandomUserAPI. When the mapping is done
            //we convert it to Location, which is our "real class" that we will work the rest of the code with,
            //to organize the properties and insert the data to the database.
            return new Location()
            {
                Id = Id,
                City = City,
                Country = Country,
                Number = StreetAddress?.Number,
                PostCode = PostCode,
                State = State,
                Street = StreetAddress?.Name,
                TimeZone = TimeZone
            };
        }

    }
}
