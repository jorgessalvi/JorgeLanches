using JorgeLanches.Models;
using JorgeLanches.Pagination;
namespace JorgeLanches.UnitTests;

public class PaginationTests
{
    public List<Categoria> categorias;

    [SetUp]
    public void Setup()
    {
        categorias = new List<Categoria> { 
            new Categoria { Id = 1},
            new Categoria { Id = 2},
            new Categoria { Id = 3}};
    }

    [Test]
    public void FilterPages_ValidCollection_ReturnPaginatedItens()
    {
        var parameters = new PaginationParameters { Pagenumber = 2, PageSize = 1 };
        var categoriasPaginated = Pagination<Categoria>.FilterPages(categorias.AsQueryable(), parameters);

        Assert.That(categoriasPaginated.Count(), Is.EqualTo(1));
        Assert.That(categoriasPaginated.First().Id, Is.EqualTo(2));
    }

    [Test]
    public void FilterPages_NullCollection_ThrowsArgumentNullException()
    {
        Assert.That(() => Pagination<Categoria>.FilterPages((IQueryable<Categoria>)null, new PaginationParameters()), Throws.ArgumentNullException);
    }
}
