# PhoneBook
Phone Book API project assessment for ABSA CIB

Greetings and thank you for taking the time to review my 'PhoneBook' solution  \:D/
My name is Mark van Dyk (34). Here were some of the ambiguities                  |
that I considered with and how I decided to handle them....                     / \

1. SPA framework
I decided to use MVC as the solution architecture rather than a pure .Web project using Angular.js.
While I realise that a separation of the User Interface and Back-End API projects might
be desirable, the scale of this project lead me to lean toward a 3, rather than 4 tier architecture.
Thus, the project PhoneBook.Api contains both the server and client logic, rendered in a neat
Single Page Application, /Views/Shared/_Layout.cshtml.

2. Docker
Sadly, the laptop I am working with at this moment is not capable of hardware virtualisation. I thus
could not enable Docker.

3. Documentation
There is this ReadMe, a Help & FAQ section on the website. Additionally the code is aslo commented
for ease of reading.

4. Database
When the app is run with the debugger attached, the MDF database file used is within the Phonebook.Db
project. If no debugger is attached, the MDF database file is assumed to be in the same directory as
the API. Database compatibility is set to MS SQL Server 2016.

5. Frameworks
The web user interface employs Razor pages that use jQuery and Bootstrap.
The API employs MVC as a design framework.
The Data Access Layer, or Entities project, models the database as a Entity Framework model.
All projects target .NET Core v5
