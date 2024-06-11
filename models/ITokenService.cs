
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace CustomClaimsPOC3.Models
{
    public interface ITokenService
    {
        public Task<TokenClaims> ValidateToken(HttpRequestData request);
        public void AddToContext(FunctionContext context, TokenClaims claims);
        public TokenClaims GetClaims(FunctionContext context);

    }
}