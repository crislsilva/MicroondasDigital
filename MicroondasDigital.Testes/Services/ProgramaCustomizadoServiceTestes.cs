using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using MicroondasDigital.Aplicacao.Services;
using MicroondasDigital.Aplicacao.Interfaces;
using MicroondasDigital.Dominio.Entidades;
using MicroondasDigital.Dominio.Interfaces.Repositorios;

namespace MicroondasDigital.Testes.Services
{
    [TestFixture]
    public class ProgramaCustomizadoServiceTestes
    {
        private Mock<IProgramaCustomizadoRepositorio> _repositorioMock;
        private Mock<IProgramaAquecimentoService> _programaAquecimentoServiceMock;
        private ProgramaCustomizadoService _service;

        [SetUp]
        public void SetUp()
        {
            _repositorioMock = new Mock<IProgramaCustomizadoRepositorio>();
            _programaAquecimentoServiceMock = new Mock<IProgramaAquecimentoService>();
            _service = new ProgramaCustomizadoService(_repositorioMock.Object, _programaAquecimentoServiceMock.Object);
        }

        [Test]
        public void Criar_ComCaracterePonto_DeveLancarExcecao()
        {
            // Arrange
            var nome = "Teste";
            var alimento = "Alimento";
            var tempo = 60;
            var potencia = 80;
            var caractere = ".";
            var instrucoes = "Instrucoes";

            // Act & Assert
            Assert.Throws<Exception>(() => _service.Criar(nome, alimento, tempo, potencia, caractere, instrucoes),
                "Caractere '.' não é permitido");
        }

        [Test]
        public void Criar_SemInformarCamposObrigatorios_DeveLancarExcecao()
        {
            // Arrange
            var nome = "";
            var alimento = "";
            var tempo = 0;
            var potencia = 0;
            var caractere = "";

            // Act & Assert
            Assert.Throws<Exception>(() => _service.Criar(nome, alimento, tempo, potencia, caractere, ""),
                "Nome é obrigatório; Alimento é obrigatório; Tempo deve ser maior que zero; Potência deve ser maior que zero; Caractere é obrigatório");
        }

        [Test]
        public void Criar_ComCaractereJaUtilizadoEmProgramaCustomizado_DeveLancarExcecao()
        {
            // Arrange
            var nome = "Teste";
            var alimento = "Alimento";
            var tempo = 60;
            var potencia = 80;
            var caractere = "?";
            var instrucoes = "Instrucoes";

            _repositorioMock.Setup(r => r.ExisteCaractere(caractere)).Returns(true);
            _programaAquecimentoServiceMock.Setup(p => p.ObterTodos()).Returns(new List<ProgramaAquecimento>());

            // Act & Assert
            Assert.Throws<Exception>(() => _service.Criar(nome, alimento, tempo, potencia, caractere, instrucoes),
                "Caractere já utilizado em programa customizado");
        }

        [Test]
        public void Criar_ComCaractereJaUtilizadoEmProgramaPreDefinido_DeveLancarExcecao()
        {
            // Arrange
            var nome = "Teste";
            var alimento = "Alimento";
            var tempo = 60;
            var potencia = 8;
            var caractere = "#";
            var instrucoes = "Instrucoes";

            var programasPreDefinidos = new List<ProgramaAquecimento>
            {
                new ProgramaAquecimento(nome, alimento, tempo, potencia, caractere, instrucoes)
            };

            _repositorioMock.Setup(r => r.ExisteCaractere(caractere)).Returns(false);
            _programaAquecimentoServiceMock.Setup(p => p.ObterTodos()).Returns(programasPreDefinidos);

            // Act & Assert
            Assert.Throws<Exception>(() => _service.Criar(nome, alimento, tempo, potencia, caractere, instrucoes), 
                "Caractere já utilizado em programa pré-definido");
        }

        [Test]
        public void Criar_ComDadosValidos_DeveCriarProgramaCustomizado()
        {
            // Arrange
            var nome = "Teste";
            var alimento = "Alimento";
            var tempo = 180;
            var potencia = 6;
            var caractere = ">";
            var instrucoes = "Instrucoes";

            _repositorioMock.Setup(r => r.ExisteCaractere(caractere)).Returns(false);
            _programaAquecimentoServiceMock.Setup(p => p.ObterTodos()).Returns(new List<ProgramaAquecimento>());

            // Act
            _service.Criar(nome, alimento, tempo, potencia, caractere, instrucoes);

            // Assert
            _repositorioMock.Verify(r => r.Inserir(It.Is<ProgramaCustomizado>(p =>
                p.Nome == nome &&
                p.Alimento == alimento &&
                p.Tempo == tempo &&
                p.Potencia == potencia &&
                p.Caractere == caractere &&
                p.Instrucoes == instrucoes)), Times.Once);
        }

        [Test]
        public void ObterTodos_DeveRetornarListaDeProgramasCustomizados()
        {
            // Arrange
            var programas = new List<ProgramaCustomizado>
            {
                new ProgramaCustomizado { 
                    Nome = "Programa1",
                    Alimento = "Alimento1",
                    Caractere = "!",
                    Tempo = 90,
                    Potencia = 10
                },
                new ProgramaCustomizado { 
                    Nome = "Programa2", 
                    Alimento = "Alimento2",
                    Caractere = "?",
                    Tempo = 120,
                    Potencia = 5
                }
            };

            _repositorioMock.Setup(r => r.ObterTodos()).Returns(programas);

            // Act
            var resultado = _service.ObterTodos();

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count());
            _repositorioMock.Verify(r => r.ObterTodos(), Times.Once);
        }

        [Test]
        public void ObterTodos_ComRepositorioVazio_DeveRetornarListaVazia()
        {
            // Arrange
            _repositorioMock.Setup(r => r.ObterTodos()).Returns(new List<ProgramaCustomizado>());

            // Act
            var resultado = _service.ObterTodos();

            // Assert
            Assert.IsNotNull(resultado);
            Assert.IsEmpty(resultado);
        }
    }
}