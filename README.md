# Contacts Manager API

### Descrição

É uma API Web desenvolvida em ASP.NET Core para gerenciar contatos com funcionalidades de cadastro, consulta, atualização e exclusão de contatos, utilizando a Clean Architecture e o Domain-Driven Design (DDD) proporcionando uma estrutura modular e fácil de manter.

### Principais funcionalidades
- **Cadastro de contatos**: permitir o cadastro de novos contatos com validações de dados, tais como: nome, telefone e e-mail.
- **Consulta de contatos**: consultar e visualizar os contatos cadastrados, os quais podem ser filtrados pelo DDD da região.
- **Atualização e exclusão**: possibilitar a atualização e exclusão de contatos previamente cadastrados.
- **Validação de regras de negócio**: evitar inconsistências, como DDDs inválidos ou duplicidade de contatos.

### Tecnologias e Ferramentas Utilizadas

- **.NET 8**: Framework principal para desenvolvimento.
- **Entity Framework Core**: ORM para manipulação de dados.
- **Docker Compose**: Orquestração dos contêineres.
- **GitHub Actions**: Integração Contínua (CI) para automação de testes e build.
- **Prometheus e Grafana**: Monitoramento de desempenho e saúde da API.
- **TestContainer .NET**: Para testes de integração com um banco de dados contêinerizado.

### Estrutura do Projeto
Este projeto segue os princípios da Clean Architecture, dividindo o código em camadas bem definidas:
- **Domain**: Contém as entidades e regras de negócio principais.
- **Application**: Contém os casos de uso (Use Cases) e os serviços de aplicação.
- **Infrastructure**: Implementação de serviços externos, como persistência de dados, integração com APIs, etc.
- **API**: Contém os controladores (Controllers) e configurações da API.
