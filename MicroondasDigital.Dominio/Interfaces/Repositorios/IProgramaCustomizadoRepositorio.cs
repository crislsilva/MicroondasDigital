using MicroondasDigital.Dominio.Entidades;
using System.Collections.Generic;

namespace MicroondasDigital.Dominio.Interfaces.Repositorios
{
    public interface IProgramaCustomizadoRepositorio
    {
        void Inserir(ProgramaCustomizado programa);
        IEnumerable<ProgramaCustomizado> ObterTodos();
        bool ExisteCaractere(string caractere);
    }
}
