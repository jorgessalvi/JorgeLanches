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
    public class CategoriasController : ControllerBase
    {

        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _Mapper;

        public CategoriasController(IUnitOfWork context, IMapper mapper)
        {
            _UnitOfWork = context;
            _Mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categorias = _UnitOfWork.CategoriaRepository.Get().ToList();

            var categoriasDto = _Mapper.Map<List<CategoriaDTO>>(categorias);

            return categoriasDto;


        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _UnitOfWork.CategoriaRepository.GetById(c => c.CategoriaId == id);

            if (categoria is null) { return NotFound("Categoria não encontrada"); }

            var categoriaDto = _Mapper.Map<CategoriaDTO>(categoria);

            return categoriaDto;

        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriaProdutos()
        {

            var categorias = _UnitOfWork.CategoriaRepository.GetCategoriasProdutos().ToList();

            var categoriasDto = _Mapper.Map<List<CategoriaDTO>>(categorias);

            return categoriasDto;

        }        


        [HttpPost]
        public ActionResult Post(CategoriaDTO categoriaDto)
        {

            if (categoriaDto is null) { return BadRequest(); }

            var categoria = _Mapper.Map<Categoria>(categoriaDto);

            _UnitOfWork.CategoriaRepository.Add(categoria);

            var categoriaDtoRetorno = _Mapper.Map<CategoriaDTO>(categoria);

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoriaDtoRetorno);

        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, CategoriaDTO categoriaDto)
        {

            if(id != categoriaDto.CategoriaId) { return BadRequest(); }

            var categoria = _Mapper.Map<Categoria>(categoriaDto);

            _UnitOfWork.CategoriaRepository.Update(categoria);
            _UnitOfWork.Commit();

            return Ok(categoriaDto);

        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _UnitOfWork.CategoriaRepository.GetById(c => c.CategoriaId == id);

            if(categoria is null) { return NotFound("Categoria não encontrada"); }

            _UnitOfWork.CategoriaRepository.Delete(categoria);
            _UnitOfWork.Commit();

            var categoriaDto = _Mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDto);

        }




    }
}
