using JobCandidate.Domain.DomainClasses;

namespace JobCandidate.Service.Interfaces
{
    public interface ICandidateService
    {
        Task<Candidate> AddOrUpdateCandidateAsync(Candidate candidate);
    }
}
