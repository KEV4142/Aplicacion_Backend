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
        /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Server=localhost;Database=Viajes;User Id=sa;Password=Arkaine+41;Encrypt=False;"); */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Viajes;User Id=sa;Password=arkaine;Encrypt=False;");
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
                 },
                 new IdentityRoleClaim<string>
                 {
                    Id = 4,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.VIAJE_READ,
                    RoleId = adminId
                },
                 new IdentityRoleClaim<string>
                 {
                     Id = 5,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.VIAJE_UPDATE,
                     RoleId = adminId
                 },
                 new IdentityRoleClaim<string>
                 {
                     Id = 6,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.VIAJE_CREATE,
                     RoleId = adminId
                 },
                  new IdentityRoleClaim<string>
                 {
                    Id = 7,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.VIAJEDETALLE_READ,
                    RoleId = adminId
                },
                 new IdentityRoleClaim<string>
                 {
                     Id = 8,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.VIAJEDETALLE_UPDATE,
                     RoleId = adminId
                 },
                 new IdentityRoleClaim<string>
                 {
                     Id = 9,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.VIAJEDETALLE_CREATE,
                     RoleId = adminId
                 },
                 new IdentityRoleClaim<string>
                 {
                    Id = 10,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.SUCURSALCOLABORADOR_READ,
                    RoleId = adminId
                },
                 new IdentityRoleClaim<string>
                 {
                     Id = 11,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.SUCURSALCOLABORADOR_UPDATE,
                     RoleId = adminId
                 },
                 new IdentityRoleClaim<string>
                 {
                     Id = 12,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.SUCURSALCOLABORADOR_CREATE,
                     RoleId = adminId
                 },
                 new IdentityRoleClaim<string>
                 {
                    Id = 13,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.SUCURSAL_READ,
                    RoleId = adminId
                },
                 new IdentityRoleClaim<string>
                 {
                     Id = 14,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.SUCURSAL_UPDATE,
                     RoleId = adminId
                 },
                 new IdentityRoleClaim<string>
                 {
                     Id = 15,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.SUCURSAL_CREATE,
                     RoleId = adminId
                 },
                 new IdentityRoleClaim<string>
                 {
                    Id = 16,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.COLABORADOR_READ,
                    RoleId = adminId
                },
                 new IdentityRoleClaim<string>
                 {
                     Id = 17,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.COLABORADOR_UPDATE,
                     RoleId = adminId
                 },
                 new IdentityRoleClaim<string>
                 {
                     Id = 18,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.COLABORADOR_CREATE,
                     RoleId = adminId
                 },
                 new IdentityRoleClaim<string>
                 {
                    Id = 19,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.TRANSPORTISTA_READ,
                    RoleId = clientId
                },
                new IdentityRoleClaim<string>
                 {
                    Id = 20,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.VIAJE_READ,
                    RoleId = clientId
                },
                new IdentityRoleClaim<string>
                 {
                    Id = 21,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.VIAJEDETALLE_READ,
                    RoleId = clientId
                },
                new IdentityRoleClaim<string>
                 {
                    Id = 22,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.SUCURSALCOLABORADOR_READ,
                    RoleId = clientId
                },
                new IdentityRoleClaim<string>
                 {
                    Id = 23,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.SUCURSAL_READ,
                    RoleId = clientId
                },
                new IdentityRoleClaim<string>
                 {
                    Id = 24,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.COLABORADOR_READ,
                    RoleId = clientId
                },
                new IdentityRoleClaim<string>
                 {
                     Id = 25,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.USUARIO_CREATE,
                     RoleId = adminId
                 },
                 new IdentityRoleClaim<string>
                 {
                     Id = 26,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.USUARIO_UPDATE,
                     RoleId = adminId
                 }
            );
        }
    }
}