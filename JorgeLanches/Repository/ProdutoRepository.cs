using JorgeLanches.Context;
using JorgeLanches.Models;
using JorgeLanches.Pagination;
using System.Linq.Expressions;

namespace JorgeLanches.Repository

{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Produto>> GetProdutosFiltroPrecoAsync(ProdutoFilterParameters parameters)
        {
            var produtos = await GetAllAsync();
            if (parameters.Preco.HasValue && !string.IsNullOrEmpty(parameters.CriterioFiltroPreco))
            {
                switch (parameters.CriterioFiltroPreco)
                {
                    case ("maior"):
                        produtos = produtos.Where(p => p.Preco > parameters.Preco);
                        break;

                    case ("igual"):
                        produtos = produtos.Where(p => p.Preco == parameters.Preco);
                        break;

                    case ("menor"):
                        produtos = produtos.Where(p => p.Preco < parameters.Preco);
                        break;
                }
            }

            var produtosPaginado = Pagination<Produto>.FilterPages(produtos.AsQueryable(), parameters);

            return produtosPaginado;
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int categoriaId)
        {
            var produtos = await GetAllAsync();
            if (produtos != null)
            {
                produtos = produtos.Where(p => p.CategoriaId == categoriaId);
            }
            return produtos;
        }
    }
}
