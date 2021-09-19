using CoodeshPharmaIncAPI.Models.Import;
using Newtonsoft.Json;

namespace CoodeshPharmaIncAPI.Models
{
    public class RootImport
    {
        public UserImport First => results?[0];

        [JsonProperty]
        private UserImport[] results { get; set; }

        public int Length => results == null ? 0 : results.Length;

        public UserImport this[int index]
        {
            get => results[index];
            set => results[index] = value;
        }
    }
}
