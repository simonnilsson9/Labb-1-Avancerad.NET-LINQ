using Labb_1_Avancerad.NET.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_1_Avancerad.NET.Data
{
    internal class SDbContext : DbContext
    {
        public SDbContext() : base()
        {

        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source = BLACKBOX; Initial Catalog =Labb 1 Avancerad.NETV2; TrustServerCertificate=True; Integrated security = True");
        }
    }
}
