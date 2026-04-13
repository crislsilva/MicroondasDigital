using NUnit.Framework;
using Moq;
using MicroondasDigital.Aplicacao.Services;
using MicroondasDigital.Aplicacao.Interfaces;
using MicroondasDigital.Dominio.Entidades;

namespace MicroondasDigital.Testes.Services
{
    public class AquecimentoServiceTestes
    {
        private AquecimentoService _service;
        private Mock<IProgramaAquecimentoService> _mockProgramaService;
        private Mock<IProgramaCustomizadoService> _mockProgramaCustomizadoService;

        [SetUp]
        public void Setup()
        {
            _mockProgramaService = new Mock<IProgramaAquecimentoService>();
            _mockProgramaCustomizadoService = new Mock<IProgramaCustomizadoService>();
            _service = new AquecimentoService(_mockProgramaService.Object, _mockProgramaCustomizadoService.Object);
        }

        #region Testes - Iniciar Aquecimento
        [Test]
        public void Iniciar_ComParametrosValidos_DeveConfigurarTempoEPotencia()
        {
            _service.Iniciar(60, 5);

            var tempo = _service.ObterTempoFormatado();
            var resultado = _service.TimerTick(); // Avança o tempo (1 segundo)

            // Assert
            Assert.AreEqual("1:00", tempo);
            Assert.IsTrue(resultado.Trim().Length == 5); // Potência 5, então deve exibir 5 caracteres no 1o segundo de aquecimento    
        }

        [Test]
        public void Iniciar_SemParametros_DeveUsarValoresPadrao()
        {
            _service.Iniciar(null, null);

            // Assert
            var tempo = _service.ObterTempoFormatado();
            var resultado = _service.TimerTick(); // Avança o tempo (1 segundo)

            Assert.AreEqual("30", tempo); //tempo padrão é 30 segundos
            Assert.IsTrue(resultado.Trim().Length == 10); // Potência padrão é 10, então deve exibir 10 caracteres no 1o segundo de aquecimento
        }

        [Test]
        public void Iniciar_QuandoJaEstaAquecendoENaoForProgramaPreDefinido_DeveAdicionarTrintaSegundos()
        {
            // Arrange
            _service.Iniciar(30, 5);

            // Act
            _service.Iniciar(30, 5);

            // Assert
            Assert.AreEqual("1:00", _service.ObterTempoFormatado());
        }

        [Test]
        public void Iniciar_QuandoProgramaPreDefinidoEstaAquecendo_NaoDeveAdicionarTempo()
        {
            // Arrange
            var programaMock = new ProgramaAquecimento("Congelado", "Alimento congelado", 120, 8, "#", "Descongelar alimento");

            _mockProgramaService
                .Setup(x => x.ObterPorNome("Congelado"))
                .Returns(programaMock);

            _service.SelecionarPrograma("Congelado");
            _service.Iniciar(programaMock.Tempo, programaMock.Potencia);
            var tempoAntes = _service.ObterTempoFormatado();

            // Act
            _service.Iniciar(30, 5);
            var tempoDepois = _service.ObterTempoFormatado();

            // Assert
            Assert.AreEqual(tempoAntes, tempoDepois);
        }

        [Test]
        public void Iniciar_QuandoPausado_NaoDeveAlterarTempo()
        {
            // Arrange
            _service.Iniciar(30, 5);
            _service.PausarOuCancelar(); // Pausa
            var tempoAntes = _service.ObterTempoFormatado();

            // Act
            _service.Iniciar(null, null); // Chama iniciar enquanto pausado
            var tempoDepois = _service.ObterTempoFormatado();

            // Assert
            Assert.AreEqual(tempoAntes, tempoDepois);
        }
        #endregion

        #region Testes - Pausar/Cancelar
        [Test]
        public void PausarOuCancelar_QuandoAquecendo_DevePausarERetornarPausado()
        {
            // Arrange
            _service.Iniciar(10, 5);

            // Act
            var resultado = _service.PausarOuCancelar();

            // Assert
            Assert.AreEqual("pausado", resultado);
        }

