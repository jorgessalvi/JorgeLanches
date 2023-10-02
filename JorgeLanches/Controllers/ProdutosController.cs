using AutoMapper;
using JorgeLanches.Context;
using JorgeLanches.DTOs;
using JorgeLanches.Models;
using JorgeLanches.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            var produtos = _UnitOfWork.ProdutoRepository.Get().ToList();

            if(produtos is null) { return NotFound(); }

            var produtosDto = _Mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDto;

        }



        [HttpGet("{id:int}", Name ="ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = _UnitOfWork.ProdutoRepository.GetById(p => p.ProdutoId ==id);
        
            if(produto is null) { return NotFound(); }

            var produtoDto = _Mapper.Map<ProdutoDTO>(produto);

            return produtoDto;
            
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosMenorPreco()
        {
            var produtos = _UnitOfWork.ProdutoRepository.GetProdutoPrecoOrdenado().ToList();
            var produtosDto = _Mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDto;
        }

        [HttpPost]
        public ActionResult Post([FromBody]ProdutoDTO produtoDto)
        {

            if (produtoDto is null)  return BadRequest();

            var produto = _Mapper.Map<Produto>(produtoDto);

            _UnitOfWork.ProdutoRepository.Add(produto);
            _UnitOfWork.Commit();

            var produtoDtoRetorno = _Mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produtoDtoRetorno);       

        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, ProdutoDTO produtoDto)
        {
            if(id != produtoDto.ProdutoId) { return BadRequest();}

            var produto = _Mapper.Map<Produto>(produtoDto);

            _UnitOfWork.ProdutoRepository.Update(produto);
            _UnitOfWork.Commit();

            return Ok();

        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {

            var produto = _UnitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto is null) { return NotFound("Produto não Encontrado."); }

            _UnitOfWork.ProdutoRepository.Delete(produto);
            _UnitOfWork.Commit();

            var produtoDto = _Mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDto);

        }


    }
}
