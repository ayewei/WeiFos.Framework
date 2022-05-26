using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WeiFos.API.Code
{

    /// <summary>
    /// 描 述：JWT自定义Token验证
    /// 创建人：叶委
    /// 日 期：2019.01.10
    /// https://docs.microsoft.com/zh-cn/dotnet/api/system.identitymodel.tokens.securitytokenhandler.validatetoken?redirectedfrom=MSDN&view=netframework-4.8#System_IdentityModel_Tokens_SecurityTokenHandler_ValidateToken_System_IdentityModel_Tokens_SecurityToken_
    /// </summary>
    public class WeiFosTokenValidator : ISecurityTokenValidator
    {
        bool ISecurityTokenValidator.CanValidateToken => true;

        int ISecurityTokenValidator.MaximumTokenSizeInBytes { get; set; }

        bool ISecurityTokenValidator.CanReadToken(string securityToken)
        {
            return true;
        }

        //验证token
        ClaimsPrincipal ISecurityTokenValidator.ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            validatedToken = null;

            //validationParameters.TOKEN
            //TODO ValidateToken 
            if (securityToken != "abcdefg")
                return null;

            //给Identity赋值
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim("name", "wyt"));
            identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, "admin"));

            var principle = new ClaimsPrincipal(identity);
            return principle;
        }
    }

}
