# AlquilaFacil Microservices

## Description
AlquilaFacil is a microservices-based application designed to facilitate the rental process of properties. The application is built using .NET 8 and MySQL for the database. It consists of several microservices, each responsible for a specific functionality within the rental process.
The application is designed to be modular and scalable, allowing for easy addition of new features and services in the future.

## Microservices
- **AlquilaFacil.API**: The main API gateway that routes requests to the appropriate microservices.
- **AlquilaFacil.IAM**: Identity and Access Management service that handles user authentication and authorization.
- **AlquilaFacil.LocalManagement**: Service responsible for managing property listings, including adding, updating, and deleting properties.
- **AlquilaFacil.Profile**: Service that manages user profiles, including personal information and preferences.
- **AlquilaFacil.Notification**: Service that handles notifications for users, including email and SMS notifications.
- **AlquilaFacil.Subscription**: Service that manages user subscriptions, including payment processing and subscription plans.
- **AlquilaFacil.Booking**: Service that manages the booking process, including availability checks and booking confirmations.

## Getting Started
To get started with the AlquilaFacil microservices, follow these steps:
1. Clone the repository to your local machine.
```bash
git clone https://github.com/Funda-Arqui-Software-3586-Grupo-5/AlquilaFacil-Microservices.git
cd AlquilaFacil-Microservices
```

2. Up the Docker containers:
```bash
docker-compose up -d
```

3. Access the application:
    - API Gateway: `http://localhost:8000`
    - MySQL: `http://localhost:3306`
    - IAM: `http://localhost:8012`
    - Local Management: `http://localhost:8013`
    - Notification: `http://localhost:8014`
    - Profile: `http://localhost:8015`
    - Subscription: `http://localhost:8016`
    - Booking: `http://localhost:8017`

- Author:
- [Funda Arqui Software](Grupo 5 - Fundamentos de Arquitectura de Software)