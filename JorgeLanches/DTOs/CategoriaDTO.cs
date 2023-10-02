using JorgeLanches.Models;
using System.ComponentModel.DataAnnotations;

namespace JorgeLanches.DTOs
{
    public class CategoriaDTO
    {

        public int CategoriaId { get; set; }

        public string? Nome { get; set; }

        public string? ImagemUrl { get; set; }

        public ICollection<ProdutoDTO>? produtos { get; set; }
    }
}
