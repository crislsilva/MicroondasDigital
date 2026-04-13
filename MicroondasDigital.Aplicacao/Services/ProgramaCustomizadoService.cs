using MicroondasDigital.Aplicacao.Interfaces;
using MicroondasDigital.Dominio.Entidades;
using MicroondasDigital.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroondasDigital.Aplicacao.Services
{
    public class ProgramaCustomizadoService : IProgramaCustomizadoService
    {
        private readonly IProgramaCustomizadoRepositorio _repositorio;
        private readonly IProgramaAquecimentoService _programaPreDefinido;
        public ProgramaCustomizadoService(
            IProgramaCustomizadoRepositorio repositorio,
            IProgramaAquecimentoService programaPreDefinido)
        {
            _repositorio = repositorio;
            _programaPreDefinido = programaPreDefinido;
        }

        #region Métodos Públicos
        public void Criar(string nome, string alimento, int tempo, int potencia, string caractere, string instrucoes)
        {
            var resultado = ValidaCamposObrigatorios(nome, alimento, tempo, potencia, caractere);

            if (!string.IsNullOrEmpty(resultado))
                throw new Exception(resultado);

            if (potencia > 10)
                throw new Exception("O valor máximmo para Potência é 10");

            if (caractere == ".")
                throw new Exception("Caractere '.' não é permitido");

            if (_repositorio.ExisteCaractere(caractere))
                throw new Exception("Caractere já utilizado em programa customizado");

            var preDefinidos = _programaPreDefinido.ObterTodos();

            if (preDefinidos.Any(p => p.CaractereAquecimento == caractere))
                throw new Exception("Caractere já utilizado em programa pré-definido");

            var programa = new ProgramaCustomizado
            {
                Nome = nome,
                Alimento = alimento,
                Tempo = tempo,
                Potencia = potencia,
                Caractere = caractere,
                Instrucoes = instrucoes
            };

            _repositorio.Inserir(programa);
        }

        public IEnumerable<ProgramaCustomizado> ObterTodos()
        {
            return _repositorio.ObterTodos();
        }

        public ProgramaCustomizado ObterPorNome(string nome)
        {
            var programa = _repositorio.
                ObterTodos()
                .FirstOrDefault(p => p.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

            return programa;
        }
        #endregion

        #region Métodos Privados
        private string ValidaCamposObrigatorios(string nome, string alimento, int tempo, int potencia, string caractere)
        {
            var erros = new List<string>();

            if (string.IsNullOrEmpty(nome))
                erros.Add("Nome é obrigatório");
            if (string.IsNullOrEmpty(alimento))
                erros.Add("Alimento é obrigatório");
            if (tempo <= 0)
                erros.Add("Tempo deve ser maior que zero");
            if (potencia <= 0)
                erros.Add("Potência deve ser maior que zero");
            if (string.IsNullOrEmpty(caractere))
                erros.Add("Caractere é obrigatório");

            return erros.Count > 0 ? string.Join("; ", erros) : "";
        }
        #endregion
    }
}
