# Bookfinder

## Descrição
Bookfinder é uma aplicação para gerenciamento de livros e favoritos, que permite aos usuários visualizar uma lista de livros, marcar favoritos e adicionar resenhas. O backend é desenvolvido em ASP.NET Core, e o sistema integra-se com a API de livros da OpenLibrary.

## Funcionalidades
- Listagem de livros
- Visualização de detalhes de um livro
- Adição de livros aos favoritos
- Exclusão de favoritos
- Adição de resenhas aos livros favoritos

## Tecnologias Utilizadas
- **ASP.NET Core**: Framework para desenvolvimento do backend
- **Entity Framework Core**: ORM para interações com o banco de dados
- **InMemory Database**: Banco de dados em memória para testes de unidade
- **MoQ**: Biblioteca para criação de mocks em testes
- **xUnit**: Framework de testes

## Estrutura do Projeto
- **Controllers/** - Controladores da API, como o `BookController`
- **Models/** - Modelos de dados, como `Book`, `FavoriteBook` e `Review`
- **Services/** - Serviços que se comunicam com APIs externas, como `IOpenLibraryService`
- **Data/** - Configuração do contexto do banco de dados (`MyContext`)
- **Tests/** - Testes de unidade da aplicação

## Pré-requisitos
- .NET SDK 6.0 ou superior
- Editor de código, como Visual Studio Code ou Visual Studio
- (Opcional) Gerador de README para facilitar a manutenção

## Instalação e Configuração

1. Clone o repositório:
    ```bash
    git clone https://github.com/seu-usuario/bookfinder.git
    cd bookfinder
    ```

2. Instale as dependências e configure o ambiente:
    - Restaure os pacotes NuGet:
      ```bash
      dotnet restore
      ```
    - Configure a conexão com o banco de dados no `appsettings.json` ou utilize o banco de dados em memória para testes.

## Executando a Aplicação
Para iniciar a aplicação em modo de desenvolvimento, utilize o comando:
```bash
dotnet run
```
### A aplicação estará disponível em http://localhost:5000.

## Executando os Testes
### Para rodar os testes unitários, utilize:

```bash
dotnet test
```
### Os testes cobrem funcionalidades como a listagem de livros, gerenciamento de favoritos e adição de resenhas.

## Rotas Principais da API
### GET /Books - Retorna uma lista de livros
### GET /Books/Details/{id} - Retorna detalhes de um livro específico
### POST /Books/Favorite - Adiciona um livro aos favoritos
### DELETE /Books/DeleteFavorite/{id} - Remove um livro dos favoritos
### POST /Books/AddReview - Adiciona uma resenha a um livro favorito

## Contribuindo
### Contribuições são bem-vindas! Siga os passos abaixo para contribuir:

### Faça um fork do projeto.
### Crie uma nova branch para suas modificações:
```bash
git checkout -b feature/nova-funcionalidade
```
### Commit suas mudanças:
```bash
git commit -m 'Adiciona nova funcionalidade'
```
### Envie para a branch principal:
```bash
git push origin feature/nova-funcionalidade
```
### Abra um Pull Request.
