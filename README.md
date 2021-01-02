<h1 align="center">Simple Asp .Net Core 3.1 Web API</h1>
<p align="center"><i>A collection of API For TODO APPS.</i></p>
<div align="center">
<a href="https://github.com/ismail5g/TodoApps/stargazers"><img src="https://img.shields.io/github/stars/ismail5g/TodoApps" alt="Stars Badge"/></a>
<a href="https://github.com/ismail5g/TodoApps/network/members"><img src="https://img.shields.io/github/forks/ismail5g/TodoApps" alt="Forks Badge"/></a>
<a href="https://github.com/ismail5g/TodoApps/pulls"><img src="https://img.shields.io/github/issues-pr/ismail5g/TodoApps" alt="Pull Requests Badge"/></a>
<a href="https://github.com/ismail5g/TodoApps/issues"><img src="https://img.shields.io/github/issues/ismail5g/TodoApps" alt="Issues Badge"/></a>
<a href="https://github.com/elangosundar/awesome-README-templates/graphs/contributors"><img alt="GitHub contributors" src="https://img.shields.io/github/contributors/ismail5g/TodoApps?color=2b9348"></a>
<a href="https://github.com/ismail5g/TodoApps/blob/main/LICENSE.txt"><img src="https://img.shields.io/github/license/ismail5g/TodoApps?color=2b9348" alt="License Badge"/></a>
</div>
<br>
<h4> This project was build for learning purpose on Web API, Anyone who learning WEB API can get benifited by 3n architecture funcationaly from this project.<h4>

## I'm Describe below how this API works
If you like this Repo, Please click the :star:


## Design Pattern
  - [Repository Pattern](#Repository Pattern)
  - [Unit Of Work](#Unit Of Work)

## Database
  - [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
  - [Cassandra](https://cassandra.apache.org/)
  
# How To Use


## Github README PROFILE CATEGORY

- Clone this repo or Download as Zip
- Open Project in Visual Studio Code/ Visual Studio 2019
- Change DB ConnectionString in appsettings.json
- [Make Migration And Update Database](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli)
- [Download Cassandra](https://phoenixnap.com/kb/install-cassandra-on-windows)
- Make Cassandra Database by raw Code
 ```
 CREATE KEYSPACE TodoDB
           WITH replication = {'class': 'SimpleStrategy', 'replication_factor': '1'};

 CREATE TABLE TodoDB.TodoItems (
           Id int PRIMARY KEY,
           Title text,
           DateTime DATE);
 ```
- Start Cassandra followed by above Cassandra blog

## Test with POSTMAN

![Screenshot_21](https://user-images.githubusercontent.com/29182508/103455239-af8f7b00-4d15-11eb-9407-3f7d3e056b53.png)

# List of `API` Actions

- `http://localhost:5000/api/todo/all` - To Get All Todo Items (GET)
- `http://localhost:5000/api/todo`     - To Create New Todo Item (POST)
- `http://localhost:5000/api/todo/1`   - To Get Specific Todo Item Id's Detail (GET)
- `http://localhost:5000/api/todo/1`   - To Update Specific Todo Item Id's Detail (PUT)
- `http://localhost:5000/api/todo/2`   - To Update `datetime` Of specific Todo Items (PATCH)
```
eg:    
{
    "op": "replace",
    "path": "/datetime",
    "value": "2021-01-04"
  }
```
- `http://localhost:5000/api/todo/2`   - To Delete specific Todo Item (DELETE)

# If Any Problem to RUN


[![LinkedIn][linkedin-shield]][linkedin-url]

[linkedin-shield]: [https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555]
[linkedin-url]: [https://linkedin.com/in/ismail5g]
[LinkedIn]:[https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white]

## :pencil: License

This project is licensed under [MIT](https://opensource.org/licenses/MIT) license.

## :man_astronaut: Show your support

Give a ⭐️ if this project helped you!
