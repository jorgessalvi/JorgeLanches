using JorgeLanches.Context;
using JorgeLanches.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JorgeLanches.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _Context;

        public ProdutosController(AppDbContext context)
        {
            _Context = context;
        }



        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var listaProdutos = _Context.Produtos.ToList();

            if(listaProdutos is null) { return NotFound(); }

            return listaProdutos;

        }



        [HttpGet("{id:int}", Name ="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _Context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
        
            if(produto is null) { return NotFound(); }

            return produto;
            
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {

            if (produto is null)  return BadRequest();

            _Context.Produtos.Add(produto);
            _Context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);
        

        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if(id != produto.ProdutoId) { return BadRequest();}

            _Context.Entry(produto).State = EntityState.Modified;
            _Context.SaveChanges();

            return Ok(produto);

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {

            var produto = _Context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if (produto is null) { return NotFound("Produto não Encontrado."); }

            _Context.Produtos.Remove(produto);
            _Context.SaveChanges();

            return Ok(produto);

        }


    }
}
