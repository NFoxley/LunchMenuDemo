This app shows users what's for lunch!

To run:

In the backend folder in the terminal: `dotnet run`

In the frontend folder in the terminal: `npm run dev`

Running the application as a public user allows you find out the menu for the a given day on campus.
If you're running the kitchen, you have the ability to log in, add and update menu items.

The stack from frontend to backend is:

Vue

ASP.NET Core API

Entity Framework Core

SQLite

Reasoning for this stack:
Vue or React would be the top choices for frontend. My opinion is Vue is more developer friendly, and does a better job scaling down for this scale of app, however it scales up nicely as well. React tends to invite a lot of overhead that isn't helpful in this case. In addition Tailwind would be a good compliment if the needs justified it, however vanilla CSS is a better match for this sort of scope.

C# and Asp.NET go hand in hand, and Entity Framework is the complementary choice to facilitate a connection to a database of any kind.

SQLite is the right scale of database here. It minimizes technical requirements, and it's easily distributed across platforms and easily shared.

Part of the architecture choices here are to make things modular and loosely coupled. If this application did need to access a different database, or switch from Vue to React, the intent is to make that process as painless as possible.
