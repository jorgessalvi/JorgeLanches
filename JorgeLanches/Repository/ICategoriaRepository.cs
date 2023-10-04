using JorgeLanches.Models;
using JorgeLanches.Pagination;
using System.Collections;

namespace JorgeLanches.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<IEnumerable<Categoria>> GetCategoriasProdutos();
        Task<PagedList<Categoria>> GetCategorias(CategoriasParameters categoriasParams);
    }
}
