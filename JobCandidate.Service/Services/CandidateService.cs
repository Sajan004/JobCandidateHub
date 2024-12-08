using JobCandidate.Data.DesignPattern.RepositoryPattern;
using JobCandidate.Data.DesignPattern.UnitOfWorkPattern;
using JobCandidate.Domain.DomainClasses;
using JobCandidate.Service.Helpers;
using JobCandidate.Service.Interfaces;
using Microsoft.Extensions.Caching.Memory;

public class CandidateService : ICandidateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Candidate> _candidateRepository;
    private readonly IMemoryCache _memoryCache;

    public CandidateService(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    {
        _unitOfWork = unitOfWork;
        _candidateRepository = unitOfWork.Repository<Candidate>();
        _memoryCache = memoryCache;
    }

    public async Task<Candidate> AddOrUpdateCandidateAsync(Candidate candidate)
    {
        if (!EmailHelper.IsValidEmail(candidate.Email))
        {
            throw new ArgumentException("Invalid email format.");
        }

        var existingCandidate = await GetCandidateFromCacheOrDbAsync(candidate.Email);

        if (existingCandidate != null)
        {
            return await UpdateCandidateAsync(existingCandidate, candidate);
        }

        return await AddCandidateAsync(candidate);
    }

    private async Task<Candidate> GetCandidateFromCacheOrDbAsync(string email)
    {
        if (_memoryCache.TryGetValue(email, out Candidate? cachedCandidate))
        {
            return cachedCandidate!;
        }

        var existingCandidate = (await _candidateRepository.GetAllAsync())
            .FirstOrDefault(c => c.Email == email);

        return existingCandidate!;
    }

    private async Task<Candidate> AddCandidateAsync(Candidate candidate)
    {
        var addedCandidate = await _candidateRepository.AddAsync(candidate);
        _memoryCache.Set(candidate.Email, addedCandidate, TimeSpan.FromDays(1));

        return addedCandidate;
    }

    private async Task<Candidate> UpdateCandidateAsync(Candidate existingCandidate, Candidate candidate)
    {
        existingCandidate.FirstName = candidate.FirstName;
        existingCandidate.LastName = candidate.LastName;
        existingCandidate.PhoneNumber = candidate.PhoneNumber;
        existingCandidate.CallTimeInterval = candidate.CallTimeInterval;
        existingCandidate.LinkedInProfile = candidate.LinkedInProfile;
        existingCandidate.GitHubProfile = candidate.GitHubProfile;
        existingCandidate.Comment = candidate.Comment;

        await _candidateRepository.UpdateAsync(existingCandidate);
        _memoryCache.Set(existingCandidate.Email, existingCandidate, TimeSpan.FromDays(1));

        return existingCandidate;
    }
}
