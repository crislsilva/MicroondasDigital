using MicroondasDigital.Aplicacao.DTO;
using MicroondasDigital.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace MicroondasDigital.Aplicacao.Interfaces
{
    public interface IProgramaAquecimentoService
    {
        List<ProgramaAquecimento> ObterTodos();
        ProgramaAquecimento ObterPorNome(string nome);
    }
}
