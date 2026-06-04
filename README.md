# Shell Gas Station Self-Service Web System

This project is a web-based self-service platform designed for the Shell gas station network. The system aims to automate the fuel payment process, station management, and overall administration of the gas station network.

## 🌟 Overview

The platform allows gas station customers to purchase fuel independently via a client web application. Managers and administrators can oversee workflows, track statistics, and control gas station equipment through specialized dashboards.

## 🚀 Key Features

*   **For Customers:**
    *   Registration and authentication via phone number/login.
    *   Self-service selection of gas pump, fuel type, and volume.
    *   Cashless payments (including cryptocurrency wallet support).
    *   Loyalty program (bonus card integration).
*   **For Managers:**
    *   Management of a specific gas station or a group of stations.
    *   Monitoring of active orders and pump statuses.
    *   Access to reports and station statistics.
*   **For Administrators:**
    *   Full administrative control over the entire gas station network.
    *   User, manager, and role management.
    *   Management of fuel reservoirs (tanks) and pricing.
    *   In-depth statistics and system-wide analytics.

## 📁 Project Structure

*   **PetrolAPI** — The server-side application (Backend in C# / .NET). It handles business logic, authorization (JWT), and database interactions (SQLite).
*   **PetrolClient** — The client-side application (Frontend in React / TypeScript). It provides the user interface for customers, managers, and administrators.
*   **PetrolWPF / PetrolControlLibrary** — Desktop applications and libraries designed for on-site hardware and terminal management.
*   **PetrolUnitTests** — Unit tests to ensure the correctness of the business logic.

## 🛠 Technology Stack

*   **Backend:** C#, .NET 8/9, Entity Framework Core, SQLite, JWT-Authentication.
*   **Frontend:** React (Vite), TypeScript.
*   **Architecture:** REST API.

## 🔧 Installation and Setup

1.  **Backend (API):**
    Navigate to the `PetrolAPI` directory and run the application via the CLI or your preferred IDE (Visual Studio / Rider):
    ```bash
    cd PetrolAPI
    dotnet run
    ```
    The API will be accessible at the address indicated in the console (typically `https://localhost:7000` or `http://localhost:5000`). API documentation is available via Swagger/Scalar OpenAPI.

2.  **Frontend (Client):**
    Navigate to the `PetrolClient` directory, install the dependencies, and start the local development server:
    ```bash
    cd PetrolClient
    npm install
    npm run dev
    ```
    The client application will be accessible at `http://localhost:5173`.

## 🔐 Test Credentials

Upon the first database initialization, the system automatically seeds test users. You can use the following credentials to test various features of the application:

### Administrator (Full System Access)
*   **Username:** `admin`
*   **Password:** `admin` *(Note: During the very first DB initialization, the password might be `password`, but it resets to `admin` upon API restart)*

### Manager (Gas Station Management)
*   **Username:** `manager1 or 2 or 3....12`
*   **Password:** `manager`

### Client (Test Customer)
*   **Phone Number:** `+48000000003`
*   **Username:** `client`
*   **Password:** `password`
*   *(The client account is automatically linked to a 100-point bonus card and cryptocurrency wallets for testing payments).*
