RaceDay – Event Management Platform

1. Project Overview

RaceDay is a web-based event management platform developed using ASP.NET Core. The system allows Organisers to create and manage sporting events, manage Participant enrolments, and publish race results. Participants can browse upcoming events, enrol in events, manage their profiles, and view their personal race history.

This project represents Part 3 of the RaceDay Proof of Evidence (POE), where an ASP.NET Core MVC front end is integrated with the RESTful API developed in Part 2.

The application uses a role-based interface to provide different experiences for Organisers and Participants. Azure Blob Storage is integrated through the API to support cloud-based image uploads, while Docker is used to containerise the application.

2. Learning Outcomes

The project demonstrates the following learning outcomes:

* Developing an ASP.NET Core MVC application that consumes a RESTful API.
* Implementing session-based authentication and role-based navigation.
* Creating separate interfaces for Organisers and Participants.
* Integrating Azure Blob Storage for cloud-based image storage through the API.
* Containerising an ASP.NET Core application using Docker.
* Implementing a responsive and user-friendly interface.
* Using GitHub for source control and version management.
* Updating GitHub Actions to build and validate the MVC application.

3. Technologies and Tools Used

Technology	Purpose
C#	Main programming language
ASP.NET Core MVC	Web application development
ASP.NET Core Web API	Backend services and data operations
Entity Framework Core	Database operations through the API
SQL Server	Database management
Azure Blob Storage	Cloud-based image storage
Docker	Application containerisation
Bootstrap	Responsive user interface
HTML and CSS	Front-end development
JavaScript	Client-side functionality
Git and GitHub	Version control and collaboration
GitHub Actions	Continuous integration
Visual Studio 2022	Development environment

4. System Architecture

RaceDay follows a client-server architecture.

The MVC application communicates with the RESTful API using HTTP requests. The API handles business logic, database operations and communication with Azure Blob Storage.

The MVC application does not access the database directly and does not communicate with Azure Blob Storage directly.

Application Communication Flow

1. The Participant or Organiser interacts with the MVC application.
2. The MVC application sends HTTP requests to the RESTful API.
3. The API processes requests and performs the required operations.
4. The API communicates with SQL Server or Azure Blob Storage when necessary.
5. The API returns responses to the MVC application.
6. The MVC application displays the information to the user.

5. Project Structure

The solution is organised into three main projects:

PROG6212_FINAL_POE_GROUP2/
│
├── .github/
│   └── workflows/
│
├── RaceDayAPI/
│   │
│   ├── RaceDayAPI/
│   │   ├── Controllers/
│   │   ├── Data/
│   │   ├── Dtos/
│   │   ├── Models/
│   │   ├── Services/
│   │   ├── wwwroot/
│   │   ├── appsettings.json
│   │   ├── Dockerfile
│   │   └── Program.cs
│   │
│   ├── RaceDayAPI.Tests/
│   │
│   ├── RaceDayMVC/
│   │   ├── Controllers/
│   │   ├── Helpers/
│   │   ├── Models/
│   │   ├── Services/
│   │   ├── Views/
│   │   ├── wwwroot/
│   │   ├── appsettings.json
│   │   ├── Dockerfile
│   │   └── Program.cs
│   │
│   └── RaceDayAPI.sln
│
└── README.md

Project Descriptions

* RaceDayAPI: Handles backend operations, database communication, authentication and file storage.
* RaceDayAPI.Tests: Contains automated tests for the API.
* RaceDayMVC: Provides the web interface and communicates with the API for application operations.

6. Application Features

6.1 Authentication and Role-Based Access

The application supports two user roles: Organiser and Participant.

* User registration and login.
* Session-based authentication.
* Role-specific navigation and interfaces.
* Restricted access to protected pages.
* Logout functionality to end authenticated sessions.

6.2 Organiser Features

Organisers have access to event management functionality, including:

* Dashboard: View event summaries, enrolment totals and upcoming events.
* Event Management: Create, edit and delete sporting events.
* Event Banners: Upload event banner images.
* Category Management: Create age or distance categories for events.
* Enrolment Management: View Participants, their selected categories and enrolment statuses.
* Results Management: Capture and publish finishing times and positions.

6.3 Participant Features

Participants can access the following functionality:

* Home Page: Browse upcoming sporting events.
* Event Details: View event descriptions, dates, locations and available categories.
* Event Enrolment: Enrol in events and select a category.
* My Enrolments: View enrolled events and their current statuses.
* My Results: View personal race history, finishing times and positions.
* Profile Management: View and update personal information and upload a profile picture.

6.4 Azure Blob Storage

Azure Blob Storage is used for cloud-based image storage.

The integration is handled through the RaceDay API. The MVC application sends image upload requests to the API, which processes the files and stores them in Azure Blob Storage.

Images supported by the application include:

* Event banner images.
* Participant profile pictures.

The MVC application does not connect directly to Azure Blob Storage.

6.5 User Interface

The application follows the RaceDay sporting theme, with an emphasis on:

