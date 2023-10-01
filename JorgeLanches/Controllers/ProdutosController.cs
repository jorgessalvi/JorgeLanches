using JorgeLanches.Context;
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

        public ProdutosController(IUnitOfWork context)
        {
            _UnitOfWork = context;
        }



        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var listaProdutos = _UnitOfWork.ProdutoRepository.Get().ToList();

            if(listaProdutos is null) { return NotFound(); }

            return listaProdutos;

        }



        [HttpGet("{id:int}", Name ="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _UnitOfWork.ProdutoRepository.GetById(p => p.ProdutoId ==id);
        
            if(produto is null) { return NotFound(); }

            return produto;
            
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosMenorPreco()
        {
            return _UnitOfWork.ProdutoRepository.GetProdutoPrecoOrdenado().ToList();
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {

            if (produto is null)  return BadRequest();

            _UnitOfWork.ProdutoRepository.Add(produto);
            _UnitOfWork.Commit();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);       

        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if(id != produto.ProdutoId) { return BadRequest();}

            _UnitOfWork.ProdutoRepository.Update(produto);
            _UnitOfWork.Commit();

            return Ok(produto);

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {

            var produto = _UnitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto is null) { return NotFound("Produto não Encontrado."); }

            _UnitOfWork.ProdutoRepository.Delete(produto);
            _UnitOfWork.Commit();

            return Ok(produto);

        }


    }
}
