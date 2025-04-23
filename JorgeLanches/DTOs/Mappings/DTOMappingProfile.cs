using AutoMapper;
using JorgeLanches.Models;

namespace JorgeLanches.DTOs.Mappings
{
    public class DTOMappingProfile : Profile
    {
        public DTOMappingProfile()
        {
            CreateMap<Produto, ProdutoDTO>().ReverseMap();
            CreateMap<Produto, ProdutoRequestDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();            
        }
    }
}
