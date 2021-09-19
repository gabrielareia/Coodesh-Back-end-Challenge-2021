using Newtonsoft.Json;

namespace CoodeshPharmaIncAPI.Models
{
    /// <summary>
    /// This class is a custom implementation that works like an array of bytes, 
    /// but it also holds other important information from the API call, like the picture's address.
    /// </summary>
    public class Picture
    {
        public int Id { get; set; } //For the database.

        /// <summary>
        /// The address of the picture to be imported and inserted into the database.
        /// This property was created to avoid creating 2 different classes, 
        /// one to import data from the random users API and one to deal with the database inside the program.
        /// </summary>
        [JsonProperty("large")]
        private string Address { get; set; } = "";

        [JsonIgnore]
        public byte[] Data { get; set; }

        [JsonIgnore]
        public int Length => Data.Length;

        public byte this[int index]
        {
            get => Data[index];
            set => Data[index] = value;
        }

        public bool SetDefaultAddress(string address)
        {
            if (string.IsNullOrEmpty(Address))
            {
                Address = address;
                return true;
            }

            return false;
        }

        //Conversion Operators:

        //Converting between Picture and byte[]
        public static implicit operator byte[](Picture p) => p == null ? new byte[0] : p.Data;
        public static implicit operator Picture(byte[] b) => new Picture() { Data = b == null ? new byte[0] : b };

        //Converting between Picture and string (the address of the image)
        public static explicit operator string(Picture p) => p.Address;
    }
}