using JobCandidate.Data.Context;
using JobCandidate.Data.DesignPattern.RepositoryPattern;

namespace JobCandidate.Data.DesignPattern.UnitOfWorkPattern
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly JobCandidateDbContext _jobCandidateDbContext;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(JobCandidateDbContext jobCandidateDbContext)
        {
            _jobCandidateDbContext = jobCandidateDbContext;
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return _repositories[typeof(T)] as IGenericRepository<T>;
            }

            var repository = new GenericRepository<T>(_jobCandidateDbContext);
            _repositories.Add(typeof(T), repository);
            return repository;
        }

        public Task<int> CompleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
