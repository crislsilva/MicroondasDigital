using MicroondasDigital.Dominio.Entidades;
using MicroondasDigital.Dominio.Interfaces;

namespace MicroondasDigital.Dominio.Strategies
{
    public class PipocaStrategy : IProgramaAquecimento
    {
        public ProgramaAquecimento ObterPrograma()
        {
            // Implementação do método para retornar o programa de pipoca
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
