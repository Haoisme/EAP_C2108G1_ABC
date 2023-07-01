using EAP_C2108G1_ABC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EAP_C2108G1_ABC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           
        }
        public virtual DbSet<Class> ClassSet { get; set; }  
        public virtual DbSet<Customer> CustomerSet { get; set; }   
    }
}