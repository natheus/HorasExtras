namespace ControleDeHorasExtras.Dominio.Domain.Models
{
    public class HoraExtra : BaseEntity
    {
        public HoraExtra(DateTime horarioInicial, DateTime horarioFinal, decimal porcentagem)
        {
            if (horarioInicial >= horarioFinal)
                throw new ArgumentException("O horário inicial deve ser anterior ao horário final.");

            if (porcentagem != 100 && porcentagem != 150)
                throw new ArgumentException("A porcentagem deve ser 100 ou 150.");

            HorarioInicial = horarioInicial;
            HorarioFinal = horarioFinal;
            Porcentagem = porcentagem;
        }

        public DateTime HorarioInicial { get; set; }
        public DateTime HorarioFinal { get; set; }
        public decimal Porcentagem { get; set; }
    }
}