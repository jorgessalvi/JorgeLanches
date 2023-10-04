using JorgeLanches.Context;
using JorgeLanches.Models;
using JorgeLanches.Pagination;
using Microsoft.EntityFrameworkCore;

namespace JorgeLanches.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Produto>> GetProdutoPrecoOrdenado()
        {
            return await Get().OrderBy(p => p.Preco).ToListAsync();
        }

        public async Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParams)
        {
            return await PagedList<Produto>.ToPagedList( Get().OrderBy(p => p.ProdutoId),
                                                   produtosParams.PageNumber,
                                                   produtosParams.PageSize);
        }
    }
}
