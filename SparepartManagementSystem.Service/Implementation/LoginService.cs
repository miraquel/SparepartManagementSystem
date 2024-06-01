using System.DirectoryServices.Protocols;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.Service.Implementation;

public class LoginService: ILoginService
{
    private readonly IConfiguration _configuration;

    public LoginService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool LoginWithUsernameAndPassword(string username, string password)
    {
        try
        {
            var server = _configuration.GetSection("ActiveDirectory:ConnectionString").Value ?? string.Empty;
            using var ldapConnection = new LdapConnection(server);

            ldapConnection.AuthType = AuthType.Basic;
            ldapConnection.Credential = new NetworkCredential($"ax-gmk\\{username}", password);

            ldapConnection.Bind();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return true;
    }

    public string GenerateToken(User user, bool isRefreshToken = false)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value ??
                                         throw new InvalidOperationException(
                                             "Jwt Configuration not set properly, missing private key"));

        var accessTokenExpiration = _configuration["Jwt:AccessTokenExpiration"] ?? throw new InvalidOperationException("Jwt Configuration not set properly, missing Access Token Expiration");
        var refreshTokenExpiration = _configuration["Jwt:RefreshTokenExpiration"] ?? throw new InvalidOperationException("Jwt Configuration not set properly, missing Refresh Token Expiration");

        var expiresSeconds = int.Parse(isRefreshToken ? refreshTokenExpiration : accessTokenExpiration);

        var expires = DateTime.UtcNow.AddSeconds(expiresSeconds);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userid", user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.Username),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            }),
            Expires = expires,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration.GetSection("Jwt:Issuer").Value ??
                     throw new InvalidOperationException("Jwt Configuration not set properly, missing issuer"),
            Audience = _configuration.GetSection("Jwt:Audience").Value ??
                       throw new InvalidOperationException("Jwt Configuration not set properly, missing audience"),
            IssuedAt = DateTime.UtcNow
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var jwt = tokenHandler.WriteToken(token);
        return jwt;
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value ??
                                         throw new InvalidOperationException(
                                             "Jwt Configuration not set properly, missing private key"));
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = _configuration.GetSection("Jwt:Issuer").Value ??
                          throw new InvalidOperationException("Jwt Configuration not set properly, missing issuer"),
            ValidateAudience = true,
            ValidAudience = _configuration.GetSection("Jwt:Audience").Value ??
                            throw new InvalidOperationException("Jwt Configuration not set properly, missing audience"),
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero
        };

        var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}