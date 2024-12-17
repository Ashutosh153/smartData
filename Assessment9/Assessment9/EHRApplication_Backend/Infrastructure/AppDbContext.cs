using Core.Interface;
using Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure    
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<UserDetails> Users { get; set; }
        public DbSet<Otp>OtpDetails { get; set; }

        public DbSet<UserType> UserTypes { get; set; }

        public DbSet<Specialisation> SpecialisationDetails { get; set; }
        public DbSet<State> StateDetails { get; set; }
        public DbSet<Cities> Cities { get; set; }
        public DbSet<Country> Country { get; set; }

        public DbSet<Appointments>  appointments { get; set; }
        public DbSet<SOAPNotesDetails> soapNotes { get; set; }
    
    }
}
