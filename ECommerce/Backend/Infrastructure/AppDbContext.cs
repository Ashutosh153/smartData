using Core.Interface;
using Domain.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public DbSet<User> Users { get; set; }

        public DbSet<UserType> UserTypes { get; set; }

        public DbSet<Country> Country { get; set; }

        public DbSet<State> State { get; set; }
        public DbSet<Otp> otpValidation { get; set; }

        public DbSet<Product> products {  get; set; }

        public DbSet<CartMaster> cartMaster { get; set; }
        public DbSet<CartDetail> cartDetail { get; set; }

        public DbSet<SalesDetail> salesDetails { get; set; }
        public DbSet<SalesMaster> salesMasters { get; set; }

        public DbSet<Card> card { get; set; }
    }
}
