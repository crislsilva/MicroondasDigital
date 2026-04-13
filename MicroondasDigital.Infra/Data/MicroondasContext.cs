using System.Data.Entity;
using MicroondasDigital.Dominio.Entidades;

namespace MicroondasDigital.Infra.Data
{
    public class MicroondasContext : DbContext
    {
        public MicroondasContext() : base("MicroondasConnection") 
        { 
        }

        public DbSet<ProgramaCustomizado> ProgramasCustomizados { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new Configurations.ProgramaCustomizadoConfiguracao());
            base.OnModelCreating(modelBuilder);
        }
    }
}