        [Test]
        public void PausarOuCancelar_QuandoAquecendo_NaoDeveAlterarTempo()
        {
            // Arrange
            _service.Iniciar(10, 5);
            var tempoAntes = _service.ObterTempoFormatado();

            // Act
            _service.PausarOuCancelar();
            var tempoDepois = _service.ObterTempoFormatado();

            // Assert
            Assert.AreEqual(tempoAntes, tempoDepois);
        }

        [Test]
        public void PausarOuCancelar_QuandoPausado_DeveCancelarERetornarCancelado()
        {
            // Arrange
            _service.Iniciar(10, 5);
            _service.PausarOuCancelar(); // Pausa

            // Act
            var resultado = _service.PausarOuCancelar(); // Cancela

            // Assert
            Assert.AreEqual("cancelado", resultado);
        }

        [Test]
        public void PausarOuCancelar_AposCancelar_TempoDeveSerzero()
        {
            // Arrange
            _service.Iniciar(10, 5);
            _service.PausarOuCancelar();

            // Act
            _service.PausarOuCancelar();

            // Assert
            Assert.AreEqual("0", _service.ObterTempoFormatado());
        }

        [Test]
        public void PausarOuCancelar_QuandoNaoEstaAquecendo_DeveCancelarERetornarCancelado()
        {
            // Act
            var resultado = _service.PausarOuCancelar();

            // Assert
            Assert.AreEqual("cancelado", resultado);
        }
        #endregion

        #region Testes - Continuar
        [Test]
        public void Continuar_QuandoPausado_DeveRetornarParaAquecimento()
        {
            // Arrange
            _service.Iniciar(10, 5);
            _service.PausarOuCancelar(); // Pausa

            // Act
            _service.Continuar();
            var statusAntes = _service.TimerTick();
            var statusDepois = _service.TimerTick();

            // Assert
            Assert.AreNotEqual(statusAntes, statusDepois);
        }

        [Test]
        public void Continuar_QuandoNaoPausado_NaoDeveFazerNada()
        {
            // Arrange
            _service.Iniciar(10, 5);
            var tempoAntes = _service.ObterTempoFormatado();

            // Act
            _service.Continuar();
            var tempoDepois = _service.ObterTempoFormatado();

            // Assert
            Assert.AreEqual(tempoAntes, tempoDepois);
        }
        #endregion

        #region Testes - ObterTempoFormatado
        [Test]
        public void ObterTempoFormatado_MaiorOuIgualA60Segundos_DeveRetornarFormatoMinutosESegundos()
        {
            // Arrange & Act
            _service.Iniciar(125, 5); // 2 minutos e 5 segundos

            // Assert
            Assert.AreEqual("2:05", _service.ObterTempoFormatado());
        }

        [Test]
        public void ObterTempoFormatado_MenorQue60Segundos_DeveRetornarApenasSegundos()
        {
            // Arrange & Act
            _service.Iniciar(45, 5);

            // Assert
            Assert.AreEqual("45", _service.ObterTempoFormatado());
        }

        [Test]
        public void ObterTempoFormatado_ZeroSegundos_DeveRetornarZero()
        {
            // Arrange & Act
            // Sem iniciar, padrão é 0

            // Assert
            Assert.AreEqual("0", _service.ObterTempoFormatado());
        }
        #endregion

        #region Testes - TimerTick
        [Test]
        public void TimerTick_QuandoNaoEstaAquecendo_DeveRetornarStatusVazio()
        {
            // Act
            var resultado = _service.TimerTick();

            // Assert
            Assert.AreEqual(string.Empty, resultado);
        }

        [Test]
        public void TimerTick_QuandoPausado_DeveRetornarStatusAnterior()
        {
            // Arrange
            _service.Iniciar(10, 2);
            var statusAntes = _service.TimerTick(); // Primeira execução

            _service.PausarOuCancelar(); // Pausa

            // Act
            var statusDepois = _service.TimerTick(); // Não deve avançar

            // Assert
            Assert.AreEqual(statusAntes, statusDepois);
        }

