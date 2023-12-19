namespace ControleDeHorasExtras.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
