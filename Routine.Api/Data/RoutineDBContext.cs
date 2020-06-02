using Microsoft.EntityFrameworkCore;
using Routine.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Data
{
    public class RoutineDBContext : DbContext
    {
        public RoutineDBContext(DbContextOptions<RoutineDBContext> options) : base(options)
        {

        }
        public DbSet<Company> Company { get; set; }
        public DbSet<Employee> Employee { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().Property(x => x.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Company>().Property(x => x.Introduction).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<Employee>().Property(x => x.EmployeeNo).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Employee>().Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Employee>().Property(x => x.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Employee>().HasOne(x => x.Company).WithMany(x => x.Employees).HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = Guid.Parse("2c4ecd04-5bae-4e20-8125-c5a26aac5c0a"),
                    Name = "Microsoft",
                    Introduction = "Greate Company"
                },
                new Company
                {
                    Id = Guid.Parse("87a2ac4f-0ee0-46a2-ac27-f89c0bbadc64"),
                    Name = "Google",
                    Introduction = "Don't be evil"
                },
                new Company
                {
                    Id = Guid.Parse("89fc3b30-ec70-411c-a333-0259faa56cad"),
                    Name = "Alipapa",
                    Introduction = "FuBao Company"
                }
                );
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = Guid.Parse("b5187888-1e98-4ded-9a74-0282fa736d5d"),
                    CompanyId = Guid.Parse("2c4ecd04-5bae-4e20-8125-c5a26aac5c0a"),
                    FirstName = "Li",
                    LastName = "Guangzhu",
                    DateOfBirth = new DateTime(1985, 7, 14),
                    Gender = Gender.男,
                    EmployeeNo = "001"
                }
                );
        }

    }
}
