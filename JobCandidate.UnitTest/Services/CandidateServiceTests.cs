using JobCandidate.Data.DesignPattern.RepositoryPattern;
using JobCandidate.Data.DesignPattern.UnitOfWorkPattern;
using JobCandidate.Domain.DomainClasses;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace JobCandidate.Service.Tests
{
    public class CandidateServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGenericRepository<Candidate>> _mockRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly CandidateService _candidateService;

        public CandidateServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IGenericRepository<Candidate>>();
            _mockUnitOfWork.Setup(u => u.Repository<Candidate>()).Returns(_mockRepository.Object);
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _candidateService = new CandidateService(_mockUnitOfWork.Object, _memoryCache);
        }

        [Fact]
        public async Task AddCandidate()
        {
            var newCandidate = new Candidate
            {
                FirstName = "Kripalu",
                LastName = "Das",
                PhoneNumber = "1235697890",
                Email = "kripalu@test.com",
                CallTimeInterval = "9am-5pm",
                LinkedInProfile = "https://www.linkedin.com/in/kripalu",
                GitHubProfile = "https://github.com/kripalu",
                Comment = "Testing Unit Test Application"
            };

            _mockRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Candidate>());

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Candidate>()))
                .ReturnsAsync(newCandidate);

            var result = await _candidateService.AddOrUpdateCandidateAsync(newCandidate);

            Assert.NotNull(result);
            Assert.Equal(newCandidate.Email, result.Email);

            Assert.True(_memoryCache.TryGetValue(newCandidate.Email, out Candidate? cachedCandidate));
            Assert.Equal(newCandidate.Email, cachedCandidate?.Email);

            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Candidate>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCandidate()
        {
            var existingCandidate = new Candidate
            {
                Id = 1,
                FirstName = "Muskan",
                LastName = "Joshi",
                PhoneNumber = "1234567890",
                Email = "muskan@test.com",
                CallTimeInterval = "9am-5pm",
                LinkedInProfile = "https://www.linkedin.com/in/muskan",
                GitHubProfile = "https://github.com/muskan",
                Comment = "Hello Free Text Comment"
            };

            var updatedCandidate = new Candidate
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Malik",
                PhoneNumber = "9876543210",
                Email = "muskan@test.com",
                CallTimeInterval = "10am-6pm",
                LinkedInProfile = "https://www.linkedin.com/in/muskan",
                GitHubProfile = "https://github.com/muskan",
                Comment = "Let have some Fun"
            };

            _memoryCache.Set(existingCandidate.Email, existingCandidate, TimeSpan.FromMinutes(30));

            _mockRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Candidate> { existingCandidate });

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Candidate>()))
                .Returns(Task.CompletedTask);

            var result = await _candidateService.AddOrUpdateCandidateAsync(updatedCandidate);

            Assert.NotNull(result);
            Assert.Equal("Jane", result.FirstName);
            Assert.Equal("Let have some Fun", result.Comment);

            Assert.True(_memoryCache.TryGetValue(updatedCandidate.Email, out Candidate? cachedCandidate));
            Assert.Equal("Jane", cachedCandidate?.FirstName);
            Assert.Equal("Let have some Fun", cachedCandidate?.Comment);

            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Candidate>()), Times.Once);
        }

        [Fact]
        public async Task AddCandidateWhenDataExistsInCache()
        {
            var cachedCandidate = new Candidate
            {
                Id = 2,
                FirstName = "Cached",
                LastName = "Candidate",
                Email = "cached@test.com",
                PhoneNumber = "9999999999",
                CallTimeInterval = "8am-4pm",
                LinkedInProfile = "https://www.linkedin.com/in/cached",
                GitHubProfile = "https://github.com/cached",
                Comment = "Cached comment"
            };

            _memoryCache.Set(cachedCandidate.Email, cachedCandidate, TimeSpan.FromMinutes(30));

            var result = await _candidateService.AddOrUpdateCandidateAsync(cachedCandidate);

            Assert.NotNull(result);
            Assert.Equal(cachedCandidate.Email, result.Email);
            Assert.Equal("Cached", result.FirstName);

            _mockRepository.Verify(r => r.GetAllAsync(), Times.Never);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Candidate>()), Times.Never);
        }
    }
}