        [Test]
        public void TimerTick_QuandoAquecendo_DeveExibirCaracteresDePotencia()
        {
            // Arrange
            _service.Iniciar(10, 3);

            // Act
            var resultado = _service.TimerTick();

            // Assert
            Assert.IsTrue(resultado.Contains("..."));
        }

        [Test]
        public void TimerTick_DeveDecrementarTempo()
        {
            // Arrange
            _service.Iniciar(10, 2);
            var tempoAntes = _service.ObterTempoFormatado();

            // Act
            _service.TimerTick();
            var tempoDepois = _service.ObterTempoFormatado();

            // Assert
            Assert.AreNotEqual(tempoAntes, tempoDepois);
            Assert.AreEqual("9", tempoDepois);
        }

        [Test]
        public void TimerTick_AoAlcançarZero_DeveFinalizarAquecimento()
        {
            // Arrange
            _service.Iniciar(1, 2);
            _service.TimerTick(); // reduz 1 segundo (zera)

            // Act
            var resultado = _service.TimerTick();

            // Assert
            Assert.IsTrue(resultado.Contains("Aquecimento concluído"));
        }

        [Test]
        public void TimerTick_AoAlcançarZero_DeveRetornarTempoZero()
        {
            // Arrange
            _service.Iniciar(1, 2);
            _service.TimerTick(); // reduz 1 segundo (zera)
            _service.TimerTick();

            // Act
            var tempo = _service.ObterTempoFormatado();

            // Assert
            Assert.AreEqual("0", tempo);
        }

        [Test]
        public void TimerTick_ComCaracterePersonalizadoDeProgramaPreDefinido_DeveExibirCaractereCorreto()
        {
            // Arrange
            var programaMock = new ProgramaAquecimento("Teste", "Alimento Teste", 60, 2, "#", "Descongelar alimento");

            _mockProgramaService
                .Setup(x => x.ObterPorNome("Teste"))
                .Returns(programaMock);
            
            // Act
            _service.SelecionarPrograma("Teste");

            // Iniciar o programa para configurar o caractere
            _service.Iniciar(programaMock.Tempo, programaMock.Potencia);
            
            var resultado = _service.TimerTick();

            // Assert
            Assert.IsTrue(resultado.Contains("##"));
        }
        #endregion

        #region Testes - SelecionarPrograma
        [Test]
        public void SelecionarPrograma_ComNomeValido_DeveRetornarDadosPrograma()
        {
            // Arrange
            var programaMock = new ProgramaAquecimento("Alimento Teste", "Alimento Teste", 300, 10, "#", "Descongelar alimento");

            _mockProgramaService
                .Setup(x => x.ObterPorNome("Alimento Teste"))
                .Returns(programaMock);

            // Act
            var resultado = _service.SelecionarPrograma("Alimento Teste");

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual("Alimento Teste", resultado.Nome);
            Assert.AreEqual(300, resultado.Tempo);
            Assert.AreEqual(10, resultado.Potencia);
            Assert.AreEqual("#", resultado.Caractere);
            Assert.AreEqual("Descongelar alimento", resultado.Instrucoes);
        }

        [Test]
        public void SelecionarPrograma_ComNomeNulo_DeveRetornarNulo()
        {
            // Act
            var resultado = _service.SelecionarPrograma(null);

            // Assert
            Assert.IsNull(resultado);
        }

        [Test]
        public void SelecionarPrograma_ComNomeVazio_DeveRetornarNulo()
        {
            // Act
            var resultado = _service.SelecionarPrograma(string.Empty);

            // Assert
            Assert.IsNull(resultado);
        }

