

using ConsultorioDeSeguros.Domain.Asegurados.Models;
using ConsultorioDeSeguros.Domain.Seguros.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsultorioDeSeguros.Infrastructure
{
    public class SegurosContexto : DbContext
    {
        public DbSet<SeguroModel> Seguro { get; set; }
        public DbSet<AseguradoModel> Asegurados { get; set; }
        public DbSet<SeguroAseguradoModel> SeguroAsegurado { get; set; }

        public SegurosContexto(DbContextOptions<SegurosContexto> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<SeguroModel>(Seguro =>
            {
                Seguro.ToTable("t_seguro", schema: "dbo");

                Seguro
                    .HasKey(field => field.Codigo)
                    .HasName("pk_seguro");

                Seguro
                    .Property(field => field.Codigo)
                    .HasColumnName("codigo")
                    .HasColumnType("int")
                    .IsRequired(true)
                    .UseIdentityColumn(1, 1);

                Seguro
                    .Property(field => field.Nombre)
                    .HasColumnName("nombre")
                    .HasColumnType("nvarchar(100)")
                    .IsRequired(true);

                Seguro
                    .Property(field => field.SumaAsegurada)
                    .HasColumnName("SumaAsegurada")
                    .HasColumnType("decimal(9,2)")
                    .IsRequired(true);

                Seguro
                    .Property(field => field.Prima)
                    .HasColumnName("prima")
                    .HasColumnType("decimal(8,2)")
                    .IsRequired(true);
            });

            modelBuilder.Entity<AseguradoModel>(asegurado =>
            {
                asegurado.ToTable("t_asegurado", schema: "dbo");

                asegurado
                    .HasKey(field => field.Cedula)
                    .HasName("pk_asegurado");

                asegurado
                    .Property(field => field.Cedula)
                    .HasColumnName("cedula")
                    .HasColumnType("nvarchar(10)")
                    .IsRequired(true);

                asegurado
                    .Property(field => field.Nombre)
                    .HasColumnName("nombre")
                    .HasColumnType("nvarchar(100)")
                    .IsRequired(true);

                asegurado
                    .Property(field => field.Telefono)
                    .HasColumnName("telefono")
                    .HasColumnType("nvarchar(14)")
                    .IsRequired(true);

                asegurado
                    .Property(field => field.Edad)
                    .HasColumnName("edad")
                    .HasColumnType("tinyint")
                    .IsRequired(true);
            });



            modelBuilder.Entity<SeguroAseguradoModel>(SeguroAsegurado =>
            {
                SeguroAsegurado.ToTable("t_seguro_asegurado", schema: "dbo");

                SeguroAsegurado
                    .HasKey(field => field.Id)
                    .HasName("pk_sseguro_asegurado");

                SeguroAsegurado
                    .Property(field => field.Id)
                    .HasColumnName("Id")
                    .HasColumnType("int")
                    .IsRequired(true)
                    .UseIdentityColumn(1, 1);
                SeguroAsegurado
                    .Property(field => field.SeguroCodigo)
                    .HasColumnName("codigo")
                    .HasColumnType("int")
                    .IsRequired(true);

                SeguroAsegurado
                    .Property(field => field.AseguradoCedula)
                    .HasColumnName("cedula")
                    .HasColumnType("nvarchar(10)")
                    .IsRequired(true);
            });

            /*
             * Relación de los objetos
             */

            modelBuilder.Entity<SeguroAseguradoModel>()
                    .HasOne(seguro => seguro.Seguro)
                    .WithMany(asegurados => asegurados.Asegurados)
                    .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<SeguroAseguradoModel>()
                    .HasOne(seguro => seguro.Asegurado)
                    .WithMany(asegurados => asegurados.Seguros)
                    .OnDelete(DeleteBehavior.Restrict);
            /*
             * Data
             */

            List<SeguroModel> seguros = new List<SeguroModel>() {
                new SeguroModel() { Codigo = 1, Nombre = "Vida", SumaAsegurada = 200.00M, Prima = 20.00M },
                new SeguroModel() { Codigo = 2, Nombre = "Salud", SumaAsegurada = 200.00M, Prima = 20.00M },
                new SeguroModel() { Codigo = 3, Nombre = "Vehícular", SumaAsegurada = 200.00M, Prima = 20.00M }
            };

            List<AseguradoModel> asegurados = new List<AseguradoModel>()
            {
                new AseguradoModel() { Cedula = "0925613556", Nombre = "Christian Franco", Telefono = "+593985749632", Edad = 25 },
                new AseguradoModel() { Cedula = "1756359625", Nombre = "Ed Sheeran", Telefono = "0985369514", Edad = 32}
            };

            List<SeguroAseguradoModel> segurosAsegurados = new List<SeguroAseguradoModel>()
            {
                new SeguroAseguradoModel(){ Id = 1, SeguroCodigo = 1,  AseguradoCedula = "1756359625"},
                new SeguroAseguradoModel(){ Id = 2, SeguroCodigo = 2,  AseguradoCedula = "1756359625"},
                new SeguroAseguradoModel(){ Id = 3, SeguroCodigo = 3,  AseguradoCedula = "1756359625"},

                new SeguroAseguradoModel(){Id = 4,SeguroCodigo = 1,  AseguradoCedula = "0925613556"},
            };


            modelBuilder.Entity<AseguradoModel>()
                .HasData(asegurados);

            modelBuilder.Entity<SeguroModel>()
                .HasData(seguros);

            modelBuilder.Entity<SeguroAseguradoModel>()
                .HasData(segurosAsegurados);
        }

    }
}
