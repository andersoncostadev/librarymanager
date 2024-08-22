# LibraryManager.Api
---

- [*LibraryManager.Api*](#librarymanagerapi)
  - [*Guidelines*](#guidelines)
  - [*Technologies Used*](#technologies-used)
  - [*Controllers*](#controllers)
    - [BOOK](#book)
      - [- Create Book ](#--create-book)
      - [- Get Book ](#--get-book)
      - [- Get Book by ID](#--get-book-by-id)
      - [- Delete Book](#--delete_book)
      - [- Update Book](#--update-book)
    - [LOAN](#loan)
      - [- Create Loan](#--create-loan)
      - [- Get Loan](#--get-loan)
      - [- Return Loan](#--return-loan)
    - [USER](#--user)
      - [- Create User](#--create-user)
      - [- Get User](#--get-user)
      - [- Get user by ID](#--get-luser-by-id)
      - [- Delete User](#--delete-user)
      - [- Update User](#--update-user)
---

## *Guidelines*

Este documento fornece diretrizes e exemplos para a LibraryManager.Api.

API responsável por fornecer métodos para gerenciamento de empréstimos de livros e métodos para gerenciamento do sistema.

---

## Objetivo Geral dos Sistemas Operations.Crew.FreePass
O sistema tem como objetivo viabilizar a criação de empréstimos de lvros.

## *Technologies Used*
- [ASP.NET Core 8](https://github.com/aspnet/Home)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server)

---

## *Controllers*

### Book

### - Create Book

-  [ POST api/Book ]

- *Description:*  
Reponsável pela criação de um novo livro no sistema, desde a validação dos dados fornecidos até a persistência no banco de dados e o retorno das informações do livro criado.

 - *Request:*
```json
    {
      "title": "string",
      "author": "string",
      "isbn": "string",
      "yearPublication": 0,
      "status": 0
    }
```
     

### - Get Book

-  [ GET api/Book ]

-  *Description:*
Responsável por buscar e retornar todos os livros cadastrados no sistema. Ele interage com o repositório para realizar a consulta e retorna uma resposta contendo os livros, ou lança uma exceção caso haja algum problema ao tentar obter os dados.

- *Request:*
    
    -Query parameters: 

       No parameters

### - Get Book by ID

-  [ GET api/Book/{id} ]

-  *Description:*
  Responsável por buscar e retornar os detalhes de um livro específico com base no ID fornecido. Ele primeiro valida o comando, em seguida, consulta o repositório para encontrar o livro e, finalmente,
 retorna as informações do livro ou lança uma exceção se o livro não for encontrado.

- *Request:*
    
    -Query parameters: 

       "id" : "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  
### - Delete Book

-  [ DELETE api/Book/{id} ]

-  *Description:*
   Responsável por excluir um livro específico do sistema. Ele valida o comando de exclusão, busca o livro pelo ID, tenta excluí-lo e lida com possíveis erros durante o processo,
   como a inexistência do livro ou falha na exclusão. Se tudo correr bem, ele retorna uma resposta que confirma a exclusão.

   *Request:*
    
    -Query parameters: 

       "id" : "3fa85f64-5717-4562-b3fc-2c963f66afa6"

   # - Update Book

    [ PUT api/Book/{id} ]

   *Description:*
   Responsável por atualizar as informações de um livro específico no sistema. Ele valida o comando de atualização, busca o livro pelo ID, atualiza seus dados,
   persiste as alterações e retorna os novos detalhes do livro atualizado. Se algo der errado durante o processo, ele lança exceções apropriadas para lidar com esses casos.

    *Request:*
    
    -Query parameters: 

       "id" : "3fa85f64-5717-4562-b3fc-2c963f66afa6"

### Loan

### - Create Loan

-  [ POST api/Loan ]

-  *Description:*
Responsável por gerenciar a lógica de criação de um empréstimo de livro. Ele valida o comando de criação, verifica a disponibilidade do livro, cria o empréstimo, atualiza o status do livro para emprestado e retorna os detalhes do empréstimo.
 Se ocorrer algum erro durante o processo, como o livro já estar emprestado ou a falha ao salvar o empréstimo, ele lança exceções apropriadas.

- *Request:*
  
  ```json
    {
      "bookId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "loanDate": "2024-08-22T18:17:25.577Z",
      "returnDate": "2024-08-22T18:17:25.577Z"
    }
  ```

### - Get Loan

-  [ GET api/Loan ]

-  *Description:*
Responsável por buscar e retornar todos os empréstimos registrados no sistema. Ele interage com o repositório para realizar a consulta e retorna uma resposta contendo a lista de empréstimos, ou lança uma exceção caso haja algum problema ao tentar obter os dados.

- *Request:*

-Query parameters: 

       No parameters

### - Return Loan

-  [ PUT api/Loan/return/id ]
  
- *Description:*
  Gerencia o processo de devolução de um livro emprestado. Ele verifica a existência do empréstimo do livro e calcula se há atraso na devolução, atualiza o status do livro para disponível,
   e exclui o registro do empréstimo, retornando uma resposta que informa se a devolução foi realizada dentro do prazo ou com atraso. 

- *Request:*

-Query parameters: 

       "id" : "3fa85f64-5717-4562-b3fc-2c963f66afa6"

### - User

### - Create User

-  [ POST api/User ]

-  *Description:*
Responsável por gerenciar a criação de novos usuários no sistema. Ele valida o comando de criação, cria a entidade de usuário com os dados fornecidos, persiste essa entidade no banco de dados e retorna os detalhes do usuário recém-criado.
Se houver algum problema durante o processo, ele lança exceções apropriadas.

- *Request:*
  
 ```json
    {
      "name": "string",
      "email": "string"
    }
  ```

### - Get User

-  [ GET api/User ]

-  *Description:*
Responsável por buscar e retornar todos os usuários registrados no sistema. Ele interage com o repositório para realizar a consulta e retorna uma resposta contendo a lista de usuários, ou lança uma exceção caso haja algum problema ao tentar obter os dados.

- *Request:*

 -Query parameters: 

       "id" : "3fa85f64-5717-4562-b3fc-2c963f66afa6"

### - Get User by ID

-  [ GET api/User/{id} ]

-  *Description:*
Responsável por buscar e retornar os detalhes de um usuário específico com base no ID fornecido. Ele valida o comando, consulta o repositório para encontrar o usuário,e retorna as informações do usuário caso ele seja encontrado.
Se o usuário não for encontrado ou se houver um problema com a validação, ele lança exceções apropriadas.

- *Request:*
    
    -Query parameters: 

       "id" : "3fa85f64-5717-4562-b3fc-2c963f66afa6"


### - Delete User

-  [ DELETE api/User/{id} ]

-  *Description:*
Responsável por excluir um usuário específico do sistema. Ele valida o comando de exclusão, busca o usuário pelo ID, tenta excluí-lo, e retorna uma resposta indicando que a exclusão foi realizada. Se houver qualquer problema durante o processo,
como o usuário não ser encontrado ou a validação falhar, ele lança exceções apropriadas.

- *Request:*
    
    -Query parameters: 

       "id" : "3fa85f64-5717-4562-b3fc-2c963f66afa6"

### - Update User

-  [ PUT api/User/{id} ]

-  *Description:*
Responsável por atualizar as informações de um usuário específico no sistema. Ele valida o comando de atualização, busca o usuário pelo ID, atualiza os dados do usuário, persiste as alterações no banco de dados e retorna os detalhes atualizados do usuário.
Se ocorrer algum problema durante o processo, como o usuário não ser encontrado ou a atualização falhar, ele lança exceções apropriadas.

- *Request:*
    
    -Query parameters: 

       "id" : "3fa85f64-5717-4562-b3fc-2c963f66afa6"
