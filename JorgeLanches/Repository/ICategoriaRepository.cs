using JorgeLanches.Models;
using JorgeLanches.Pagination;
using System.Collections;

namespace JorgeLanches.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        IEnumerable<Categoria> GetCategoriasProdutos();
        PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParams);
    }
}
