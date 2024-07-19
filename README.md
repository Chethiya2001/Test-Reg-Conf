# Microservices Setup with Docker Compose

This project demonstrates a microservices architecture for user registration, payment processing, and email notifications. Each service is containerized using Docker and managed with Docker Compose.

## Microservices

**Base URL USE API GATEWAY - `http://localhost:****`**

1. **Registration Service**
   - **API Endpoint:** `/api/Account/register`
   - Handles user registration.

2. **Payment Service**
   - **API Endpoints:**
     - `GET /api/payments`
     - `PUT /api/payments`
     - `POST /api/payments`
     - `DELETE /api/payments`
   - Manages payment transactions.

3. **Email Service**
   - **API Endpoint:** `POST /api/email`
   - Sends email notifications.

## Prerequisites

- Docker
- DotNet-8
## Databse Migration
- **Goto AuthApiService:**
- dotnet ef migrations add InitialMigration --startup-project . --project ../AuthManager
- dotnet ef database update --startup-project . --project ../AuthManager

4. **Run services**

- Clone GitHub repository
- Run docker desktop -> goto infra file -> open terminal then run `docker-compose up -d`
- Open Project files in the terminal (Api.Gateway/Api.Payment/Api.Email/AuthSrvice.Api -> `dotnet run`
