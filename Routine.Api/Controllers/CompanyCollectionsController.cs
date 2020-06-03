using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Entities;
using Routine.Api.Helpers;
using Routine.Api.Models;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companyCollections")]
    public class CompanyCollectionsController:ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;

        public CompanyCollectionsController(IMapper mapper,ICompanyRepository companyRepository)
        {
            _mapper = mapper??throw new ArgumentNullException(nameof(mapper));
            _companyRepository = companyRepository??throw new ArgumentNullException(nameof(companyRepository));
        }
        [HttpGet("{Ids}",Name =nameof(GetCompanyCollection))]
        public async Task<IActionResult> GetCompanyCollection([FromRoute][ModelBinder(BinderType =typeof(ArrayModelBinder))]IEnumerable<Guid> Ids)
        {
            if (Ids==null)
            {
                return BadRequest();
            }
            var companies = await _companyRepository.GetCompaniesAsync(Ids);
            if (Ids.Count()!=companies.Count())
            {
                return NotFound();
            }
            var returnDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return Ok(returnDto);
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> CreateCompanyCollection(IEnumerable<CompanyAddDto> companyAddDtos)
        {
            var companies = _mapper.Map<IEnumerable<Company>>(companyAddDtos);
            foreach (var item in companies)
            {
                _companyRepository.AddCompany(item);
            }
            await _companyRepository.SaveAsync();
            var returnDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            var returnIds = string.Join(",",returnDtos.Select(x=>x.Id));
            return CreatedAtRoute(nameof(GetCompanyCollection),new { Ids=returnIds},returnDtos);
        }
    }
}
