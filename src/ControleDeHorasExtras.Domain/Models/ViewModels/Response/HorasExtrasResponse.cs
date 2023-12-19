namespace ControleDeHorasExtras.Domain.Models.ViewModels.Response
{
    public class HorasExtrasResponse
    {
        public int? Total { get; set; }
        public int? DiasTrabalhados { get; set; }
        public decimal? ValorHoraTrabalhada { get; set; }
        public decimal? GanhosEstimadosHora { get; set; }
        public decimal? GanhosEstimadosMes { get; set; }
    }
}
