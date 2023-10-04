using AutoMapper;
using JorgeLanches.Context;
using JorgeLanches.DTOs;
using JorgeLanches.Models;
using JorgeLanches.Pagination;
using JorgeLanches.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JorgeLanches.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _Mapper;

        public ProdutosController(IUnitOfWork context, IMapper mapper)
        {
            _UnitOfWork = context;
            _Mapper = mapper;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = await _UnitOfWork.ProdutoRepository.GetProdutos(produtosParameters);

            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            if(produtos is null) { return NotFound(); }

            var produtosDto = _Mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDto;

        }



        [HttpGet("{id:int}", Name ="ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            var produto = await _UnitOfWork.ProdutoRepository.GetById(p => p.ProdutoId ==id);
        
            if(produto is null) { return NotFound(); }

            var produtoDto = _Mapper.Map<ProdutoDTO>(produto);

            return produtoDto;
            
        }

        [HttpGet("menorpreco")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosMenorPreco()
        {
            var produtos = await _UnitOfWork.ProdutoRepository.GetProdutoPrecoOrdenado();
            var produtosDto = _Mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]ProdutoDTO produtoDto)
        {

            if (produtoDto is null)  return BadRequest();

            var produto = _Mapper.Map<Produto>(produtoDto);

            _UnitOfWork.ProdutoRepository.Add(produto);
            await _UnitOfWork.Commit();

            var produtoDtoRetorno = _Mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produtoDtoRetorno);       

        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ProdutoDTO produtoDto)
        {
            if(id != produtoDto.ProdutoId) { return BadRequest();}

            var produto = _Mapper.Map<Produto>(produtoDto);

            _UnitOfWork.ProdutoRepository.Update(produto);
            await _UnitOfWork.Commit();

            return Ok();

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {

            var produto = await _UnitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto is null) { return NotFound("Produto não Encontrado."); }

            _UnitOfWork.ProdutoRepository.Delete(produto);
            await _UnitOfWork.Commit();

            var produtoDto = _Mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDto);

        }


    }
}
