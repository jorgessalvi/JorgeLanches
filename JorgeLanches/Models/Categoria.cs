using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace JorgeLanches.Models
{
    public class Categoria
    {
        public Categoria()
        {
            Produtos = new Collection<Produto>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(80)] 
        public string? Nome { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }

        public ICollection<Produto>? Produtos { get; set; }

    }
}
