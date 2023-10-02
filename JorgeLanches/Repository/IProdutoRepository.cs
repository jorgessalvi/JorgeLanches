using JorgeLanches.Models;
using JorgeLanches.Pagination;

namespace JorgeLanches.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutoPrecoOrdenado();
        PagedList<Produto> GetProdutos(ProdutosParameters produtosParams);
    }
}