        [Test]
        public void SelecionarPrograma_DeveConfigurarCaractereDoPrograma()
        {
            // Arrange
            var programaMock = new ProgramaAquecimento("Rápido", "Alimento", 90, 5, "*", "Teste");

            _mockProgramaService
                .Setup(x => x.ObterPorNome("Rápido"))
                .Returns(programaMock);

            // Act
            var programa = _service.SelecionarPrograma("Rápido");

            // Assert
            Assert.AreEqual("*", programa.Caractere);
        }

        [Test]
        public void SelecionarPrograma_DeveReiniciarEAlterarDadosParaAquecimento()
        {
            //Configura mock
            var programaMock = new ProgramaAquecimento("Alimento 1", "Alimento 1", 60, 5, "#", "Teste 1");
            var programaMock2 = new ProgramaAquecimento("Alimento 2", "Alimento 2", 30, 2, "*", "Teste 2");
            
            _mockProgramaService
                .Setup(x => x.ObterPorNome("Alimento 1"))
                .Returns(programaMock);

            //Seleciona o 1o programa
            var programa1 = _service.SelecionarPrograma("Alimento 1");

            //Seleciona o 2o programa
            _mockProgramaService
                .Setup(x => x.ObterPorNome("Alimento 2"))
                .Returns(programaMock2);

            var programa2 = _service.SelecionarPrograma("Alimento 2");

            // Assert
            Assert.AreEqual(60, programa1.Tempo);
            Assert.AreEqual(30, programa2.Tempo);
            Assert.AreEqual(5, programa1.Potencia);
            Assert.AreEqual(2, programa2.Potencia);
            Assert.AreEqual("#", programa1.Caractere);
            Assert.AreEqual("*", programa2.Caractere);
        }

        [Test]
        public void SelecionarPrograma_DeveMarcarComoProgramaPreDefinido_ENaoAdicionarTempoAoIniciarNovamente()
        {
            var programaMock = new ProgramaAquecimento("Alimento 1", "Alimento 1", 60, 2, "#", "Teste 1");

            _mockProgramaService
                .Setup(x => x.ObterPorNome("Alimento 1"))
                .Returns(programaMock);
            
            _service.SelecionarPrograma("Alimento 1");

            //Iniciando o programa para marcar como pré-definido
            _service.Iniciar(programaMock.Tempo, programaMock.Potencia);
            var tempo1 = _service.ObterTempoFormatado();

            Assert.AreEqual("1:00", tempo1);

            //Tentar iniciar novamente (simulando o click em iniciar enquanto aquece com programa pré-definido)
            _service.Iniciar(null, null);
            
            _service.TimerTick(); //Avançar o tempo para simular o aquecimento
            var tempo2 = _service.ObterTempoFormatado(); ;

            // Assert - Não deve ter adicionado tempo, pois é programa pré-definido
            Assert.AreEqual("59", tempo2);
        }
        #endregion

        #region Testes - Fluxos Integrados
        [Test]
        public void FluxoCompleto_IniciarPausarContinuarCancelar()
        {
            _service.Iniciar(10, 2);
            Assert.AreEqual("10", _service.ObterTempoFormatado());

            var pausaResultado = _service.PausarOuCancelar();
            Assert.AreEqual("pausado", pausaResultado);

            _service.Continuar();
            _service.TimerTick();
            Assert.AreEqual("9", _service.ObterTempoFormatado());

            _service.PausarOuCancelar();    //1a vez pausa novamente
            var cancelaResultado = _service.PausarOuCancelar(); //2a vez cancela

            Assert.AreEqual("cancelado", cancelaResultado);
            Assert.AreEqual("0", _service.ObterTempoFormatado());
        }

        [Test]
        public void FluxoCompleto_AquecerAteOFim()
        {
            _service.Iniciar(2, 1);

            _service.TimerTick(); // 1 segundo restante
            Assert.AreEqual("1", _service.ObterTempoFormatado());

            _service.TimerTick(); // 0 segundos, aquecimento concluído
            var resultado = _service.TimerTick(); // Chamada após conclusão
            Assert.IsTrue(resultado.Contains("Aquecimento concluído"));
        }
        #endregion
    }
}