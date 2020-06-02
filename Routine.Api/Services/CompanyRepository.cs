using Microsoft.EntityFrameworkCore;
using Routine.Api.Data;
using Routine.Api.DtoParameters;
using Routine.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly RoutineDBContext _routineDBContext;

        public CompanyRepository(RoutineDBContext routineDBContext)
        {
            _routineDBContext = routineDBContext ?? throw new ArgumentNullException(nameof(routineDBContext));
        }
        public void AddCompany(Company company)
        {
            if (company==null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            if (company.Id==Guid.Empty)
            {
                company.Id = Guid.NewGuid();
            }
            
            if (company.Employees!=null)
            {
                foreach (var employee in company.Employees)
                {
                    employee.Id = Guid.NewGuid();
                    employee.CompanyId = company.Id;
                }
            }
            _routineDBContext.Company.Add(company);
        }

        public void AddEmployee(Guid companyId, Employee employee)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            employee.Id = Guid.NewGuid();
            employee.CompanyId = companyId;
            _routineDBContext.Employee.Add(employee);
        }

        public async Task<bool> CompanyExistsAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            return await _routineDBContext.Company.AnyAsync(x => x.Id == companyId);
        }

        public void DeleteCompany(Company company)
        {
            if (company==null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            _routineDBContext.Company.Remove(company);
        }

        public void DeleteEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            _routineDBContext.Employee.Remove(employee);
        }

        public void DeleteEmployeeByCompanyId(Guid companyId)
        {
            var employee = _routineDBContext.Employee.Where(x=>x.CompanyId==companyId);
            foreach (var item in employee)
            {
                _routineDBContext.Employee.Remove(item);
            }            
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(CompanyDtoParameters parameters)
        {           
            var item = _routineDBContext.Company as IQueryable<Company>;
            if (!string.IsNullOrWhiteSpace(parameters.CompanyName))
            {
                item = item.Where(x=>x.Name.Contains(parameters.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                item = item.Where(x=>x.Introduction.Contains(parameters.SearchTerm)||x.Name.Contains(parameters.SearchTerm));
            }
            item=item.Skip(parameters.PageSize*(parameters.PageNumber-1)).Take(parameters.PageSize);
            return await item.ToListAsync();
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds)
        {
            if (companyIds==null)
            {
                throw new ArgumentNullException(nameof(companyIds));
            }
            return await _routineDBContext.Company.Where(x=>companyIds.Contains(x.Id)).OrderBy(x=>x.Name).ToListAsync();
        }

        public async Task<Company> GetCompanyAsync(Guid companyId)
        {
            if (companyId==Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            return await _routineDBContext.Company.FirstOrDefaultAsync((System.Linq.Expressions.Expression<Func<Company, bool>>)(x=> x.Id == companyId));
        }

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            if (employeeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }
            return await _routineDBContext.Employee.Where(x=>x.CompanyId==companyId&&x.Id==employeeId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, string genderDisplay,string q)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            var item = _routineDBContext.Employee.Where(x => x.CompanyId == companyId);
            if (!string.IsNullOrWhiteSpace(genderDisplay))
            {
                Gender gender = Enum.Parse<Gender>(genderDisplay);
                item = item.Where(x => x.Gender == gender);
                var list = item.ToList();
            }
            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                item = item.Where(x=>x.FirstName.Contains(q)||x.LastName.Contains(q)||x.EmployeeNo.Contains(q));
                var list = item.ToList();
            }
            return await item.OrderBy(x => x.EmployeeNo).ToListAsync();
        }

        public bool GetEmployeesByCompanyIdAsync(Guid companyId)
        {
           return _routineDBContext.Employee.Where(x=>x.CompanyId==companyId).Count()>0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _routineDBContext.SaveChangesAsync() >= 0; 
        }

        public void UpdateCompany(Company company)
        {
            
        }

        public void UpdateEmployee(Employee employee)
        {
            
        }
    }
}
