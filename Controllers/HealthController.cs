using Microsoft.AspNetCore.Mvc;

namespace CatalogoService.Controllers
{
    [ApiController]
    [Route("[controller]")] // Rota será /health
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            // Este endpoint simplesmente retorna 200 OK se a API estiver funcionando.
            return Ok("Status: Healthy");
        }
    }
}
