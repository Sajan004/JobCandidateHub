using JobCandidate.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace JobCandidate.Data.DesignPattern.RepositoryPattern
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly JobCandidateDbContext _jobCandidateDbContext;

        public GenericRepository(JobCandidateDbContext jobCandidateDbContext)
        {
            _jobCandidateDbContext = jobCandidateDbContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _jobCandidateDbContext.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _jobCandidateDbContext.Set<T>().AddAsync(entity);
            await _jobCandidateDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _jobCandidateDbContext.Set<T>().Update(entity);
            await _jobCandidateDbContext.SaveChangesAsync();
        }
    }
}
