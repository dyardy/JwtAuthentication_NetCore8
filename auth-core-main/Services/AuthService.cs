using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthCore.Helpers;
using AuthCore.Models;
using Microsoft.IdentityModel.Tokens;

namespace AuthCore.Services;

public class AuthService
{
    public string GenerateToken(User user)
    {
        // Fetch the user record from the database
        // var storedUser = _userRepository.GetByEmail(user.Email);
        //
        // // Validate the provided password
        // if (!ValidatePassword(user.Password, storedUser.Password))
        // {
        //     throw new Exception("Invalid password");
        // }

        // Continue with token generation as before...
        
        
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AuthSettings.PrivateKey);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(user),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = credentials,
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Name, user.Email));

        foreach (var role in user.Roles)
            claims.AddClaim(new Claim(ClaimTypes.Role, role));

        return claims;
    }
    
    private bool ValidatePassword(string providedPassword, string storedPassword)
    {
        // Implement your password validation logic here
        // This typically involves hashing the provided password
        // and comparing it with the stored hashed password
        return true;
    }    
}
