using JorgeLanches.Models;

namespace JorgeLanches.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutoPrecoOrdenado();
    }
}
