using Microsoft.AspNetCore.Mvc;
using CatalogoService.Data;
using CatalogoService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoService.Controllers
{
    [ApiController]
    [Route("catalogo/[controller]")] // Rota será /catalogo/profissionais
    public class ProfissionaisController : ControllerBase
    {
        private readonly CatalogoDbContext _context;

        public ProfissionaisController(CatalogoDbContext context)
        {
            _context = context;
        }

        // GET /catalogo/profissionais/{idProfissional}/servicos
        [HttpGet("{idProfissional}/servicos")]
        public async Task<ActionResult<IEnumerable<Servico>>> GetServicosPorProfissional(string idProfissional)
        {
            // Usamos LINQ para consultar o banco de dados.
            // É como escrever: "SELECIONE todos os serviços ONDE a coluna IdProfissional é igual ao valor que recebemos".
            var servicos = await _context.Servicos
                                         .Where(s => s.IdProfissional == idProfissional)
                                         .ToListAsync();
            
            // Para uma busca de lista, nunca retornamos 404 Not Found.
            // Se nenhum serviço for encontrado, simplesmente retornamos uma lista vazia com um status 200 OK.
            return Ok(servicos);
        }
    }
}
