using MicroondasDigital.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroondasDigital.Aplicacao.Interfaces
{
    public interface IProgramaAquecimentoService
    {
        List<ProgramaAquecimento> ObterTodos();
        ProgramaAquecimento ObterPorNome(string nome);
    }
}
