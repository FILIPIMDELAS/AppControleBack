using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using System.Configuration;
using AppControle.Models.GetData;
using AppControle.Models.GetDataReform;


namespace AppControle.Models
{
    public class AcessControlContext : DbContext
    {
        public AcessControlContext(DbContextOptions<AcessControlContext> options) : base(options) => Database.EnsureCreated();

        public DbSet<User> User { get; set; } = null!;
        public DbSet<UserPermission> UserPermission { get; set; } = null!;
        public DbSet<RelPermUser> RelPermUser { get; set; } = null!;
        public DbSet<Work> Work { get; set; } = null!;
        public DbSet<RelUserWork> RelUserWork { get; set; } = null!;

        public DbSet<GetDateCont> GetDateCont { get; set; } = null!;
        public DbSet<GetDateMed> GetDateMed { get; set; } = null!;
        public DbSet<GetDateOrc> GetDateOrc { get; set; } = null!;

        public DbSet<GetDateReform> GetDateReform { get; set; } = null!;
    }
}
