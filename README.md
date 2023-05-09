# ASP.NET VK task
## Technical task
Implement an API application on ASP.NET Core (5 or later)
### Requirments and restrictions:

***Main:***
- Response/Request in **JSON** 
- **Asynchronous** API methods
- Use **PostgreSQL** as DBMS
- **EntityFrameworkCore** as ORM
- Requirment models:
	- user(id, login, password, user_groub_id, user_state_id)
	- user_group(id, code, description), where code in [Admin, User]
	- user_state(id, code, description), where code in [Active, Blocked]
- Application must implement user **creation, selecting** (list of users as well), **deleting**. Delete opereation represents setting user`s status to 'Blocked'
- System must prohibit having more than one user with status 'Admin'
- After user creation user must have status 'Active'. User creation takes at least 5 seconds.

***Optionally:***
- Basic authentication
- user list pagination
- bunch of xUnit tests