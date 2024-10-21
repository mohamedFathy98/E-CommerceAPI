global using UserAddress = Domain.Entites.Address;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Persistence.Identity
{
    public class StoreIdentityContext : IdentityDbContext<User>
    {
        public StoreIdentityContext(DbContextOptions<StoreIdentityContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserAddress>().ToTable("Addresses");
        }
    }
}
