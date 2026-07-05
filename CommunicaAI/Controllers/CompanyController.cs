using CommunicaAI.DTO.Company;
using CommunicaAI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunicaAI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyProfileRepository _companyRepository;

        public CompanyController(ICompanyProfileRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [HttpGet("profiles")]
        public async Task<ActionResult<List<CompanyProfileResponse>>> GetCompanyProfiles()
        {
            var profiles = await _companyRepository.GetAllActiveAsync();
            
            var response = profiles.Select(p => new CompanyProfileResponse
            {
                Id = p.Id,
                CompanyName = p.CompanyName,
                InterviewStyle = p.InterviewStyle,
                FocusAreas = p.FocusAreas
            }).ToList();

            return Ok(response);
        }
    }
}
