using MicroondasDigital.Aplicacao.DTO;
using MicroondasDigital.Aplicacao.Interfaces;
using MicroondasDigital.Dominio.Entidades;
using System;
using System.Linq;
using System.Text;

namespace MicroondasDigital.Aplicacao.Services
{
    public class AquecimentoService : IAquecimentoService
    {
        private int _tempoRestante;
        private int _potencia;
        private bool _estaAquecendo;
        private bool _estaPausado;
        private bool _programaPreDefinido = false;
        private string _caractere = ".";

        private readonly StringBuilder _status = new StringBuilder();
        private readonly IProgramaAquecimentoService _programaAquecimentoService;
        private readonly IProgramaCustomizadoService _programaCustomizadoService;

        public AquecimentoService(IProgramaAquecimentoService programaAquecimentoService, IProgramaCustomizadoService programaCustomizadoService)
        {
            _programaAquecimentoService = programaAquecimentoService;
            _programaCustomizadoService = programaCustomizadoService;
        }

        public AquecimentoService()
        {
        }

        #region Métodos Públicos
        public void Iniciar(int? tempoSegundos, int? potencia)
        {
            //Bloquear incremento de tempo quando é programa pré-definido (e está aquecendo)
            if (_programaPreDefinido && _estaAquecendo)
                return;

            if (!_estaAquecendo && !_estaPausado)
            {
                var aquecimento = new Aquecimento(tempoSegundos ?? 30, potencia ?? 10);
                
                _tempoRestante = aquecimento.Tempo;
                _potencia = aquecimento.Potencia;
                _estaAquecendo = true;
                _estaPausado = false;
                _status.Clear();

                if (!_programaPreDefinido)
                    _caractere = ".";
            }
            else
            {
                if (!_estaPausado)
                    _tempoRestante += 30;
            }
        }
        
        public void Continuar()
        {
            if (_estaPausado)
                _estaPausado = false;
        }

        public string ObterTempoFormatado()
        {
            int minutos = _tempoRestante / 60;
            int segundos = _tempoRestante % 60;

            return minutos > 0 ? $"{minutos}:{segundos:D2}" : segundos.ToString();
        }

        public string PausarOuCancelar()
        {
            if (_estaAquecendo && !_estaPausado)
            {
                _estaPausado = true;
                return "pausado";
            }
            else
            {
                ResetarParametrosAquecimento(true);
                return "cancelado";
            }
        }

        public string TimerTick()
        {
            if (!_estaAquecendo || _estaPausado)
                return _status.ToString();

            if (_tempoRestante <= 0)
            {
                _estaAquecendo = false;
                _status.Append("Aquecimento concluído");
                ResetarParametrosAquecimento(false);
                return _status.ToString();
            }

            for (int i = 0; i < _potencia; i++)
            {
                _status.Append(_caractere);
            }

            _status.Append(" ");
            _tempoRestante--;

            return _status.ToString();
        }

        public ProgramaSelecionadoDto SelecionarPrograma(string nome)
        {
            ResetarParametrosAquecimento(true);

            if (string.IsNullOrEmpty(nome))
            {
                return null;
            }

            ProgramaSelecionadoDto programaSelecionado = null;

            //Primeiro busca nos programas pré-definidos, 
            var preDefinido = _programaAquecimentoService.ObterPorNome(nome);

            if (preDefinido != null)
            {
                programaSelecionado = new ProgramaSelecionadoDto
                {
                    Nome = preDefinido.Nome,
                    Tempo = preDefinido.Tempo,
                    Potencia = preDefinido.Potencia,
                    Caractere = preDefinido.CaractereAquecimento,
                    Instrucoes = preDefinido.Instrucoes
                };
            }

            //Se não encontrou nos pré-definidos, busca nos programas customizados
            if (programaSelecionado == null)
            {
                var customizado = _programaCustomizadoService.ObterPorNome(nome);

                if (customizado != null)
                {
                    programaSelecionado = new ProgramaSelecionadoDto
                    {
                        Nome = customizado.Nome,
                        Tempo = customizado.Tempo,
                        Potencia = customizado.Potencia,
                        Caractere = customizado.Caractere,
                        Instrucoes = customizado.Instrucoes
                    };
                }
            }
 
            if (programaSelecionado != null)
            {
                _tempoRestante = programaSelecionado.Tempo;
                _potencia = programaSelecionado.Potencia;
                _programaPreDefinido = true;
                _caractere = programaSelecionado.Caractere;
                return programaSelecionado;
            }

            throw new Exception($"Programa {nome} não encontrado.");
        }


        #endregion

        #region Métodos Privados
        private void ResetarParametrosAquecimento(bool limparStatus)
        {
            _tempoRestante = 0;
            _potencia = 0;
            _estaAquecendo = false;
            _estaPausado = false;
            _caractere = ".";
            _programaPreDefinido = false;

            if (limparStatus) 
                _status.Clear();
        }
        #endregion
    }
}
