using JorgeLanches.Context;
using JorgeLanches.Models;
using JorgeLanches.Pagination;
using JorgeLanches.Repository;
using Microsoft.EntityFrameworkCore;

namespace JorgeLanches.UnitTests;

public class ProdutoRepositoryTests
{
    private AppDbContext _dbContext;
    private ProdutoRepository _produtoRepository;

    [SetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        _dbContext = new AppDbContext(options);
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();

        _produtoRepository = new ProdutoRepository(_dbContext);

        _produtoRepository.Create(GetNewProduto(1));
        _produtoRepository.Create(GetNewProduto(2));
        await _produtoRepository.CommitAsync();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public async Task GetProdutosPorCategoriaAsync_WhenCalled_ShouldReturnProductsOnlyFromChosenCategoryId()
    {
        var produtos = await _produtoRepository.GetProdutosPorCategoriaAsync(2);

        Assert.That(produtos.Where(p => p.CategoriaId == 2), Is.Not.Empty);
        Assert.That(produtos.Where(p => p.CategoriaId == 1), Is.Empty);
    }

    [Test]
    [TestCase("maior", 1, 2)]
    [TestCase("igual", 1, 1)]
    [TestCase("menor", 2, 1)]
    public async Task GetProdutosFiltroPrecoAsync_WhenCalled_ReturnExpectedResult(string criterio, int preco, int expectedResult)
    {
        var filters = new ProdutoFilterParameters { CriterioFiltroPreco = criterio, Preco = preco };
        var produtos = await _produtoRepository.GetProdutosFiltroPrecoAsync(filters);

        Assert.That(produtos.Where(p => p.Preco == expectedResult), Is.Not.Empty);
        Assert.That(produtos.Where(p => p.Preco != expectedResult), Is.Empty);
    }

    private static Produto GetNewProduto(int param)
    {
        return new Produto
        {
            Id = param,
            Nome = param.ToString(),
            Descricao = param.ToString(),
            Preco = param,
            CategoriaId = param,
            ImagemUrl = param.ToString(),
            QtdEstoque = param
        };
    }
}
