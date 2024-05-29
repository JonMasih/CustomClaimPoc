
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;using System.Text.Json.Serialization;

namespace CustomClaimsPOC3.models
{
public class ExtendedTokenClaimsResponse
    {
    public Data data { get; set; }
    public ExtendedTokenClaimsResponse()
    {
        data = new Data();
    }
}
public class Data
    {
     [JsonPropertyName("@odata.type")]
    public string odatatype { get; set; }
    public List<Action> actions { get; set; }
    public Data()
    {
        odatatype = "microsoft.graph.onTokenIssuanceStartResponseData";
         actions = new List<Action>();
        actions.Add(new Action());
    }
}
public class Action
{
     [JsonPropertyName("@odata.type")]
    public string odatatype { get; set; }
    public Claims claims { get; set; }
    public Action()
    {
        odatatype = "microsoft.graph.tokenIssuanceStart.provideClaimsForToken";
        claims = new Claims();
    }
}
public class Claims
{ 
    public string correlationId { get; set; }
    public string partnerName { get; set; }
    public List<string> groupAccess { get; set; }
    public List<string> lobAccess { get; set; }

    public List<string> tinAccess { get; set; }
    public List<string> customRoles { get; set; }
    public Claims()
    {
        customRoles = new List<string>();
        groupAccess = new List<string>();
        lobAccess = new List<string>();
        tinAccess = new List<string>();
        
        }
}
}
