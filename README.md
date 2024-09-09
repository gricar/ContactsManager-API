# Contact Manager API

### Descrição
> É uma API Web desenvolvida em ASP.NET Core para gerenciar contatos com funcionalidades de cadastro, consulta, atualização e exclusão de contatos, utilizando boas práticas de arquitetura e testes automatizados.

<details>
  <summary><strong>Principais funcionalidades</strong></summary>
  
  - **Cadastro de contatos**: permitir o cadastro de novos contatos, incluindo nome, telefone e e-mail. Associe cada contato a um DDD correspondente à região.
  - **Consulta de contatos**: implementar uma funcionalidade para consultar e visualizar os contatos cadastrados, os quais podem ser filtrados pelo DDD da região.
  - **Atualização e exclusão**: possibilitar a atualização e exclusão de contatos previamente cadastrados.
   
</details>

<details>
  <summary><strong>Executando o projeto</strong></summary>

  - É necessário ter o `Docker` e o [`Docker Compose`](https://docs.docker.com/compose) instalado em sua máquina.

  - Clone o projeto: `git clone https://github.com/gricar/ContactManager-API.git`.

  - Entre na pasta do projeto: `cd ContactsManagement`.

  - Restaure as dependências: `dotnet restore`.
  
  - Entre na pasta do projeto: `cd Database` e execute o **script** para iniciar o Docker Compose: `docker-compose up -d --build`.
</details>
