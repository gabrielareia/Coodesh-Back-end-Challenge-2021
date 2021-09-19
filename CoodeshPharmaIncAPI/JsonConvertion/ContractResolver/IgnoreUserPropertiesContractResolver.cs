using CoodeshPharmaIncAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace CoodeshPharmaIncAPI.JsonConvertion.ContractResolver
{
    public class IgnoreUserPropertiesContractResolver : DefaultContractResolver
    {
        //Use this class to ignore properties in User class when deserializing from Json.

        //This method is called for every property.
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);



            if (member.DeclaringType == typeof(User))
            {
                if (property.PropertyName == "Id" || property.PropertyName == "Location")
                {
                    property.Ignored = true;
                }
            }

            return property;
        }

    }
}
