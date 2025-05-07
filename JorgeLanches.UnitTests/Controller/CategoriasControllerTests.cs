using AutoMapper;
using JorgeLanches.Controllers;
using JorgeLanches.DTOs;
using JorgeLanches.Models;
using JorgeLanches.Pagination;
using JorgeLanches.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework.Constraints;

namespace JorgeLanches.UnitTests;

public class CategoriasControllerTests
{
    private CategoriasController _categoriasController;
    private Mock<IMapper> _mapperMock;
    private Mock<ICategoriaRepository> _categoriaRepoMock;

    [SetUp]
    public void Setup()
    {
        _categoriaRepoMock = new Mock<ICategoriaRepository>();
        _mapperMock = new Mock<IMapper>();

        _categoriasController = new CategoriasController(_categoriaRepoMock.Object, _mapperMock.Object);        
    }

    [Test]
    public async Task GetAsync_WithPaginationParams_ReturnEnumOfCategoriaDTO()
    {        
        var taskCategorias = new Mock<Task<IEnumerable<Categoria>>>();
        _categoriaRepoMock.Setup(r => r.GetAllAsync(new PaginationParameters()))
                            .Returns(() => taskCategorias.Object);

       _mapperMock.Setup(m => m.Map<IEnumerable<CategoriaDTO>>(It.IsAny<IEnumerable<Categoria>>()))
                    .Returns(new List<CategoriaDTO>() { new CategoriaDTO()});

        var result = await _categoriasController.GetAsync(new PaginationParameters());
        var statusResult = result.Result as OkObjectResult;
        
        Assert.That(result, Is.TypeOf<ActionResult<IEnumerable<CategoriaDTO>>>());
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task GetAsync_WhenRepoReturnsNull_ReturnNotFound()
    {
        _categoriaRepoMock.Setup(r => r.GetAllAsync(It.IsAny<PaginationParameters>())).ReturnsAsync(() => null);

        var result = await _categoriasController.GetAsync(new PaginationParameters());
        var statusResult = result.Result as NotFoundResult;

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
    }

    [Test]
    public async Task GetAsync_WithCategoriaId_ReturnCategoria()
    {
        _categoriaRepoMock.Setup(r => r.GetAsync(c => c.Id == 1)).ReturnsAsync(() => new Categoria { Id = 1 });
        _mapperMock.Setup(m => m.Map<CategoriaDTO>(It.IsAny<Categoria>())).Returns(new CategoriaDTO());

        var result = await _categoriasController.GetAsync(1);
        var statusResult = result.Result as OkObjectResult;

        Assert.That(result, Is.TypeOf<ActionResult<CategoriaDTO>>());
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task GetAsync_WithIdWhenRepoReturnsNull_ReturnNotFound()
    {
        _categoriaRepoMock.Setup(r => r.GetAsync(c => c.Id == 1)).ReturnsAsync(() => null);

        var result = await _categoriasController.GetAsync(1);
        var statusResult = result.Result as NotFoundObjectResult;

        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
    }

    [Test]
    public async Task GetCategoriasProdutosAsync_WhenCalled_ReturnEnumOfCategorias()
    {
        var categoriaProdutos = new List<Categoria> {
            new Categoria { Produtos = new List<Produto>() } };
        _categoriaRepoMock.Setup(r => r.GetCategoriasProdutosAsync()).ReturnsAsync(() => categoriaProdutos);

        var result = await _categoriasController.GetCategoriasProdutosAsync();
        var statusResult = result.Result as OkObjectResult;

        Assert.That(result, Is.TypeOf<ActionResult<IEnumerable<Categoria>>>());
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task GetCategoriasProdutosAsync_WhenRepoReturnsNull_ReturnNotFound()
    {
        _categoriaRepoMock.Setup(r => r.GetCategoriasProdutosAsync()).ReturnsAsync(() => null);

        var result = await _categoriasController.GetCategoriasProdutosAsync();
        var statusResult = result.Result as NotFoundResult;

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
    }

    [Test]
    public async Task GetCategoriaPorNomeAsync_WhenCalled_ReturnEnumOfCategoriaDTO()
    {
        _categoriaRepoMock.Setup(r => r.GetCategoriaPorNomeAsync(It.IsAny<CategoriaFilterParameters>())).ReturnsAsync(() => new List<Categoria>());
        _mapperMock.Setup(m => m.Map<IEnumerable<CategoriaDTO>>(It.IsAny<IEnumerable<Categoria>>())).Returns(new List<CategoriaDTO>());

        var result = await _categoriasController.GetCategoriaPorNomeAsync(new CategoriaFilterParameters());
        var statusResult = result.Result as OkObjectResult;

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        Assert.That(result, Is.TypeOf<ActionResult<IEnumerable<CategoriaDTO>>>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task GetCategoriaPorNomeAsync_WhenRepoReturnsNull_ReturnNotFound()
    {
        _categoriaRepoMock.Setup(r => r.GetCategoriaPorNomeAsync(It.IsAny<CategoriaFilterParameters>())).ReturnsAsync(() => null);

        var result = await _categoriasController.GetCategoriaPorNomeAsync(new CategoriaFilterParameters());
        var statusResult = result.Result as NotFoundResult;

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
    }

    [Test]
    public async Task Delete_IdNotFound_ReturnNotFound()
    {
        _categoriaRepoMock.Setup(r => r.GetAsync(c => c.Id == 1)).ReturnsAsync(() => null);

        var result = await _categoriasController.Delete(1);
        var statusResult = result.Result as NotFoundResult;

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
    }

    [Test]
    public async Task Delete_IdFound_ReturnDeletedEntity()
    {
        var categoria = new Categoria();
        _categoriaRepoMock.Setup(r => r.GetAsync(c => c.Id == 1)).ReturnsAsync(() => categoria);
        _mapperMock.Setup(m => m.Map<CategoriaDTO>(It.IsAny<Categoria>())).Returns(new CategoriaDTO());

        var result = await _categoriasController.Delete(1);
        var statusResult = result.Result as OkObjectResult;

        _categoriaRepoMock.Verify(r => r.Delete(categoria), Times.Once);
        _categoriaRepoMock.Verify(r => r.CommitAsync(), Times.Once);
        Assert.That(result, Is.TypeOf<ActionResult<CategoriaDTO>>());
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task Put_InvalidId_ReturnBadRequest()
    {
        var result = await _categoriasController.Put(1, new CategoriaDTO { Id = 2 });
        var statusResult = result.Result as BadRequestResult;

        Assert.That(result.Result, Is.TypeOf<BadRequestResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
    }

    [Test]
    public async Task Put_ValidId_ReturnUpdatedEntity()
    {
        var categoria = new Categoria { Id = 1 };
        var categoriaDTO = new CategoriaDTO { Id = 1 };
        _mapperMock.Setup(m => m.Map<Categoria>(categoriaDTO)).Returns(categoria);
        _mapperMock.Setup(m => m.Map<CategoriaDTO>(categoria)).Returns(categoriaDTO);
        _categoriaRepoMock.Setup(r => r.Update(categoria)).Returns(categoria);

        var result = await _categoriasController.Put(1, categoriaDTO);
        var statusResult = result.Result as OkObjectResult;

        _categoriaRepoMock.Verify(r => r.Update(categoria), Times.Once);
        _categoriaRepoMock.Verify(r => r.CommitAsync(), Times.Once);
        Assert.That(result, Is.TypeOf<ActionResult<CategoriaDTO>>());
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task Post_WithNullParam_ReturnBadRequest()
    {
        var result = await _categoriasController.Post((CategoriaDTO)null);
        var statusResult = result.Result as BadRequestResult;

        Assert.That(result.Result, Is.TypeOf<BadRequestResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
    }

    [Test]
    public async Task Post_WithValidParam_ReturnCreatedAtRouteResult()
    {
        var categoria = new Categoria { Id = 1 };
        var categoriaDTO = new CategoriaDTO { Id = 1 };
        _mapperMock.Setup(m => m.Map<Categoria>(categoriaDTO)).Returns(categoria);
        _mapperMock.Setup(m => m.Map<CategoriaDTO>(categoria)).Returns(categoriaDTO);
        _categoriaRepoMock.Setup(r => r.Create(categoria)).Returns(categoria);

        var result = await _categoriasController.Post(categoriaDTO);
        var statusResult = result.Result as CreatedAtRouteResult;

        _categoriaRepoMock.Verify(r => r.Create(categoria), Times.Once);
        _categoriaRepoMock.Verify(r => r.CommitAsync(), Times.Once());
        Assert.That(result, Is.TypeOf<ActionResult<CategoriaDTO>>());
        Assert.That(result.Result, Is.TypeOf<CreatedAtRouteResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That(statusResult.RouteName, Is.EqualTo("ObterCategoria"));
    }
}


