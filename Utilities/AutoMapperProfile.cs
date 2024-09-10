
using AutoMapper;
using jwtRoles.DTOs;
using jwtRoles.Models;

namespace jwtRoles.Utilities;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
          CreateMap<ProductoDTO, Producto>();
          CreateMap<RegistroUsuarioDTO, Usuario>();
    }    
}