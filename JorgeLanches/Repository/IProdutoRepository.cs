using JorgeLanches.Models;
using JorgeLanches.Pagination;

namespace JorgeLanches.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int categoriaId);
        Task<IEnumerable<Produto>> GetProdutosFiltroPrecoAsync(ProdutoFilterParameters parameters);
    }
}
