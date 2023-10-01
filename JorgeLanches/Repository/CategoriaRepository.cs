using JorgeLanches.Context;
using JorgeLanches.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace JorgeLanches.Repository
{
    public class CategoriaRepository : Repository<Categoria> , ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            return Get().Include(c => c.produtos);
        }
    }
}
