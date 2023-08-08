namespace ControleDeHorasExtras.Models
{
    public class HoraExtra
    {
        public int Id { get; set; }
        public DateTime HorarioInicial { get; set; }
        public DateTime HorarioFinal { get; set; }
        public decimal Porcentagem { get; set; }
    }
}