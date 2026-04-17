# Property Damage Management (PropertyDM)

## Overview

**PropertyDM** is a modern **.NET 8 Blazor Web Application** designed to streamline property damage reporting, contractor selection, and claim management workflows.  
The application is containerized using Docker and deployed to **AWS ECS (Fargate)** with automated CI/CD through **GitHub Actions**.

## Architecture

**Technology Stack**

* .NET 8
* Blazor Web App
* ASP.NET Core
* Docker
* AWS ECS (Fargate)
* Amazon ECR
* AWS Secrets Manager
* GitHub Actions (CI/CD)
* PostgreSQL (Amazon RDS)

### Authentication

The application supports **OpenID Connect (OIDC)** authentication and optional external identity providers.
Supported providers:
* OpenID Connect (OIDC)
* Google
* Facebook
* Twitter

### Google Maps Integration

The application integrates with **Google Maps Platform** to provide location services, mapping, and contractor discovery functionality.

Required capability:
* Map rendering and location visualization
* Geocoding and reverse geocoding
* Places and contractor search

### AI Integration
The application integrates with **OpenAI** services to support AI-driven features.

## Running Locally
### Prerequisites

* .NET 8 SDK
* Docker (optional)
* PostgreSQL (local instance or remote database)
* Visual Studio or VS Code

### Run application

```bash
dotnet restore
dotnet build
dotnet run
