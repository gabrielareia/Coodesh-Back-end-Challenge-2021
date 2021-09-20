# Coodesh-Back-end-Challenge-2021
###### <p align="right">Developed by [Gabriel Areia](https://github.com/gabrielareia)</p>
This is a *challenge by [Coodesh](https://coodesh.com)*, where I needed to create a REST API for a fictional company using .NET Core.

Here are the original challenge instructions (in Portuguese): https://lab.coodesh.com/public-challenges/back-end-challenge-2021

## The project
The challenge consisted in creating a REST API for the fictional company *Pharma Inc*, that would help the company to visualize information about their patients, in a simple and organized way. 

To simulate the users, the challenge requested the usage of the https://randomuser.me API, and everyday, in a predefined hour, my API has to update all the users in the database with new ones, a Hosted Service was needed to handle this scheduling.

Unfortunately there was a short deadline for this project, so I couldn't do everything I wanted. If I had more time I would create Unit Tests using xUnit, organize the code better and improve the performance. But I think it is already pretty good the way it is and I am pretty happy with the result. 

Feel free to explore the code, I might update something in the future just for fun.

## Technologies
- For this project I used **.NET Core**
- The API was built with **ASP.NET Core**
- I wrote the project in **C#** but knowledge in **SQL Server** was also needed to verify the queries generated by Entity.
- The SQL Server database was created using **Entity Framework Core**
- The documentation was done in **Swagger**, using **Swashbuckle.AspNetCore**
- User data was simulated using **https://randomuser.me**
- Data was serialized and deserialized with **Newtonsoft.Json**
- To organize myself and help me work quicker I used **https://trello.com**

## Instructions
Clone or download this repo, make sure all the packages or any dependencies are installed, run it.

It will open the Swagger UI, I configured the UI to appear as the main page (just "https://[domain].com", not "https://[domain].com/swagger/index.html").

There are 5 main actions for this API (and 1 extra hidden action).

| Methods | Path                | Description | Note | Status Code
| --------|-------------        | -----   | ------- | ----
| GET     | api/                |   Only returns the message _*"REST Back-end Challenge 20201209 Running"*_ | | <p align="center">```200```</p>
| GET     | api/users           |   Lists all users with status ```Published```. | Accepts pagination via query: <br>```Size=123&Page=456``` | <p align="center">```200``` ```404``` </p>
| GET     | api/users/:userId   |   Returns the specified user | It can return users with other status, like ```Trash``` or ```Draft``` | <p align="center">```200``` ```404```</p>
| PUT     | api/users/:userId   |   Updates the specified user | Requires an Api Key | <p align="center">```200``` ```404``` ```400``` ```500```</p>
| DELETE  | api/users/:userId   |   Deletes the specified user | Requires an Api Key | <p align="center">```204``` ```404```</p>
| GET (hidden)     | api/users/:userId/picture           |   Returns the user's picture | It is hidden from SwaggerUI because the challenge didn't specifically ask for that | <p align="center">```200``` ```404```</p>

To use ```PUT``` or ```DELETE``` an Api Key is needed, the Api Key for this project is in "appsettings.json" for convenience, in real life an Api Key should never be stored there.

## Conclusion

I really enjoyed working on this challenge, I hope to be able to participate in other projects like that in the future.

Good reading.
