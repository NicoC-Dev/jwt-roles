
using AutoMapper;
using jwtRoles.Data;
using Microsoft.AspNetCore.Mvc;
using jwtRoles.DTOs;
using jwtRoles.Models;
using Microsoft.EntityFrameworkCore;

namespace jwtRoles.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController: ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UsuarioController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpGet("consultar")]
    public async Task<ActionResult<List<Usuario>>> Get(){
        return await _context.Usuarios.ToListAsync();
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Post(RegistroUsuarioDTO usuarioDto)
    {
        const string cliente = "cliente";
        var usuario = _mapper.Map<Usuario>(usuarioDto);
        var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email);

        if (usuarioExistente != null)  
        {
            return BadRequest("El correo ya está registrado.");
        }
        else
        {
            //Por practicidad se crea por defecto como cliente.
            usuario.Rol = cliente;
            
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return Ok(usuario);
        }

    }

    
    
}