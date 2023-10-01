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
    public class CategoriasController : ControllerBase
    {

        private readonly IUnitOfWork _UnitOfWork;

        public CategoriasController(IUnitOfWork context)
        {
            _UnitOfWork = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _UnitOfWork.CategoriaRepository.Get().ToList();
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _UnitOfWork.CategoriaRepository.GetById(c => c.CategoriaId == id);

            if (categoria is null) { return NotFound("Categoria não encontrada"); }

            return categoria;

        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriaProdutos()
        {

            return _UnitOfWork.CategoriaRepository.GetCategoriasProdutos().ToList();

        }        


        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {

            if (categoria is null) { return BadRequest(); }

            _UnitOfWork.CategoriaRepository.Add(categoria);

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoria);

        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {

            if(id != categoria.CategoriaId) { return BadRequest(); }

            _UnitOfWork.CategoriaRepository.Update(categoria);
            _UnitOfWork.Commit();

            return Ok(categoria);

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _UnitOfWork.CategoriaRepository.GetById(c => c.CategoriaId == id);

            if(categoria is null) { return NotFound("Categoria não encontrada"); }

            _UnitOfWork.CategoriaRepository.Delete(categoria);
            _UnitOfWork.Commit();

            return Ok(categoria);

        }




    }
}
