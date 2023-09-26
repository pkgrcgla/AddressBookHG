using AddressBookHG_EL.Entities;
using AddressBookHG_EL.IdentityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_DL.ContextInfo
{
    public class AddressbookContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AddressbookContext(DbContextOptions<AddressbookContext> opt)
          : base(opt)
        {

        }

        public virtual DbSet<City> CityTable { get; set; }
        public virtual DbSet<District> DistrictTable { get; set; }
        public virtual DbSet<Neighborhood> NeighborhoodTable { get; set; }
        public virtual DbSet<UserAddress> UserAddressTable { get; set; }
        public virtual DbSet<UserForgotPasswordTokens> UserForgotPasswordTokensTable { get; set; }
        public virtual DbSet<UserForgotPasswordsHistorical> UserForgotPasswordsHistoricalTable { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppRole>(x =>
            {
                x.ToTable("ROLES");
            });
            builder.Entity<UserForgotPasswordsHistorical>(x =>
            {
                x.ToTable("UserForgotPasswordsHistorical");
            });
            builder.Entity<UserForgotPasswordTokens>(x =>
            {
                x.ToTable("UserForgotPasswordTokens");
            });

            builder.Entity<City>()
            .HasIndex(u => u.PlateCode)
            .IsUnique(true);


            //builder.Entity<AppUser>(x =>
            //{
            //    x.ToTable("USERS");
            //});
        }
    }
}
