using Asp.Versioning;
using JobCandidate.Domain.DomainClasses;
using JobCandidate.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobCandidateHub.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateService _candidateService;

        public CandidateController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }
  
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateCandidate([FromBody] Candidate candidate)
        {
            if (candidate == null)
            {
                return BadRequest();
            }



            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _candidateService.AddOrUpdateCandidateAsync(candidate);
            return Ok(result);
        }
    }
}
