using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using jwtRoles.Models;
using Microsoft.IdentityModel.Tokens;

namespace jwtRoles.Services;

public class AuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerarJwtToken(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)); //Agrego ! para decir que no va a ser nulo
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //Con las claims incluyo informacion en el toquen creo el token.
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"), //Nombre complejo
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email), //Mail
            new Claim(ClaimTypes.Role, usuario.Rol) //rol
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds

        );

        return new JwtSecurityTokenHandler().WriteToken(token);


    }


}