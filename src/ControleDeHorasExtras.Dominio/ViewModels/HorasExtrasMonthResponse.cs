using ControleDeHorasExtras.Models;

namespace ControleDeHorasExtras.ViewModels
{
    public class HorasExtrasMonthResponse
    {
        public int DiasHorasExtras { get; set; }
        public List<HoraExtra>? HorasExtras { get; set; }
    }
}
