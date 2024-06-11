using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CustomClaimsPOC3.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ContextItemKeys
    {
        JwtTokenClaims
    }
}
