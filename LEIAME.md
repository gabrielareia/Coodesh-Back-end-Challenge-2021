# Coodesh-Back-end-Challenge-2021
###### <p align="right">Desenvolvido por [Gabriel Areia](https://github.com/gabrielareia)</p>
Este é um *desafio da [Coodesh](https://coodesh.com)*, onde eu precisei criar uma REST API para uma empresa fictícia usando .NET Core.

Aqui estão as instruções originais do desafio: https://lab.coodesh.com/public-challenges/back-end-challenge-2021

## O desafio
O desafio consistia em criar uma REST API para a empresa fictícia *Pharma Inc*, que ajudaria a empresa a visualizar informações sobre seus pacientes, de uma maneira simples e organizada.

Para simular os usuários, o desafio requisitava o uso da API https://randomuser.me, e todo dia, em uma hora predefinida, minha API tem que atualizar todos os usuários no banco de dados com novos usuários, um Hosted Service foi preciso para lidar com esse agendamento.

Infelizmente esse projeto tinha um prazo curto, então eu não pude fazer tudo o que eu queria. Se eu tivesse mais tempo eu criaria Unit Tests usando xUnit, organizaria melhor o código e melhoraria a performance. Mas eu acho que já está muito legal do jeito que está e eu estou muito feliz com o resultado.

Fique a vontade para explorar o código, talvez eu atualize algo no futuro só por diversão.

## Tecnologias
- Para esse projeto eu usei **.NET Core**
- A API foi criada com **ASP.NET Core**
- Eu escrevi esse projeto em **C#** mas conhecimento em **SQL Server** também foi preciso para verificar as queries geradas pelo Entity.
- O banco de dados SQL Server foi criado usando **Entity Framework Core**
- A documentação foi feita com **Swagger**, usando **Swashbuckle.AspNetCore**
- Os dados dos usuários foram simulados usando **https://randomuser.me**
- Dados foram serializados e deserializados com **Newtonsoft.Json**
- Para me organizar e me ajudar a trabalhar mais rápido eu usei **https://trello.com**


## Instruções
Clone ou baixe esse repositório, tenha certeza que todos os pacotes ou dependências estão instalados, na janela "Package Manager Console" execute o comando ```update-database``` para criar o banco de dados, e finalmente execute o programa (espere alguns segundos até que o programa termine de baixar os dados da API).

Vai abrir a UI do Swagger, eu configurei para a UI aparecer como página principal (só "https://[dominio].com", e não "https://[dominio].com/swagger/index.html").

Essa API tem 5 actions principais (e 1 action extra escondida).

| Métodos | Caminho                | Descrição | Notas | Status Code
| --------|-------------        | -----   | ------- | ---- 
| GET     | api/                |   Apenas retorna a mensagem  _*"REST Back-end Challenge 20201209 Running"*_ | | <p align="center">```200```</p>
| GET     | api/users           |   Lista todos os usuários com status ```Published```. | Aceita paginação através da query: <br>```Size=123&Page=456``` | <p align="center">```200``` ```404``` </p>
| GET     | api/users/:userId   |   Retorna o usuário especificado | Pode retornar usuários com outros status, como ```Trash``` ou ```Draft``` | <p align="center">```200``` ```404```</p>
| PUT     | api/users/:userId   |   Atualiza o usuário especificado | Precisa de uma Api Key | <p align="center">```200``` ```404``` ```400``` ```500```</p>
| DELETE  | api/users/:userId   |   Deleta o usuário especificado |  Precisa de uma Api Key | <p align="center">```204``` ```404```</p>
| GET (hidden)     | api/users/:userId/picture           |   Retorna a foto do usuário | É escondido da SwaggerUI porque o desafio não pedia especificamente por isso | <p align="center">```200``` ```404```</p>

Para usar ```PUT``` ou ```DELETE``` uma Api Key é necessária, a Api Key para esse projeto está em "appsettings.json" por conveniência, na vida real uma Api Key nunca deveria ser guardada lá.

## Conclusão
Eu gostei muito de trabalhar nesse desafio, e espero poder participar em outros projetos como esse no futuro.

Boa leitura.
