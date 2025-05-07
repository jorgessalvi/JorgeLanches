using AutoMapper;
using JorgeLanches.Controllers;
using JorgeLanches.DTOs;
using JorgeLanches.Models;
using JorgeLanches.Pagination;
using JorgeLanches.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace JorgeLanches.UnitTests;

public class ProdutosControllerTests
{
    private Mock<IMapper> _mapperMock;
    private Mock<IProdutoRepository> _produtoRepositoryMock;
    private ProdutosController _produtosController;

    [SetUp]
    public void Setup()
    {
        _mapperMock = new Mock<IMapper>();
        _produtoRepositoryMock = new Mock<IProdutoRepository>();
        _produtosController = new ProdutosController(_produtoRepositoryMock.Object, _mapperMock.Object);
    }

    [Test]
    public async Task Get_WithPaginationParams_ReturnNotFound()
    {
        _produtoRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<PaginationParameters>())).ReturnsAsync(() => null);

        var result = await _produtosController.Get(new PaginationParameters());
        var statusResult = result.Result as NotFoundResult;

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
    }

    [Test]
    public async Task Get_WithPaginationParams_ReturnEnumOfProdutos()
    {      
        var result = await _produtosController.Get(new PaginationParameters());
        var statusResult = result.Result as OkObjectResult;

        Assert.That(result, Is.TypeOf<ActionResult<IEnumerable<ProdutoDTO>>>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task Get_WithId_ReturnNotFound()
    {
        var result = await _produtosController.Get(1);
        var statusResult = result.Result as NotFoundObjectResult;

        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
    }
    

    [Test]
    public async Task Get_WithId_ReturnProduto()
    {
        _produtoRepositoryMock.Setup(r => r.GetAsync(p => p.Id == 1)).ReturnsAsync(() => new Produto { Id = 1 });
        _mapperMock.Setup(m => m.Map<ProdutoDTO>(It.IsAny<Produto>())).Returns(new ProdutoDTO());

        var result = await _produtosController.Get(1);
        var statusResult = result.Result as OkObjectResult;

        Assert.That(result, Is.TypeOf<ActionResult<ProdutoDTO>>());
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task Post_NullParam_ReturnBadRequest()
    {
        var result = await _produtosController.Post((ProdutoRequestDTO)null);
        var statusResult = result.Result as BadRequestResult;

        Assert.That(result.Result, Is.TypeOf<BadRequestResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
    }

    [Test]
    public async Task Post_ValidParam_ReturnCreatedAtRouteResult()
    {
        ProdutoRequestDTO produtoRequestDTO = new ProdutoRequestDTO { Id = 1 };
        Produto produto = new Produto { Id = 1 };
        _mapperMock.Setup(m => m.Map<Produto>(produtoRequestDTO)).Returns(produto);
        _mapperMock.Setup(m => m.Map<ProdutoDTO>(produto)).Returns(new ProdutoDTO { Id = 1 });
        _produtoRepositoryMock.Setup(r => r.Create(produto)).Returns(produto);

        var result = await _produtosController.Post(produtoRequestDTO);
        var statusResult = result.Result as CreatedAtRouteResult;

        _produtoRepositoryMock.Verify(r => r.Create(produto), Times.Once);
        _produtoRepositoryMock.Verify(r => r.CommitAsync(), Times.Once);
        Assert.That(result, Is.TypeOf<ActionResult<ProdutoDTO>>());
        Assert.That(result.Result, Is.TypeOf<CreatedAtRouteResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That(statusResult.RouteName, Is.EqualTo("ObterProduto"));
    }

    [Test]
    public async Task Put_InvalidId_ReturnBadRequest()
    {
        var result = await _produtosController.Put(1, new ProdutoRequestDTO { Id = 2 });
        var statusResult = result.Result as BadRequestResult;

        Assert.That(result.Result, Is.TypeOf<BadRequestResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
    }

    [Test]
    public async Task Put_ValidId_ReturnUpdatedEntity()
    {
        ProdutoRequestDTO produtoRequestDTO = new ProdutoRequestDTO { Id = 1 };
        Produto produto = new Produto { Id = 1 };
        _mapperMock.Setup(m => m.Map<Produto>(produtoRequestDTO)).Returns(produto);
        _mapperMock.Setup(m => m.Map<ProdutoDTO>(produto)).Returns(new ProdutoDTO { Id = 1 });
        _produtoRepositoryMock.Setup(r => r.Update(produto)).Returns(produto);

        var result = await _produtosController.Put(1, produtoRequestDTO);
        var statusResult = result.Result as OkObjectResult;

        _produtoRepositoryMock.Verify(r => r.Update(produto), Times.Once);
        _produtoRepositoryMock.Verify(r => r.CommitAsync(), Times.Once);
        Assert.That(result, Is.TypeOf<ActionResult<ProdutoDTO>>());
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task Delete_InvalidId_ReturnNotFound()
    {
        var result = await _produtosController.Delete(0);
        var statusResult = result.Result as NotFoundObjectResult;

        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
    }

    [Test]
    public async Task Delete_ValidId_ReturnDeletedProduto()
    {
        Produto produto = new Produto { Id = 1 };
        _produtoRepositoryMock.Setup(r => r.GetAsync(p => p.Id == 1)).ReturnsAsync(() => produto);
        _mapperMock.Setup(m => m.Map<ProdutoDTO>(produto)).Returns(new ProdutoDTO());

        var result = await _produtosController.Delete(1);
        var statusResult = result.Result as OkObjectResult;

        _produtoRepositoryMock.Verify(r => r.Delete(produto), Times.Once);
        _produtoRepositoryMock.Verify(r => r.CommitAsync(), Times.Once);
        Assert.That(result, Is.TypeOf<ActionResult<ProdutoDTO>>());
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task GetProdutosPorCategoria_InvalidId_ReturnNotFound()
    {
        //This time i had to setup so it returns null, but on Get_WithId_ReturnNotFound it wasn't necessary. Why?
        _produtoRepositoryMock.Setup(r => r.GetProdutosPorCategoriaAsync(0)).ReturnsAsync(() => null);

        var result = await _produtosController.GetProdutoPorCategoria(0);
        var statusResult = result.Result as NotFoundResult;

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
    }

    [Test]
    public async Task GetProdutosPorCategoria_ValidId_ReturnEnumOfProduto()
    {
        _produtoRepositoryMock.Setup(r => r.GetProdutosPorCategoriaAsync(1)).ReturnsAsync(() => new List<Produto>());
        _mapperMock.Setup(m => m.Map<IEnumerable<ProdutoDTO>>(It.IsAny<IEnumerable<Produto>>())).Returns(new List<ProdutoDTO>());

        var result = await _produtosController.GetProdutoPorCategoria(1);
        var statusResult = result.Result as OkObjectResult;

        Assert.That(result, Is.TypeOf<ActionResult<IEnumerable<ProdutoDTO>>>());
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task GetProdutosFiltroPreco_InvalidParams_ReturnNotFound()
    {
        _produtoRepositoryMock.Setup(r => r.GetProdutosFiltroPrecoAsync(It.IsAny<ProdutoFilterParameters>())).ReturnsAsync(() => null);

        var result = await _produtosController.GetProdutoFiltroPreco(new ProdutoFilterParameters());
        var statusResult = result.Result as NotFoundResult;

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
    }
    [Test]
    public async Task GetProdutosFiltroPreco_ValidParams_ReturnEnumOfProduto()
    {
        _produtoRepositoryMock.Setup(r => r.GetProdutosFiltroPrecoAsync(It.IsAny<ProdutoFilterParameters>())).ReturnsAsync(() => new List<Produto>());
        _mapperMock.Setup(m => m.Map<IEnumerable<ProdutoDTO>>(It.IsAny<IEnumerable<Produto>>())).Returns(new List<ProdutoDTO>());

        var result = await _produtosController.GetProdutoFiltroPreco(new ProdutoFilterParameters());
        var statusResult = result.Result as OkObjectResult;

        Assert.That(result, Is.TypeOf<ActionResult<IEnumerable<ProdutoDTO>>>());
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }
}
