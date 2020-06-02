using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Routine.Api.DtoParameters;
using Routine.Api.Entities;
using Routine.Api.Models;
using Routine.Api.Services;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompaniesController(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(_companyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }
        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies([FromQuery]CompanyDtoParameters parameters)
        {
            var companies = await _companyRepository.GetCompaniesAsync(parameters);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return Ok(companiesDto);
        }
        [HttpGet("{companyId}", Name = nameof(GetCompany))]
        public async Task<ActionResult<CompanyDto>> GetCompany(Guid companyId)
        {
            var company = await _companyRepository.GetCompanyAsync(companyId);
            if (company == null)
            {
                return NoContent();
            }
            return Ok(_mapper.Map<CompanyDto>(company));
        }
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> AddCompany(CompanyAddDto companyAddDto)
        {
            var company = _mapper.Map<Company>(companyAddDto);
            _companyRepository.AddCompany(company);
            bool result = await _companyRepository.SaveAsync();
            var returnDto = _mapper.Map<CompanyDto>(company);
            return CreatedAtRoute(nameof(GetCompany), new { companyId = returnDto.Id }, returnDto);

        }
        [HttpOptions]
        public ActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "Get,Post,Options,Put");
            return Ok();
        }
        [HttpPut("{companyId}")]
        public async Task<IActionResult> UpdateCompany(Guid companyId, CompanyUpdateDto companyUpdateDto)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                var companyAdd = _mapper.Map<Company>(companyUpdateDto);
                companyAdd.Id = companyId;
                _companyRepository.AddCompany(companyAdd);
                bool result = await _companyRepository.SaveAsync();
                var returnDto = _mapper.Map<CompanyDto>(companyAdd);
                return CreatedAtRoute(nameof(GetCompany), new { companyId = returnDto.Id }, returnDto);
            }
            var company = await _companyRepository.GetCompanyAsync(companyId);
            _mapper.Map(companyUpdateDto, company);
            _companyRepository.UpdateCompany(company);
            await _companyRepository.SaveAsync();
            return NoContent();
        }
        [HttpPatch("{companyId}")]
        public async Task<IActionResult> PatchCompany(Guid companyId,JsonPatchDocument<CompanyUpdateDto> patchDocument)
        {
            var companyEntity =await _companyRepository.GetCompanyAsync(companyId);
            if (companyEntity==null)
            {
                var companyUpdateDto = new CompanyUpdateDto();
                patchDocument.ApplyTo(companyUpdateDto,ModelState);
                if (!TryValidateModel(patchDocument))
                {
                    return ValidationProblem(ModelState);
                }
                companyEntity = _mapper.Map<Company>(companyUpdateDto);
                companyEntity.Id = companyId;
                _companyRepository.AddCompany(companyEntity);
                await _companyRepository.SaveAsync();
                var returnDto = _mapper.Map<CompanyDto>(companyEntity);
                return CreatedAtRoute(nameof(GetCompany),new { companyId=returnDto.Id},returnDto);
            }
            var dtoPatch = _mapper.Map<CompanyUpdateDto>(companyEntity);
            patchDocument.ApplyTo(dtoPatch,ModelState);
            if (!TryValidateModel(dtoPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(dtoPatch,companyEntity);
            _companyRepository.UpdateCompany(companyEntity);
            await _companyRepository.SaveAsync();
            return NoContent();
        }
        [HttpDelete("{companyId}")]
        public async Task<IActionResult> DeleteCompany(Guid companyId)
        {
            var company =await _companyRepository.GetCompanyAsync(companyId);
            if (company==null)
            {
                return NotFound();
            }
            //if (_companyRepository.GetEmployeesByCompanyIdAsync(companyId))
            //{
            //    _companyRepository.DeleteEmployeeByCompanyId(companyId);
            //    await _companyRepository.SaveAsync();
            //}
            _companyRepository.DeleteCompany(company);
            await _companyRepository.SaveAsync();
            return NoContent();
        }
        public override ActionResult ValidationProblem(ModelStateDictionary keyValuePairs)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}