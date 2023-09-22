using JorgeLanches.Context;
using JorgeLanches.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JorgeLanches.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {

        private readonly AppDbContext _Context;

        public CategoriasController(AppDbContext context)
        {
            _Context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> Get()
        {
            return await _Context.Categorias.AsNoTracking().ToListAsync();
        }

        [HttpGet("{id:int}", Name ="ObterCategoria")]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            var categoria = await _Context.Categorias.AsNoTracking().FirstOrDefaultAsync(c => c.CategoriaId == id);

            if(categoria is null) { return NotFound("Categoria não encontrada"); }

            return categoria;

        }

        [HttpGet("produtos")]
        public async  Task<ActionResult<IEnumerable<Categoria>>> GetCategoriaProdutos()
        {

            return await _Context.Categorias.AsNoTracking().Include(c => c.produtos).ToListAsync();

        }


        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {

            if (categoria is null) { return BadRequest(); }

            _Context.Categorias.Add(categoria);
            _Context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoria);

        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {

            if(id != categoria.CategoriaId) { return BadRequest(); }

            _Context.Entry(categoria).State = EntityState.Modified;
            _Context.SaveChanges();

            return Ok(categoria);

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _Context.Categorias.FirstOrDefault(c => c.CategoriaId == id);

            if(categoria is null) { return NotFound("Categoria não encontrada"); }

            _Context.Categorias.Remove(categoria);
            _Context.SaveChanges();

            return Ok(categoria);

        }




    }
}
