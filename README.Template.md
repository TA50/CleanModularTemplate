# CleanModularTemplate

A robust and scalable Modular Monolith template for .NET, built with Clean Architecture principles and modern development practices. This template provides a solid foundation for building complex applications by enforcing module boundaries while sharing common infrastructure.

## 🏗 Architecture

The project follows the **Modular Monolith** architecture, where the application is composed of loosely coupled modules. Each module encapsulates a specific business domain (e.g., IAM, Accounts) and is designed to be independent, with its own internal architecture (typically Clean Architecture).

### Key Architectural Concepts

*   **Modules**: Self-contained units of functionality. They communicate via public contracts or shared infrastructure (events/commands).
*   **Clean Architecture**: Within each module, concerns are separated into layers (API, Domain, Infrastructure).
*   **Shared Kernel**: A `Shared` project provides common building blocks, abstractions, and infrastructure code used across all modules to avoid duplication.
*   **Single Host**: The `CleanModularTemplate.Host` project acts as the entry point, composing all modules into a single running application.
*   **Orchestration**: Uses **.NET Aspire** for local development orchestration and cloud-native readiness.

## 📂 Project Structure

The solution is organized into the following key directories under `src/`:

*   **`IAM/`**: Identity and Access Management module. Handles user authentication, registration, and permissions (e.g., OTP verification).
*   **`Accounts/`**: (Example Module) Likely handles user profiles or account management.
*   **`Shared/`**: The shared kernel containing reusable code.
    *   `CleanModularTemplate.Shared.Api`: API utilities, FastEndpoints extensions.
    *   `CleanModularTemplate.Shared.Domain`: Base domain entities, interfaces (AggregateRoot, IDomainEvent).
    *   `CleanModularTemplate.Shared.Infrastructure`: Infrastructure implementations (Messaging, Persistence).
*   **`CleanModularTemplate.Host/`**: The main ASP.NET Core Web API project that bootstraps the application.
*   **`CleanModularTemplate.AspireHost/`**: The .NET Aspire AppHost project for orchestration.
*   **`CleanModularTemplate.ServiceDefaults/`**: Standard service configurations (OpenTelemetry, Health Checks) shared by Aspire projects.
*   **`CleanModularTemplate.DatabaseInitializer/`**: Utility for database migration and seeding.

## 🔧 Shared Module Features

The `Shared` module is the backbone of the template, providing essential services:

### Domain Layer
*   **Base Classes**: `Entity<TId>`, `AggregateRoot<TId>` for domain modeling.
*   **Domain Events**: `IDomainEvent`, `IDomainEventHandler`, and `IHasDomainEvents` to support event-driven architecture within the domain.
*   **Abstractions**: `IAuditable`, `IDeletable` for common entity behaviors.

### Infrastructure Layer
*   **Messaging**:
    *   **In-Memory Bus**: `InMemoryBus` for dispatching commands and events.
    *   **Domain Event Dispatcher**: `DefaultDomainEventDispatcher` to automatically dispatch events raised by aggregates.
*   **Persistence**:
    *   Extensions for configuring **Entity Framework Core** with **PostgreSQL (Npgsql)**.
*   **Utilities**: `SystemTimeProvider` for testable time dependence.

### API Layer
*   **FastEndpoints Integration**: Extension methods (`ResultsFastEndpointExtensions`) to seamlessly map `Ardalis.Result` objects to HTTP responses (e.g., `ToCreatedResult`, `ToOkOrNotFoundResult`).
*   **Authentication**: `ClaimsExtensions` for easy access to user claims (e.g., `TryGetUserId`).
*   **Swagger**: Configuration for grouping endpoints in documentation.

## 📦 Packages & Technologies

This template leverages a curated stack of modern .NET libraries:

### Core & API
*   **[FastEndpoints](https://fast-endpoints.com/)**: A developer-friendly alternative to Minimal APIs and MVC for building high-performance APIs.
*   **[Scalar](https://scalar.com/)**: For beautiful and interactive API documentation (Swagger UI alternative).
*   **[.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/)**: For cloud-native application development and orchestration.

### Domain & Logic
*   **[Ardalis.Result](https://github.com/ardalis/Result)**: For implementing the Result pattern to handle success and errors without exceptions.
*   **[Ardalis.Specification](https://github.com/ardalis/Specification)**: For encapsulating query logic.
*   **[Ardalis.GuardClauses](https://github.com/ardalis/GuardClauses)**: For argument validation.
*   **[Vogen](https://github.com/SteveDunn/Vogen)**: For strongly-typed Value Objects.

### Data & Persistence
*   **Entity Framework Core**: ORM for data access.
*   **Npgsql**: PostgreSQL provider for EF Core.

### Observability & Testing
*   **OpenTelemetry**: For distributed tracing, metrics, and logging.
*   **xUnit**: Testing framework.
*   **Bogus**: For generating fake data.
*   **NSubstitute**: For mocking.
*   **Shouldly**: For fluent assertions.
*   **SonarAnalyzer**: For static code analysis.

## 🚀 Getting Started

1.  Ensure you have the **.NET 10 SDK** (or compatible version) installed.
2.  Ensure **Docker** is running (required for Aspire and PostgreSQL).
3.  Run the **Aspire Host** project (`CleanModularTemplate.AspireHost`).
    *   This will spin up the database, API host, and dashboard.
4.  Access the **Aspire Dashboard** to view running services and logs.
5.  Explore the API documentation via the **Scalar** UI provided by the Host project.
