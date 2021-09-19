using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace CoodeshPharmaIncAPI.JsonConvertion.ContractResolver
{
    public class PictureAddressContractResolver : DefaultContractResolver
    {
        //Use this class to serialize the type with the original property names.

        //This method is called for every property while serializing.
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            //Use the name of the property instead of JsonProperty() attribute.
            property.PropertyName = property.UnderlyingName;

            return property;
        }

    }
}
