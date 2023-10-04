using JorgeLanches.Models;
using JorgeLanches.Pagination;

namespace JorgeLanches.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutoPrecoOrdenado();
        Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParams);
    }
}
