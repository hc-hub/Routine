using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Routine.Api.Entities;
using Routine.Api.Models;
using Routine.Api.Services;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public EmployeesController(ICompanyRepository companyRepository,IMapper mapper)
        {
            _companyRepository = companyRepository??throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper??throw new ArgumentNullException(nameof(_mapper));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesAsync(Guid companyId,[FromQuery]string genderDisplay,string q)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var employees = await _companyRepository.GetEmployeesAsync(companyId,genderDisplay,q);
            if (employees==null)
            {
                return NotFound();
            }
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(employeesDto);
        }
        [HttpGet("{employeeId}",Name =(nameof(GetEmployeeAsync)))]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeAsync(Guid companyId,Guid employeeId)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var employee = await _companyRepository.GetEmployeeAsync(companyId,employeeId);
            if (employee == null)
            {
                return NotFound();
            }
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return Ok(employeeDto);
        }
        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(Guid companyId,EmployeeAddDto employeeAddDto)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var employee = _mapper.Map<Employee>(employeeAddDto);            
            _companyRepository.AddEmployee(companyId,employee);
            await _companyRepository.SaveAsync();
            var returnDto = _mapper.Map<EmployeeDto>(employee);
            return CreatedAtRoute(nameof(GetEmployeeAsync),new { companyId, employeeId = returnDto.Id}, returnDto);

        }
    }
}