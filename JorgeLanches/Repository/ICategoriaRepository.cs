using JorgeLanches.Models;
using JorgeLanches.Pagination;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace JorgeLanches.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<IEnumerable<Categoria>> GetCategoriasProdutosAsync();
        Task<IEnumerable<Categoria>> GetCategoriaPorNomeAsync(CategoriaFilterParameters parameters);
    }
}
