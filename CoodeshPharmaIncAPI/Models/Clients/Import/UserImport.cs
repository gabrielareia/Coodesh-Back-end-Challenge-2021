using Newtonsoft.Json;

namespace CoodeshPharmaIncAPI.Models.Import
{
    /// <summary>
    /// This class is supposed to be used only when importing data from RandomUserAPI, use <Class>User</Class> otherwise.
    /// </summary>
    public class UserImport : User
    {
        public string Email { get => Contact.Email; set => Contact.Email = value; }
        public string Phone { get => Contact.Phone; set => Contact.Phone = value; }
        [JsonProperty("cell")]
        public string Cellphone { get => Contact.Cellphone; set => Contact.Cellphone = value; }

        [JsonProperty("location")]
        public LocationImport LocationImport { get; set; }

        public UserImport()
        {
            Contact = new Contact();
        }

        public User ToBaseUser()
        {
            //This UserImport class is used only for mapping from the RandomUserAPI. When the mapping is done
            //we convert it to User, which is our "real class" that we will work the rest of the code with,
            //to organize the properties (remove Email, Cellphone and Phone from the main class and put it in the Contact class
            //and insert the data to the database.
            return new User()
            {
                Id = Id,
                Birthday = Birthday,
                Contact = Contact,
                Gender = Gender,
                Location = LocationImport.ToBaseLocation(),
                Login = Login,
                Name = Name,
                Nationality = Nationality,
                Picture = Picture,
                Registered = Registered
            };
        }
    }
}
