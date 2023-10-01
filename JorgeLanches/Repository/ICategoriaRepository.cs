using JorgeLanches.Models;
using System.Collections;

namespace JorgeLanches.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        IEnumerable<Categoria> GetCategoriasProdutos();
    }
}
