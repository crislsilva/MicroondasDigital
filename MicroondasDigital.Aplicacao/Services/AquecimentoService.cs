using System.Text;
using MicroondasDigital.Aplicacao.DTO;
using MicroondasDigital.Aplicacao.Interfaces;
using MicroondasDigital.Dominio.Entidades;

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

        public AquecimentoService(IProgramaAquecimentoService programaAquecimentoService)
        {
            _programaAquecimentoService = programaAquecimentoService;
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

            if (string.IsNullOrEmpty(nome)) {
                return null;
            }

            var programa = _programaAquecimentoService.ObterPorNome(nome);

            _tempoRestante = programa.Tempo;
            _potencia = programa.Potencia;
            _programaPreDefinido = true;
            _caractere = programa.CaractereAquecimento;

            return new ProgramaSelecionadoDto 
            {
                Nome = programa.Nome,
                Tempo = programa.Tempo,
                Potencia = programa.Potencia,                
                Caractere = programa.CaractereAquecimento,
                Instrucoes = programa.Instrucoes
            };
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
