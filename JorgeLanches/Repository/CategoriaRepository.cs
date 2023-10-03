using JorgeLanches.Context;
using JorgeLanches.Models;
using JorgeLanches.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace JorgeLanches.Repository
{
    public class CategoriaRepository : Repository<Categoria> , ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParams)
        {
            return PagedList<Categoria>.ToPagedList( Get().OrderBy(c => c.CategoriaId),
                                                     categoriasParams.PageNumber,
                                                     categoriasParams.PageSize);
        }

        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            return Get().Include(c => c.produtos);
        }
    }
}
