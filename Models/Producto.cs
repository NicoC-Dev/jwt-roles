
using System.ComponentModel.DataAnnotations;

namespace jwtRoles.Models;

public class Producto
{
    /*
        Por temas de practicidad el producto se mantiene sencillo.
    */ 
    [Key]
    public int ProductoId {get;set;}

    [Required]
    [StringLength(20)]
    public string Nombre {get;set;} = null!;

}