using Modelo.entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Persistencia.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistencia
{
    public class IdentityContext : IdentityDbContext<AppUser>
    {
        public IdentityContext()
        {
        }
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Server=localhost;Database=Viajes;User Id=sa;Password=Arkaine+41;Encrypt=False;");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            CargarDataSeguridad(modelBuilder);
        }
        private void CargarDataSeguridad(ModelBuilder modelBuilder)
        {
            var adminId = Guid.NewGuid().ToString();
            var clientId = Guid.NewGuid().ToString();

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminId,
                    Name = CustomRoles.ADMIN,
                    NormalizedName = CustomRoles.ADMIN
                }
            );

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = clientId,
                    Name = CustomRoles.CLIENT,
                    NormalizedName = CustomRoles.CLIENT
                }
            );

            modelBuilder.Entity<IdentityRoleClaim<string>>()
            .HasData(
                new IdentityRoleClaim<string>
                {
                    Id = 1,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.TRANSPORTISTA_READ,
                    RoleId = adminId
                },
                 new IdentityRoleClaim<string>
                 {
                     Id = 2,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.TRANSPORTISTA_UPDATE,
                     RoleId = adminId
                 },
                 new IdentityRoleClaim<string>
                 {
                     Id = 3,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.TRANSPORTISTA_WRITE,
                     RoleId = adminId
                 }
            );
        }
    }
}