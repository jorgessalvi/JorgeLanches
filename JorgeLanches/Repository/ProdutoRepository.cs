using JorgeLanches.Context;
using JorgeLanches.Models;

namespace JorgeLanches.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Produto> GetProdutoPrecoOrdenado()
        {
            return Get().OrderBy(p => p.Preco);
        }
    }
}
