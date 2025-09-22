using System.ComponentModel.DataAnnotations;

namespace CatalogoService.Models
{
    public class Servico
    {
        [Key] 
        public int Id { get; set; }

        [Required] 
        [StringLength(100)] 
        public string? Nome { get; set; }

        [StringLength(500)]
        public string? Descricao { get; set; }

        [Required]
        public decimal Preco { get; set; }
        
        public int DuracaoEmMinutos { get; set; }

        [Required]
        public string? IdProfissional { get; set; }
    }
}