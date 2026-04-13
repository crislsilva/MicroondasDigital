using MicroondasDigital.Dominio.Entidades;
using System.Collections.Generic;

namespace MicroondasDigital.Aplicacao.Interfaces
{
    public interface IProgramaCustomizadoService
    {
        void Criar(string nome, string alimento, int tempo, int potencia, string caractere, string instrucoes);
        IEnumerable<ProgramaCustomizado> ObterTodos();
        ProgramaCustomizado ObterPorNome(string nome);
    }
}
