# Fitness Platform

## Description
A web application built with ASP.NET Core (Backend) and React (Frontend) for a fitness platform. The platform allows users to browse and purchase fitness programs, track their progress, and view instructional videos. Users can receive personalized fitness plans based on their profile, including age, weight, height, and gender.

The application includes a secure authentication system, payment integration (statistical), and detailed program tracking. The frontend communicates with the backend API to handle user data and display personalized fitness plans.

## Features
- User Registration and Login
- Browse and Purchase Fitness Programs
- Track Progress and View Fitness Program Details
- Personalized Fitness Plan Generation (based on user's data)
- Video tutorials for each program
- Secure payment system (statistical payments)
- Soft delete and data management with timestamps (created, updated, deleted)

## Technologies Used
- ASP.NET Core (Backend)
- React (Frontend)
- SQL Server (Database)
- Entity Framework Core
- RESTful API architecture

## Installation

### Backend:
1. Clone the backend repository: `git clone <backend-repo-url>`
2. Open the project in Visual Studio or your preferred IDE.
3. Configure the database connection in `appsettings.json`.
4. Run migrations: `Add-Migration InitialCreate` and `Update-Database`.
5. Run the backend server: `dotnet run`.

### Frontend:
1. Clone the frontend repository: `git clone <frontend-repo-url>`
2. Navigate to the project folder and run `npm install` to install dependencies.
3. Run the frontend server: `npm start`.

## Contributing
If you would like to contribute to this project, please fork the repository and create a pull request with your changes. Ensure your code follows the project's conventions and passes all tests.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
