# Todoist

## Technologies
- ASP.NET Core, ASP.NET MVC
- EF Core
- GIT
- OAuth2.0
- Identity for authentication
- AutoMapper

## Functionality
- Authentication (with email confirmation and OAuth2.0):
I implementated authentication using the asp.net identity library. An user will not be enter to todo list if her have not confirmed email. However, can be enter by OAuth2.0 using Google account and login without confirming email and password.
- CRUD of operaitons with the boards and tasks (the boards contains tasks):
If you have account, you can create, read, update and delete your boards where you save tasks (in the future you can invite other users to work on tasks together).
- Sorting tasks positions:
To displays tasks in the correct position you can move a task up or bottom.