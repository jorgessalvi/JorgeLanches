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
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriaParameters)
        {
            var categorias = await _UnitOfWork.CategoriaRepository.GetCategorias(categoriaParameters);

            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            var categoriasDto = _Mapper.Map<List<CategoriaDTO>>(categorias);

            return categoriasDto;
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            var categoria = await _UnitOfWork.CategoriaRepository.GetById(c => c.CategoriaId == id);

            if (categoria is null) { return NotFound("Categoria não encontrada"); }

            var categoriaDto = _Mapper.Map<CategoriaDTO>(categoria);

            return categoriaDto;
        }

        [HttpGet("produtos")]
        public async  Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriaProdutos()
        {
            var categorias = await _UnitOfWork.CategoriaRepository.GetCategoriasProdutos();

            var categoriasDto = _Mapper.Map<List<CategoriaDTO>>(categorias);

            return categoriasDto;
        }        

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null) { return BadRequest(); }

            var categoria = _Mapper.Map<Categoria>(categoriaDto);

            _UnitOfWork.CategoriaRepository.Add(categoria);
            await _UnitOfWork.Commit();

            var categoriaDtoRetorno = _Mapper.Map<CategoriaDTO>(categoria);

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoriaDtoRetorno);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CategoriaDTO categoriaDto)
        {
            if(id != categoriaDto.CategoriaId) { return BadRequest(); }

            var categoria = _Mapper.Map<Categoria>(categoriaDto);

            _UnitOfWork.CategoriaRepository.Update(categoria);
            await _UnitOfWork.Commit();

            return Ok(categoriaDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _UnitOfWork.CategoriaRepository.GetById(c => c.CategoriaId == id);

            if(categoria is null) { return NotFound("Categoria não encontrada"); }            

            _UnitOfWork.CategoriaRepository.Delete(categoria);
            await _UnitOfWork.Commit();

            var categoriaDto = _Mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDto);
        }




    }
}
