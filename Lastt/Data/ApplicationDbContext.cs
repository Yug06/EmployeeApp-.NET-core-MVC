using Lastt.Models;
using Microsoft.EntityFrameworkCore;

namespace Lastt.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeRelation>()
                .HasOne<Employee>(am => am.Employee1)
                .WithMany()
                .HasForeignKey(am => am.EmpId1)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EmployeeRelation>()
                .HasOne<Employee>(am => am.Employee2)
                .WithMany()
                .HasForeignKey(am => am.EmpId2)
                .OnDelete(DeleteBehavior.NoAction);
        }
        public DbSet<Employee> tbl_emp { get; set; }
        public DbSet<EmployeeRelation> tbl_rel { get; set; } 
    }
}
