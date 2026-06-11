# WaT Assistant Backend

The backend service for the Work and Travel Assistant application. This provides a robust RESTful API for managing job offers, notes, important information, and user authentication.

## Tech Stack
* **Framework:** .NET 8 (ASP.NET Core Web API)
* **Database:** PostgreSQL
* **Infrastructure:** Docker & Docker Compose
* **Key Features:** Entity Framework Core, Custom Rate Limiting, JWT Authentication, Global Exception Handling Middleware.

## Getting Started

This project is fully dockerized. You do not need to install the .NET SDK or PostgreSQL locally on your machine.

### Prerequisites
* [Docker Desktop](https://www.docker.com/products/docker-desktop/) (or Docker Engine) installed and running.

### Running Locally

**1. Clone the repository:**
```bash
git clone https://github.com/WaT-Assistant/wat-backend.git]
```
**Note!
Always switch to your project root in your command line.


**2. Environment Variables:**
Make sure you have a `.env` file in the root directory of the project with your database credentials (e.g., `DB_USER`, `DB_PASSWORD`, `DB_NAME`).

**3. Start the containers:**
```bash
docker compose up --build
```

**4. Access the API:**
Once running, the API will be available at:
* HTTP: `http://localhost:8080`
* HTTPS: `https://localhost:8081`

### Stopping the Application
To gracefully stop the containers and free up resources, run:
```bash
docker compose down
```

## CI/CD
This repository utilizes GitHub Actions. Every push or pull request to the `main` or `dev` branches automatically triggers a workflow that builds and verifies the Docker images to ensure code integrity.