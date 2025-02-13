using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modelo.entidades;
using Persistencia.Models;

namespace Persistencia;

public class BackendContext : IdentityDbContext<AppUser>
{
    public BackendContext()
    {
    }

    public BackendContext(DbContextOptions<BackendContext> options) : base(options)
    {
    }

    public virtual DbSet<Colaborador> Colaboradores { get; set; }

    public virtual DbSet<Sucursal> Sucursales { get; set; }

    public virtual DbSet<SucursalColaborador> SucursalesColaboradores { get; set; }

    public virtual DbSet<Transportista> Transportistas { get; set; }

    public virtual DbSet<Viaje> Viajes { get; set; }

    public virtual DbSet<ViajeDetalle> ViajesDetalles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Colaborador>(entity =>
        {
            entity.HasKey(e => e.ColaboradorID).HasName("pkColaboradorID");

            entity.Property(e => e.Coordenada)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("A");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Sucursal>(entity =>
        {
            entity.HasKey(e => e.SucursalID).HasName("pkSucursalID");

            entity.Property(e => e.Coordenada)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("A");
        });

        modelBuilder.Entity<SucursalColaborador>(entity =>
        {
            entity.HasKey(e => new { e.SucursalID, e.ColaboradorID }).HasName("pkSucursalColaboradorID");

            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("A");

            entity.HasOne(d => d.Colaborador).WithMany(p => p.SucursalesColaboradores)
                .HasForeignKey(d => d.ColaboradorID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkSucursalesColaboradoresColaboradorID");

            entity.HasOne(d => d.Sucursal).WithMany(p => p.SucursalesColaboradores)
                .HasForeignKey(d => d.SucursalID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkSucursalesColaboradoresSucursalID");
        });

        modelBuilder.Entity<Transportista>(entity =>
        {
            entity.HasKey(e => e.TransportistaID).HasName("pkTransportistaID");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("A");
            entity.Property(e => e.Tarifa)
                .HasDefaultValueSql("('0.00')")
                .HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<Viaje>(entity =>
        {
            entity.HasKey(e => e.ViajeID).HasName("pkViajeID");

            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("A");
            entity.Property(e => e.Fecha).HasColumnType("datetime");

            entity.HasOne(d => d.Sucursal).WithMany(p => p.Viajes)
                .HasForeignKey(d => d.SucursalID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkViajesSucursalID");

            entity.HasOne(d => d.Transportista).WithMany(p => p.Viajes)
                .HasForeignKey(d => d.TransportistaID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkViajesTransportistaID");

            entity.HasOne<AppUser>().WithMany()
                    .HasForeignKey(e => e.UsuarioID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fkViajesUsuarioID");
        });

        modelBuilder.Entity<ViajeDetalle>(entity =>
        {
            entity.HasKey(e => e.ViaDetID).HasName("pkViaDetID");

            entity.ToTable("ViajesDetalle");

            entity.Property(e => e.ViaDetID).ValueGeneratedOnAdd();


            entity.HasOne(d => d.Colaborador).WithMany(p => p.ViajesDetalles)
                .HasForeignKey(d => d.ColaboradorID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkViajesDetalleColaboradorID");

            entity.HasOne(d => d.Viaje).WithMany(p => p.ViajesDetalles)
                .HasForeignKey(d => d.ViajeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkViajesDetalleViajeID");
            
            entity.HasOne<AppUser>().WithMany()
                    .HasForeignKey(e => e.UsuarioID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fkViajesDetalleUsuarioID");
        });

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
