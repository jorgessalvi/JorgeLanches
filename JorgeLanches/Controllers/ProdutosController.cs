using AutoMapper;
using JorgeLanches.Context;
using JorgeLanches.DTOs;
using JorgeLanches.DTOs.Mappings;
using JorgeLanches.Models;
using JorgeLanches.Pagination;
using JorgeLanches.Repository;
using JorgeLanches.Repository.UnitOfWork;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery]PaginationParameters parameters)
        {
            var produtos = await _unitOfWork.ProdutoRepository.GetAllAsync(parameters);
            if(produtos is null)
            {
                return NotFound();
            }

            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return produtosDTO.ToList();
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet("{produtoId:int}", Name="ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int produtoId)
        {
            var produto =  await _unitOfWork.ProdutoRepository.GetAsync(p => p.Id == produtoId);
            if (produto is null)
            {
                return NotFound("Produto Não encontrado.");
            }

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
            return produtoDTO;
        }

        [Authorize(Policy = "UserOnly")]
        [HttpPost]
        public async Task<ActionResult> Post(ProdutoRequestDTO produtoRequestDTO)
        {
            if (produtoRequestDTO is null)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoRequestDTO);

            _unitOfWork.ProdutoRepository.Create(produto);
            await _unitOfWork.CommitAsync();

            var novoProdutoDTO = _mapper.Map<ProdutoDTO>(produto);
            return new CreatedAtRouteResult("ObterProduto", new { produtoId = novoProdutoDTO.Id }, novoProdutoDTO);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{produtoId:int}")]
        public async Task<ActionResult> Put(int produtoId, ProdutoRequestDTO produtoRequestDTO)
        {
            if (produtoId != produtoRequestDTO.Id)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoRequestDTO);

            _unitOfWork.ProdutoRepository.Update(produto);
            await _unitOfWork.CommitAsync();

            var produtoAtualizadoDTO = _mapper.Map<ProdutoDTO>(produto);
            return Ok(produtoAtualizadoDTO);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{produtoId:int}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int produtoId)
        {
            var produto = await _unitOfWork.ProdutoRepository.GetAsync(p => p.Id == produtoId);
            if (produto is null)
            {
                return NotFound("Produto Não encontrado.");
            }

            _unitOfWork.ProdutoRepository.Delete(produto);
            await _unitOfWork.CommitAsync();

            var produtoRemovidoDTO = _mapper.Map<ProdutoDTO>(produto);
            return Ok(produtoRemovidoDTO);
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet("/ProdutoPorCategoria")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutoPorCategoria(int categoriaId)
        {
            var produtos = await _unitOfWork.ProdutoRepository.GetProdutosPorCategoriaAsync(categoriaId);
            if (produtos is null)
            {
                return NotFound();
            }

            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return produtosDTO.ToList();            
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet("/ProdutoPorPreço")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutoFiltroPreco([FromQuery]ProdutoFilterParameters produtoFilterParameters)
        {
            var produtos = await _unitOfWork.ProdutoRepository.GetProdutosFiltroPrecoAsync(produtoFilterParameters);
            if (produtos is null)
            {
                return NotFound();
            }

            return _mapper.Map<IEnumerable<ProdutoDTO>>(produtos).ToList();            
        }
    }
}
