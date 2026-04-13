using MicroondasDigital.Dominio.Entidades;
using MicroondasDigital.Dominio.Interfaces.Entidades;

namespace MicroondasDigital.Dominio.Strategies
{
    public class PipocaStrategy : IProgramaAquecimento
    {
        public ProgramaAquecimento ObterPrograma()
        {
            return new ProgramaAquecimento(
                nome: "Pipoca",
                alimento: "Pipoca (de micro-ondas)",
                tempo: 180,
                potencia: 7,
                caractere: "*",
                instrucoes: "Observar o barulho de estouros do milho, caso houver um intervalo de mais de " +
                            "10 segundos entre um estouro e outro interrompa o aquecimento."
            );
        }
    }
}
