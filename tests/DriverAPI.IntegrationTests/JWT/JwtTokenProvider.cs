using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DriverAPI.IntegrationTests;

public static class JwtTokenProvider
{
    public static string Issuer { get; } = "www.imfcontainer.com";
    public static SecurityKey SecurityKey { get; } =
        new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes("0)(fACrAOGA^3#6zAUCA@IA*IA*FA%NB>6&{3$7tBPWB8wBQOC>#@!3^9tDN5*{XD9qDK{~{")
        );
    public static SigningCredentials SigningCredentials { get; } =
        new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
    internal static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();
}