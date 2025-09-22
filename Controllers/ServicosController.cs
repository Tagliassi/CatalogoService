using Microsoft.AspNetCore.Mvc;
using CatalogoService.Data;
using CatalogoService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoService.Controllers
{
    [ApiController]
    [Route("catalogo/[controller]")]
    public class ServicosController : ControllerBase
    {
        private readonly CatalogoDbContext _context;

        public ServicosController(CatalogoDbContext context)
        {
            _context = context;
        }

        // POST /catalogo/servicos
        [HttpPost]
        public async Task<ActionResult<Servico>> CriarServico(Servico servico)
        {
            if (servico == null)
            {
                return BadRequest("Dados do serviço inválidos.");
            }

            // Adiciona o novo serviço ao contexto do EF Core
            _context.Servicos.Add(servico);

            // Salva as mudanças no banco de dados de forma assíncrona
            await _context.SaveChangesAsync();

            // Retorna um status 201 Created com os dados do serviço criado
            // e um cabeçalho 'Location' com a URL para acessar o novo recurso (boa prática REST)
            return CreatedAtAction(nameof(GetServicoPorId), new { id = servico.Id }, servico);
        }

        // GET /catalogo/servicos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Servico>> GetServicoPorId(int id)
        {
            var servico = await _context.Servicos.FindAsync(id);

            if (servico == null)
            {
                return NotFound();
            }

            return servico;
        }
    }
}
