using MicroondasDigital.Dominio.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace MicroondasDigital.Infra.Configurations
{
    public class ProgramaCustomizadoConfiguracao : EntityTypeConfiguration<ProgramaCustomizado>
    {
        public ProgramaCustomizadoConfiguracao()
        {
            ToTable("ProgramasCustomizados");
            
            HasKey(x => x.Id);
            
            Property(x => x.Nome)
                .IsRequired()
                .HasMaxLength(50);

            Property(x => x.Alimento)
                .IsRequired()
                .HasMaxLength(100);

            Property(x => x.Tempo)
                .IsRequired();

            Property(x => x.Potencia)
                .IsRequired();

            Property(x => x.Caractere)
                .IsRequired()
                .HasMaxLength(1);

            Property(x => x.Instrucoes)
                .IsOptional();
        }
    }
}
