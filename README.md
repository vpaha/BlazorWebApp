# Property Damage Management web application (PropertyClaims)

## Overview

**PropertyClaims** is a **.NET 10 Blazor Web Application** designed to streamline property damage reporting, contractor selection, and claim management workflows.  
The application is containerized using Docker and deployed to **AWS ECS (Fargate)** with automated CI/CD through **GitHub Actions**.

## Architecture

**Technology Stack**

* .NET 10
* Blazor Web App
* ASP.NET Core
* Docker
* Amazon ECR
* Secrets Manager
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
