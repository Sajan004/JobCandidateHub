using JobCandidate.Data.Context;
using JobCandidate.Data.DesignPattern.RepositoryPattern;

namespace JobCandidate.Data.DesignPattern.UnitOfWorkPattern
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly JobCandidateDbContext _jobCandidateDbContext;
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly object _lock = new();

        public UnitOfWork(JobCandidateDbContext jobCandidateDbContext)
        {
            _jobCandidateDbContext = jobCandidateDbContext ?? throw new ArgumentNullException(nameof(jobCandidateDbContext));
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (_repositories.TryGetValue(type, out var repository))
            {
                return (IGenericRepository<T>)repository!;
            }

            lock (_lock)
            {                
                if (!_repositories.TryGetValue(type, out repository))
                {
                    repository = new GenericRepository<T>(_jobCandidateDbContext);
                    _repositories.Add(type, repository);
                }
            }

            return (IGenericRepository<T>)repository!;
        }

        public async Task<int> CompleteAsync()
        {
            return await _jobCandidateDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _jobCandidateDbContext.Dispose();
        }
    }
}
