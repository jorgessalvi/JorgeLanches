namespace JorgeLanches.DTOs
{
    public class ProdutoRequestDTO : ProdutoDTO
    {
        public float QtdEstoque { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
