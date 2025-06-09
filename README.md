<h2>JorgeLanches, a web api catalog project.</h2>

JorgeLanches is a catalog project I made following a C# course to practice my web development skills. Using the endpoints I provided, users are capable of performing simple CRUD operations for Products and Categories,
as well as a few specific operations like listing products by price or name.

It is built using .NET 9, mySQL, ASP.NET Core and EF Core. Below you will find useful information about the functionalities, tools used and code design.

<h3>About the Project</h3>
JorgeLanches was developed using Domain Driven Design and Code First approach, modeling the database through EF Core Migrations.
<br>
<br>
At some point, I had to deviate from the course I was following and make my own decisions, mainly to ensure I would kept a pattern throughout the project.
The best example I can give is the removal of the Unit Of Work, as it revealed itself an obstacle to unit testing.

<h3>Authorization and Authentication using swagger interface.</h3>
This project implements Authentication and Authorization, so in order do consume Produtos and Categorias endpoints, first we need to create a new user and login.

/register endpoint is used to create a new user, and it's important to notice that a strong password is required, with special characters and numbers.

![image](https://github.com/user-attachments/assets/8fac9e64-10df-4f1a-974c-a90495dc3c5e)

Now using /login, inform the new user login and password in the request body. Providing the correct credentials, the response body will deliver the JWT token and a refresh token, as well as the expiration date time.
![image](https://github.com/user-attachments/assets/fff40489-ea7a-41b7-8da1-0a29582528a4)

For authorization, copy the token string without quotation marks, click the "Authorize" button at the top of the page and type Bearer and the token separated by a space, as shown in the gif below.
![chrome_WN6OmjM2vV](https://github.com/user-attachments/assets/c134c0aa-7911-44e8-b7fd-cec38baa7731)

From this point, we are able to consume Produtos and Categorias endpoints for as long as our token is not expired.

<h3>Code Features</h3>
<ul>
  <li>Every endpoint request produces a response containing the status code according to the operation result.</li>  
  <li>Categorias and Produtos POST requests deliver a CreatedAtRoutResult.</li>
  <li>Data Annotations are utilized to provide model attributes validation.</li>
  <li>Pagination is provided using optional query parameters and have a default value when not informed on request.</li>
  <li>ASP.NET Roles is used to manage authorizations.</li>
  <li>Global Rate Limiting using fixed window.</li>
  <li>Repository Pattern.</li>
  <li>Async Methods.</li>
  <li>DTOs with AutoMapper.</li>
  <li>Dependency Injection.</li>
  <li>Unit Tests using mock and InMemory Database.</li>
</ul>








