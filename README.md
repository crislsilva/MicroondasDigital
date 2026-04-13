# Micro-ondas Digital

Aplicação web que simula o funcionamento de um micro-ondas digital,
permitindo aquecimento manual e por programas pré-definidos ou
customizados. Também permite o cadastro dos programas customizados.

------------------------------------------------------------------------

## Tecnologias Utilizadas

-   C#
-   .NET Framework (Web API + ASP.NET MVC)
-   Entity Framework 6 (Code First + Migrations)
-   SQL Server
-   JavaScript (Vanilla)
-   HTML5 / CSS3
-   NUnit para testes unitários

------------------------------------------------------------------------

## Funcionalidades

-   Aquecimento manual com controle de tempo e potência\
-   Pausar, continuar e cancelar aquecimento\
-   Exibição do progresso em tempo real\
-   Programas de aquecimento pré-definidos\
-   Cadastro de programas customizados\
-   Persistência em banco de dados (SQL Server)\
-   Diferenciação visual de programas customizados\
-   Interface web com Razor (ASP.NET MVC)\

------------------------------------------------------------------------

## Como Instalar e Executar

### Pré-requisitos

-   Visual Studio 2022\
-   SQL Server\
-   .NET Framework 4.8.1
-   Observação: os projetos usam package.config para os pacotes do NuGet

------------------------------------------------------------------------

### Passo a passo

1.  Clonar o repositório

git clone https://github.com/crislsilva/MicroondasDigital.git

2.  Abrir a solução no Visual Studio

MicroondasDigital.sln

3.  Configurar a conexão com o banco no Web.config

4.  Executar as migrations:

Update-Database -ProjectName MicroondasDigital.Infra -StartupProjectName
MicroondasDigital.Api

5.  Executar a aplicação (F5)


------------------------------------------------------------------------

## Estrutura do Projeto

MicroondasDigital.sln

-   MicroondasDigital.Api
-   MicroondasDigital.Web
-   MicroondasDigital.Aplicacao
-   MicroondasDigital.Dominio
-   MicroondasDigital.Infra
-   MicroondasDigital.Testes

------------------------------------------------------------------------

## .gitignore

Este projeto utiliza um arquivo `.gitignore` para ignorar arquivos desnecessários, como:
- pasta .vs (todos os projetos)
- pasta bin (todos os projetos)
- pasta obj (todos os projetos)
- pasta packages

------------------------------------------------------------------------

## Challenge

This is a challenge by https://coodesh.com/

------------------------------------------------------------------------

## Autor

Cristiano Luiz da Silva
