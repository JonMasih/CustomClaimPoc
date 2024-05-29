using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Newtonsoft.Json;


namespace CustomClaimsPOC3.models
{
    public class TokenClaimsExtensionRequest
    {
       [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("source")]
        public string? Source { get; set; }

        [JsonProperty("data")]
        public RequestData? data { get; set; }
    }

    public class RequestData
    {
       [JsonProperty("@odata.type")]
        public string? ODatatype { get; set; }

        [JsonProperty("tenantId")]
        public string? TenantId { get; set; }

        [JsonProperty("authenticationEventListenerId")]
        public string? AuthenticationEventListenerId { get; set; }

        [JsonProperty("customAuthenticationExtensionId")]
        public string? CustomAuthenticationExtensionId { get; set; }

        [JsonProperty("authenticationContext")]
        public AuthenticationContext? AuthenticationContext { get; set; }
    }

    public class AuthenticationContext
    {
        [JsonProperty("correlationId")]
        public string? correlationId { get; set; }

        [JsonProperty("client")]
        public Client? Client { get; set; }

        [JsonProperty("protocol")]
        public string? Protocol { get; set; }

        [JsonProperty("clientServicePrincipal")]
        public ClientServicePrincipal? ClientServicePrincipal { get; set; }

        [JsonProperty("resourceServicePrincipal")]
        public ResourceServicePrincipal? ResourceServicePrincipal { get; set; }

        [JsonProperty("user")]
        public User? User { get; set; }
    }

    public class Client
    {
        [JsonProperty("ip")]
        public string? Ip { get; set; }

        [JsonProperty("locale")]
        public string? Locale { get; set; }

        [JsonProperty("market")]
        public string? Market { get; set; }
    }

    public class ClientServicePrincipal
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("appId")]
        public string? AppId { get; set; }

        [JsonProperty("appDisplayName")]
        public string? AppDisplayName { get; set; }

        [JsonProperty("displayName")]
        public string? DisplayName { get; set; }
    }

    public class ResourceServicePrincipal
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("appId")]
        public string? AppId { get; set; }

        [JsonProperty("appDisplayName")]
        public string? AppDisplayName { get; set; }

        [JsonProperty("displayName")]
        public string? DisplayName { get; set; }
    }

    public class User
    {
        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("displayName")]
        public string? DisplayName { get; set; }

        [JsonProperty("givenName")]
        public string? GivenName { get; set; }

        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("mail")]
        public string? Mail { get; set; }

        [JsonProperty("preferredLanguage")]
        public string? PreferredLanguage { get; set; }

        [JsonProperty("surname")]
        public string? Surname { get; set; }

        [JsonProperty("userPrincipalName")]
        public string? UserPrincipalName { get; set; }

        [JsonProperty("userType")]
        public string? UserType { get; set; }
    }
}