using JorgeLanches.Context;
using JorgeLanches.Models;
using JorgeLanches.Pagination;
using JorgeLanches.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JorgeLanches.UnitTests;

public class CategoriaRepositoryTests
{
    private AppDbContext _dbContext;
    private CategoriaRepository _categoriaRepository;

    [SetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        _dbContext = new AppDbContext(options);
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();

        _categoriaRepository = new CategoriaRepository(_dbContext);

        _categoriaRepository.Create(CreateNewCategoria(1));
        _categoriaRepository.Create(CreateNewCategoria(2));
        await _categoriaRepository.CommitAsync();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public async Task GetCategoriaPorNomeAsync_NameNotFound_ReturnNull()
    {
        var filtro = new CategoriaFilterParameters { Identificação = "a" };
        var categorias = await _categoriaRepository.GetCategoriaPorNomeAsync(filtro);

        Assert.IsNull(categorias);
    }

    [Test]
    public async Task GetCategoriaPorNomeAsync_WhenCalled_ReturnCategoriasFilteredByName()
    {
        var filtro = new CategoriaFilterParameters { Identificação = "1" };
        var categorias = await _categoriaRepository.GetCategoriaPorNomeAsync(filtro);

        Assert.That(categorias.First().Nome, Does.Contain("1"));
        Assert.That(categorias.Count(), Is.EqualTo(1));
    }

    private static Categoria CreateNewCategoria(int param)
    {
        return new Categoria
        {
            Id = param,
            ImagemUrl = param.ToString(),
            Nome = param.ToString()
        };
    }
}
