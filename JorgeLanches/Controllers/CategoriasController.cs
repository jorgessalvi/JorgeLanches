using AutoMapper;
using JorgeLanches.Context;
using JorgeLanches.DTOs;
using JorgeLanches.DTOs.Mappings;
using JorgeLanches.Filters;
using JorgeLanches.Models;
using JorgeLanches.Pagination;
using JorgeLanches.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace JorgeLanches.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaRepository categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAsync([FromQuery]PaginationParameters paginationParameters)
        {
            var categorias = await _categoriaRepository.GetAllAsync(paginationParameters);
            if (categorias is null)
            {
                return NotFound();
            }
            var categoriasDTO = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
            return Ok(categoriasDTO);
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet("{categoriaId:int}", Name ="ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> GetAsync(int categoriaId)
        {
            var categoria = await _categoriaRepository.GetAsync(c => c.Id == categoriaId);
            if (categoria is null)
            {
                return NotFound("Categoria Não encontrada");
            }
            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
            return Ok(categoriaDTO);
        }

        [EnableRateLimiting(policyName:"FixedWindow")]
        [Authorize(Policy = "UserOnly")]
        [HttpGet("/CategoriaProdutos")]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
        {
            var categorias = await _categoriaRepository.GetCategoriasProdutosAsync();
            if (categorias is null)
            {
                return NotFound();
            }

            return Ok(categorias);
        }

        [Authorize(Policy = "UserOnly")]
        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null)
            {
                return BadRequest();
            }

            var categoria = _mapper.Map<Categoria>(categoriaDTO);

            _categoriaRepository.Create(categoria);
            await _categoriaRepository.CommitAsync();

            var novaCategoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return new CreatedAtRouteResult("ObterCategoria",
                                    new { categoriaId = novaCategoriaDTO.Id },
                                    novaCategoriaDTO);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{categoriaId:int}")]
        public async Task<ActionResult<CategoriaDTO>> Put(int categoriaId, CategoriaDTO categoriaDTO)
        {
            if (categoriaId != categoriaDTO.Id)
            {
                return BadRequest();
            }

            var categoria = _mapper.Map<Categoria>(categoriaDTO);

            _categoriaRepository.Update(categoria);
            await _categoriaRepository.CommitAsync();

            var categoriaAlteradaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaAlteradaDTO);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("categoriaId:int")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int categoriaId)
        {
            var categoria = await _categoriaRepository.GetAsync(c => c.Id == categoriaId);
            if (categoria is null)
            {
                return NotFound();
            }

            _categoriaRepository.Delete(categoria);
            await _categoriaRepository.CommitAsync();

            var categoriaRemovidaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaRemovidaDTO);
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet("/CategoriaPorNome")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriaPorNomeAsync([FromQuery]CategoriaFilterParameters parameters)
        {
            var categorias = await _categoriaRepository.GetCategoriaPorNomeAsync(parameters);
            if (categorias is null)
            {
                return NotFound();
            }
            var categoriasDTO = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias).ToList();
            return Ok(categoriasDTO);
        }
    }
}
