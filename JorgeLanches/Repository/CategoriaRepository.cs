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

        public async Task<PagedList<Categoria>> GetCategorias(CategoriasParameters categoriasParams)
        {
            return await PagedList<Categoria>.ToPagedList( Get().OrderBy(c => c.CategoriaId),
                                                     categoriasParams.PageNumber,
                                                     categoriasParams.PageSize);
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return await Get().Include(c => c.produtos).ToListAsync();
        }

        
    }
}
