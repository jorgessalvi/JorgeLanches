using JorgeLanches.Context;
using JorgeLanches.Models;
using JorgeLanches.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JorgeLanches.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasProdutosAsync()
        {
            return await _context.Categorias.AsNoTracking().Include(c => c.Produtos).ToListAsync();
        }

        public async Task<IEnumerable<Categoria>> GetCategoriaPorNomeAsync(CategoriaFilterParameters parameters)
        {
            var categorias =  _context.Categorias.AsQueryable();
            if (!string.IsNullOrEmpty(parameters.Identificação) && categorias is not null)
            {
               categorias = categorias.Where(c => c.Nome.Contains(parameters.Identificação));
            }

            if (categorias is null)
                return null;

            var categoriasPaginado = Pagination<Categoria>.FilterPages(categorias, parameters);

            return await categoriasPaginado.ToListAsync();
        }
    }
}

