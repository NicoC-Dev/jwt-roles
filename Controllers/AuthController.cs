
using AutoMapper;
using jwtRoles.Data;
using jwtRoles.DTOs;
using jwtRoles.Models;
using jwtRoles.Services;
using Microsoft.AspNetCore.Mvc;

namespace jwtRoles.Controllers;


[ApiController]
[Route("Api/[Controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly AppDbContext _context;

    public AuthController(AuthService authService,AppDbContext context )
    {
        _authService = authService;
        _context = context;
        
    }

    [HttpGet("login")]
    public IActionResult Login(LoginUsuarioDTO login)
    {
        //Verifico que el mail del usuario este en la base de datos.
        var usuario = _context.Usuarios.SingleOrDefault(u => u.Email == login.Email);

        if (usuario == null || !VerificarContrasena(usuario.Contrasena, login.Contrasena))
        {
            return Unauthorized(new { message = "Email o contraseña incorrectos" });
        }

        //Genero el token
        var token = _authService.GenerarJwtToken(usuario);

        //Devuelvo el token
        return Ok(new {token});

    }

    private bool VerificarContrasena(string contrasena, string contrasenaIngresada)
    {
        // Verifico la contraseña
        return contrasena == contrasenaIngresada; // Este es un ejemplo simple, lo ideal es usar hashing seguro
    }
}