using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatalogoService.Data;
using CatalogoService.Models;

namespace CatalogoService.Controllers
{
    [ApiController]
    [Route("catalogo/[controller]")]
    public class BuscaController : ControllerBase
    {
        private readonly CatalogoDbContext _context;

        public BuscaController(CatalogoDbContext context)
        {
            _context = context;
        }

        public record ResultadoDto(string idServico, string nome, string idProfissional);
        public record RespostaDto(List<ResultadoDto> resultados);

        [HttpGet]
        public async Task<ActionResult<RespostaDto>> BuscaSimples(
            [FromQuery] string? termo = null,
            [FromQuery] string? cidade = null)
        {
            var q = _context.Servicos.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(termo))
            {
                var like = $"%{termo.Trim()}%";
                q = q.Where(s =>
                    EF.Functions.Like(s.Nome!, like) ||
                    EF.Functions.Like(s.Descricao!, like));
            }

            var itens = await q
                .OrderBy(s => s.Nome)
                .Select(s => new ResultadoDto(
                    s.Id.ToString(),
                    s.Nome ?? string.Empty,
                    s.IdProfissional ?? string.Empty
                ))
                .ToListAsync();

            return Ok(new RespostaDto(itens));
        }

        public class BuscaAvancadaRequest
        {
            public string? servico { get; set; }
            public string? regiao { get; set; }
            public decimal? faixaPrecoMax { get; set; }
            public int? avaliacoesMinimas { get; set; }
        }

        /// <summary>
        /// POST /busca/avancada
        /// Realiza uma busca com múltiplos filtros.
        /// Campos suportados no modelo atual: servico (nome/descricao), regiao (descricao), faixaPrecoMax (preco).
        /// avaliacoesMinimas é aceito mas ignorado pois não há avaliações no modelo.
        /// </summary>
        [HttpPost("avancada")]
        public async Task<ActionResult<RespostaDto>> BuscaAvancada([FromBody] BuscaAvancadaRequest req)
        {
            var q = _context.Servicos.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(req.servico))
            {
                var like = $"%{req.servico.Trim()}%";
                q = q.Where(s =>
                    EF.Functions.Like(s.Nome!, like) ||
                    EF.Functions.Like(s.Descricao!, like));
            }

            if (!string.IsNullOrWhiteSpace(req.regiao))
            {
                var likeReg = $"%{req.regiao.Trim()}%";
                q = q.Where(s => EF.Functions.Like(s.Descricao!, likeReg));
            }

            if (req.faixaPrecoMax.HasValue)
            {
                q = q.Where(s => s.Preco <= req.faixaPrecoMax.Value);
            }

            var itens = await q
                .OrderBy(s => s.Nome)
                .Select(s => new ResultadoDto(
                    s.Id.ToString(),
                    s.Nome ?? string.Empty,
                    s.IdProfissional ?? string.Empty
                ))
                .ToListAsync();

            return Ok(new RespostaDto(itens));
        }
    }
}
