namespace ControleDeHorasExtras.Dominio.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
