

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace jwtRoles.Models;

public class Usuario
{
    [Key]
    public int UsuarioId {get;set;}

    [Required]
    [StringLength(50)]
    public string Nombre {get;set;} = string.Empty;

    [Required]
    [StringLength(50)]
    public string Apellido {get;set;} = string.Empty;

    [Required]
    [EmailAddress]
    public string Email {get;set;} = string.Empty;

    [Required]
    [Range(2, 20)]
    public string Contrasena {get;set;} = string.Empty;

    //Rol.
    public string Rol {get;set;} = string.Empty;
    
}