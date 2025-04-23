namespace JorgeLanches.Pagination
{
    public class ProdutoFilterParameters : PaginationParameters
    {
        public decimal? Preco { get; set; }
        public string? CriterioFiltroPreco { get; set; } //"maior", "menor" ou "igual"
    }
}
