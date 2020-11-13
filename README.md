# Welcome to RBAC-React-Redux-ASPNETCore repository !
This is a basic implementation of role-based access control which can be fully controlled from an Admin panel instead of hard coding the permissions inside of your code. 
This implementation covers the scenario of a basic microservice based system where the users can be having different roles and based on their roles, their permission will be different accross both in the Client side and in the Server side. 

## Technology used

This repository uses a number of frameworks and libraries to work:

* [ReactJS] - A JavaScript library for building user interfaces
* [ASP.NET Core API] - Build secure REST APIs on any platform with C#
* [SQL Server] - SQL Server 2019 Express is a free edition of SQL Server
* [MongoDB] - The database for modern applications
* [Redis] - The database for caching 


## Installation and Run

Install the dependencies and dev dependencies and start the server.

You can manually install the database servers and configure the connections string by yourself. 
Or you can use the below docker command to run the database automatically for you. 

To up and running the database servers

```sh
$ cd .\artifacts\docker
$ docker-compose up
``` 

To run Auth server

```sh
$ cd .\server\AuthWebApplication\AuthWebApplication\
$ dotnet restore
$ dotnet watch run
```
Verify the deployment by navigating to your server address in your preferred browser.

```sh
https://localhost:5001/
```

To run Resource server

```sh
$ cd .\server\WebApplication2\WebApplication2
$ dotnet restore
$ dotnet watch run
```
Verify the deployment by navigating to your server address in your preferred browser.

```sh
https://localhost:5003/
```

To run client

```sh
$ cd .\client
$ npm install
$ npm start
```
Verify the deployment by navigating to your server address in your preferred browser.

```sh
http://localhost:3000/
```


### How to run video

React Redux JWT Authentication using ASP.NET Core API

[![React Redux JWT Authentication using ASP.NET Core API](http://img.youtube.com/vi/ToEO8INViW8/0.jpg)](http://www.youtube.com/watch?v=ToEO8INViW8) 


How to run and debug the systems within less than 3 minutes with docker compose

[![How to run and debug the systems within less than 3 minutes](https://img.youtube.com/vi/3KcUTvjlB3g/0.jpg)](https://www.youtube.com/watch?v=3KcUTvjlB3g) 


### Todos
 - Write tests
 - Add nodejs resource server

License
----

MIT

   [node.js]: <http://nodejs.org>
   [express]: <http://expressjs.com>
   [ReactJS]: <https://reactjs.org/>
   [Gulp]: <http://gulpjs.com>
   [ASP.NET Core API]:<https://dotnet.microsoft.com/apps/aspnet/apis>
   [SQL Server]:<https://www.microsoft.com/en-us/sql-server/sql-server-downloads>
   [MongoDB]:<https://www.mongodb.com/>
   [Redis]:<https://redis.io/>
