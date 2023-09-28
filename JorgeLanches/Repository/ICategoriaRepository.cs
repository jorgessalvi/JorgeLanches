using JorgeLanches.Models;
using System.Collections;

namespace JorgeLanches.Repository
{
    public interface ICategoriaRepository
    {
        IEnumerable<Categoria> GetCategoriasProdutos();
    }
}
