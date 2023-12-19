using System.ComponentModel.DataAnnotations;

namespace ControleDeHorasExtras.Web.DTOs
{
    public class HoraExtraDTO
    {
        [Required]
        public DateTime HorarioInicial { get; set; }
        [Required]
        public DateTime HorarioFinal { get; set; }
        [Required]
        public decimal Porcentagem { get; set; }
    }
}
