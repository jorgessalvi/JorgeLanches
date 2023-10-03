using JorgeLanches.Context;
using JorgeLanches.Models;
using JorgeLanches.Pagination;

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

        public PagedList<Produto> GetProdutos(ProdutosParameters produtosParams)
        {
            return PagedList<Produto>.ToPagedList( Get().OrderBy(p => p.ProdutoId),
                                                   produtosParams.PageNumber,
                                                   produtosParams.PageSize);
        }
    }
}
