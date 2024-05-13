# Restaurant reservation project for C#2 course
## Description
This project aims to create a simple restaurant reservation system that uses ASP.NET for users and WPF for administration.
## Data layer
- Custom Database client for basic CRUD operations.
- Built with SQLite
- Everything is made using reflection
## WPF desktop application
- Used for users that have admin access
- Allows basic work with the current database - adding, editing, and removing entries.
## ASP.NET web application
- Used for customers
- Allows overview of available tables if you are signed in
- Registered users can reserve tables and view tables that are available
- Logged-in user is stored in a singleton, so only one client can be logged in at a time. This is a very bad idea!
