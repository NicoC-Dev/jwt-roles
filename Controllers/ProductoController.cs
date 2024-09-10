
using AutoMapper;
using jwtRoles.Data;
using jwtRoles.DTOs;
using jwtRoles.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace jwtRoles.Controllers;

[ApiController]
[Route("Api/Productos")]
public class ProductoController : ControllerBase 
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public ProductoController(AppDbContext context, IMapper mapper)
    {
        this._context = context;
        this._mapper = mapper;
    }

    //Metodo Http Get para traer todos los productos ordenados por nombre.
    [Authorize(Roles="admin, cliente")]
    [HttpGet("listar")]
    public async Task<ActionResult<List<Producto>>> Get()
    {
        return await _context.Productos.OrderBy(p => p.Nombre).ToListAsync();
    }

    //Metodo Http Post para ingresar los productos
    [Authorize(Roles="admin")]
    [HttpPost("registrar")]
    public async Task<ActionResult> Post(ProductoDTO productoDto)
    {
        var producto = _mapper.Map<Producto>(productoDto);

        //Verifico que el producto cargado no exista previamente.
        var productoExistente =  await _context.Productos.FirstOrDefaultAsync(p => p.Nombre == producto.Nombre);
        if (productoExistente == null )
        {
            producto.Nombre = producto.Nombre.ToLower(); 
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return Ok();
            

        }
        
        // Retorno un error 400 BadRequest si ya existe el producto
        return BadRequest("Ya existe el producto");

        


    }
    [Authorize(Roles="admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, ProductoDTO productoDto)
    {
        var product = _mapper.Map<Producto>(productoDto);   

        product.ProductoId = id;

        _context.Update(product);
        await _context.SaveChangesAsync();
        return Ok();


    }
    [Authorize(Roles="admin")]
    [HttpDelete("id:int")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleteProduct = await _context.Productos.Where(p => p.ProductoId == id).ExecuteDeleteAsync();

        if(deleteProduct == 0)
        {
            return NotFound();
        }

        return NoContent();


       
    }

}