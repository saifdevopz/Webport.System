//using Blazor.Common;
//using Microsoft.AspNetCore.Components.Authorization;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;

//namespace Blazor.Server.Authentication;

//public class CustomAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor) : AuthenticationStateProvider
//{
//    public override Task<AuthenticationState> GetAuthenticationStateAsync()
//    {
//        try
//        {
//            if (httpContextAccessor.HttpContext!.Request.Cookies.ContainsKey(BlazorConstants.AuthCookieName))
//            {
//                var token = httpContextAccessor.HttpContext.Request.Cookies[BlazorConstants.AuthCookieName];
//                var handler = new JwtSecurityTokenHandler();
//                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
//                var claims = new List<Claim>();
//                foreach (var claim in jsonToken!.Claims)
//                {
//                    claims.Add(new Claim(claim.Type, claim.Value));
//                }

//                var claimsIdentity = new ClaimsIdentity(claims, "jwt");
//                var user = new ClaimsPrincipal(claimsIdentity);
//                return Task.FromResult(new AuthenticationState(user));
//            }
//            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
//        }
//        catch (Exception)
//        {
//            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
//        }
//    }
//}
