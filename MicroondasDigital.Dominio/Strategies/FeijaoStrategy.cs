using MicroondasDigital.Dominio.Entidades;
using MicroondasDigital.Dominio.Interfaces.Entidades;

namespace MicroondasDigital.Dominio.Strategies
{
    public class FeijaoStrategy : IProgramaAquecimento
    {
        public ProgramaAquecimento ObterPrograma()
        {
            return new ProgramaAquecimento(
                nome: "Feijão",
                alimento: "Feijão congelado",
                tempo: 480,
                potencia: 9,
                caractere: "&",
                instrucoes: "Deixe o recipiente destampado e em casos de plástico, cuidado ao retirar o recipiente " +
                            "pois o mesmo pode perder resistência em altas temperaturas "
            );
        }
    }
}