* Consistent colours and typography.
* Responsive layouts using Bootstrap.
* Role-specific navigation menus.
* Clear event information and management pages.
* Visually distinct enrolment statuses.
* User-friendly validation and error handling.

7. Requirements and Configuration

The following software and tools are required to run the project:

* Visual Studio 2022.
* .NET SDK compatible with the project.
* SQL Server.
* Docker Desktop.
* An Azure Storage account for Blob Storage functionality.
* Git.

Database Configuration

Configure the SQL Server connection string in the API configuration file:

RaceDayAPI/appsettings.json

Use your own database connection details.

API Configuration

The MVC application must be configured with the correct API base address.

Configure the API URL in:

RaceDayMVC/appsettings.json

For production or Docker deployment, use the appropriate production configuration.

Azure Blob Storage Configuration

Configure the Azure Storage connection details and required container settings in the API.

Sensitive information, including database passwords, Azure connection strings and access keys, must not be committed to GitHub.

Use environment variables or secure configuration settings for production deployments.

8. Running the Application Locally

Step 1: Clone the Repository

git clone <YOUR_GITHUB_REPOSITORY_URL>

Step 2: Open the Solution

Navigate to the project directory and open:

RaceDayAPI/RaceDayAPI.sln

Open the solution in Visual Studio 2022.

Step 3: Configure the API

Ensure that:

* SQL Server is running.
* The API database connection string is configured.
* Azure Blob Storage settings are configured.
* The API is using the correct HTTP port.

Step 4: Run the API

Start the RaceDayAPI project and ensure that it is running successfully.

Step 5: Configure and Run MVC

Ensure that the MVC application’s API base URL points to the running API.

Set RaceDayMVC as the startup project and run the application.

The MVC application should now communicate with the API and display the RaceDay platform.

9. Docker Containerisation

Docker is used to package the RaceDay application into containers, allowing it to run in a consistent environment.

The API and MVC application have separate Dockerfiles.

Building the API Docker Image

From the repository root, run:

docker build -t racedayapi -f RaceDayAPI/Dockerfile RaceDayAPI

Building the MVC Docker Image

docker build -t racedaymvc -f RaceDayAPI/RaceDayMVC/Dockerfile RaceDayAPI/RaceDayMVC

Running the MVC Container

The API must be running and accessible to the MVC container.

Use the following command, replacing the port placeholders with the actual configured ports and adjusting the environment variable name to match the MVC configuration:

docker run -d --name raceday-mvc -p 8081:<MVC_CONTAINER_PORT> -e ASPNETCORE_ENVIRONMENT=Production -e ApiSettings__BaseUrl=http://host.docker.internal:<API_HTTP_PORT> racedaymvc

The API base URL must be configured correctly so that the containerised MVC application can communicate with the API.

Note: The command above assumes Docker Desktop on Windows and an API running on the host machine. If both applications run in Docker containers, configure their communication using a shared Docker network instead.

10. Testing

The solution includes a dedicated RaceDayAPI.Tests project.

To execute the available automated tests, use:

dotnet test RaceDayAPI/RaceDayAPI.sln

Testing should cover the relevant API functionality, authentication, event management and other implemented operations.

The MVC application should also be tested manually to verify:

* Registration and login.
* Role-specific navigation.
* Access restrictions.
* Event creation and management.
* Participant enrolments.
* Image uploads.
* Results publishing.
* Profile updates.
* Docker functionality.

11. GitHub and Continuous Integration

GitHub is used to manage the project’s source code and track development progress.

The project is maintained in the group repository, with Part 3 development tracked through GitHub commits.

GitHub Actions is used for continuous integration to build the relevant projects, including the MVC application.

The Part 3 submission requires a minimum of 20 meaningful commits demonstrating the development process.

12. Demonstration Video

An unlisted YouTube video demonstrating the completed RaceDay application will be provided as part of the submission.

The demonstration covers:

* The running RaceDay application.
* Organiser and Participant functionality.
* The MVC project structure.
* Communication between MVC and the RESTful API.
* Azure Blob Storage integration.
* Docker containerisation and application execution.

YouTube Demonstration: [Insert unlisted YouTube video link]

13. Submission

The final submission includes:

* GitHub repository containing the source code.
* ASP.NET Core MVC application.
* RESTful API.
* Dockerfiles.
* Complete README documentation.
* Docker run instructions.
* Updated GitHub Actions workflow.
* Minimum of 20 meaningful commits.
* Unlisted YouTube demonstration video.

GitHub Repository: [Insert repository link]

14. Group Members

Student Name	Student Number
[Group Member 1]	[Student Number]
[Group Member 2]	[Student Number]
[Group Member 3]	[Student Number]
[Group Member 4]	[Student Number]

⸻

Project: RaceDay – Event Management Platform
Module: PROG6212
Part: Part 3 – MVC Application, Azure Blob Storage and Docker
Repository: PROG6212_FINAL_POE_GROUP2
