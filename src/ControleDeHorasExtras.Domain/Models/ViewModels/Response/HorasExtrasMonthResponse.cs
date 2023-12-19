namespace ControleDeHorasExtras.Domain.Models.ViewModels.Response
{
    public class HorasExtrasMonthResponse
    {
        public int DiasHorasExtras { get; set; }
        public List<HoraExtra>? HorasExtras { get; set; }
    }
}
