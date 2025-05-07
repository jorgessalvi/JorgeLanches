using AutoMapper;
using JorgeLanches.Context;
using JorgeLanches.DTOs;
using JorgeLanches.DTOs.Mappings;
using JorgeLanches.Models;
using JorgeLanches.Pagination;
using JorgeLanches.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace JorgeLanches.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;

        public ProdutosController(IProdutoRepository produtoRepository, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery]PaginationParameters parameters)
        {
            var produtos = await _produtoRepository.GetAllAsync(parameters);
            if(produtos is null)
            {
                return NotFound();
            }

            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDTO.ToList());
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet("{produtoId:int}", Name="ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int produtoId)
        {
            var produto =  await _produtoRepository.GetAsync(p => p.Id == produtoId);
            if (produto is null)
            {
                return NotFound("Produto Não encontrado.");
            }

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
            return Ok(produtoDTO);
        }

        [Authorize(Policy = "UserOnly")]
        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> Post(ProdutoRequestDTO produtoRequestDTO)
        {
            if (produtoRequestDTO is null)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoRequestDTO);

            _produtoRepository.Create(produto);
            await _produtoRepository.CommitAsync();

            var novoProdutoDTO = _mapper.Map<ProdutoDTO>(produto);
            return new CreatedAtRouteResult("ObterProduto", new { produtoId = novoProdutoDTO.Id }, novoProdutoDTO);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{produtoId:int}")]
        public async Task<ActionResult<ProdutoDTO>> Put(int produtoId, ProdutoRequestDTO produtoRequestDTO)
        {
            if (produtoId != produtoRequestDTO.Id)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoRequestDTO);

            _produtoRepository.Update(produto);
            await _produtoRepository.CommitAsync();

            var produtoAtualizadoDTO = _mapper.Map<ProdutoDTO>(produto);
            return Ok(produtoAtualizadoDTO);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{produtoId:int}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int produtoId)
        {
            var produto = await _produtoRepository.GetAsync(p => p.Id == produtoId);
            if (produto is null)
            {
                return NotFound("Produto Não encontrado.");
            }

            _produtoRepository.Delete(produto);
            await _produtoRepository.CommitAsync();

            var produtoRemovidoDTO = _mapper.Map<ProdutoDTO>(produto);
            return Ok(produtoRemovidoDTO);
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet("/ProdutoPorCategoria")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutoPorCategoria(int categoriaId)
        {
            var produtos = await _produtoRepository.GetProdutosPorCategoriaAsync(categoriaId);
            if (produtos is null)
            {
                return NotFound();
            }

            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDTO.ToList());            
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet("/ProdutoPorPreço")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutoFiltroPreco([FromQuery]ProdutoFilterParameters produtoFilterParameters)
        {
            var produtos = await _produtoRepository.GetProdutosFiltroPrecoAsync(produtoFilterParameters);
            if (produtos is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<ProdutoDTO>>(produtos).ToList());
        }
    }
}
