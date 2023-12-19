using ControleDeHorasExtras.Dominio.Domain.Models;

namespace ControleDeHorasExtras.Dominio.Domain.Models.ViewModels.Response
{
    public class HorasExtrasMonthResponse
    {
        public int DiasHorasExtras { get; set; }
        public List<HoraExtra>? HorasExtras { get; set; }
    }
}
