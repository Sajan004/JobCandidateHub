using JobCandidate.Data.DesignPattern.RepositoryPattern;

namespace JobCandidate.Data.DesignPattern.UnitOfWorkPattern
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : class;
        Task<int> CompleteAsync();
    }
}
