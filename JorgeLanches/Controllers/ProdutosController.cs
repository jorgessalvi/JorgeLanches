using JorgeLanches.Context;
using JorgeLanches.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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



        [HttpGet("{id:int}")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _Context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
        
            if(produto is null) { return NotFound(); }

            return produto;
            
        }

        


    }
}
