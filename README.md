# LexCore Content Manager

This project implements a simple content management system with a .NET Minimal API backend, an Angular frontend, and an optional Python data normalization script.

## Project Structure

```
LexCore
│
├── backend
│   └── LexCore.Api
│       ├── Endpoints
│       ├── Models
│       ├── DTOs
│       ├── Services
│       ├── Repositories
│       ├── Validation
│       └── Program.cs
│
├── frontend
│   └── lexcore-web
│       ├── src/app/pages
│       ├── src/app/components
│       ├── src/app/services
│       ├── src/app/models
│       ├── src/app/shared
│       └── src/app/environments
│
├── python
│   └── normalizer.py
│
├── README.md
├── SOLUTION.md
└── LexCore.sln
```

## Getting Started

### Prerequisites

*   [.NET SDK 8.x](https://dotnet.microsoft.com/download/dotnet/8.0)
*   [Node.js 20 LTS](https://nodejs.org/en/download/)
*   [Angular CLI](https://angular.io/cli) (installed via `npm install -g @angular/cli`)
*   [Python 3.8+](https://www.python.org/downloads/)

### Running the Backend (LexCore.Api)

1.  Navigate to the backend directory:
    ```bash
    cd LexCore/backend/LexCore.Api
    ```
2.  Run the application:
    ```bash
    dotnet run
    ```
    The API will typically run on `http://localhost:5000` (or a similar port).

### Running the Frontend (lexcore-web)

1.  Navigate to the frontend directory:
    ```bash
    cd LexCore/frontend/lexcore-web
    ```
2.  Install dependencies:
    ```bash
    npm install
    ```
3.  Start the Angular development server:
    ```bash
    ng serve --open
    ```
    The application will open in your browser, usually at `http://localhost:4200`.

### Running the Python Normalizer

1.  Navigate to the python directory:
    ```bash
    cd LexCore/python
    ```
2.  Run the script:
    ```bash
    python3 normalizer.py
    ```
    This will process `sample_input.jsonl` and generate `normalized_output.json`.

## API Endpoints

The backend provides the following REST API endpoints for `ContentItem` management:

*   `GET /api/content`: List all content items, with optional `language` and `status` query parameters for filtering.
*   `GET /api/content/{id}`: Get a single content item by its ID.
*   `POST /api/content`: Create a new content item. Requires `CreateContentItemRequest` in the request body.
*   `PUT /api/content/{id}`: Update an existing content item. Requires `UpdateContentItemRequest` in the request body.
*   `DELETE /api/content/{id}`: Delete a content item by its ID.

## Frontend Features

The Angular SPA provides:

*   Display of content items in a table.
*   Filtering by language and status.
*   A form to create new content items.
*   Functionality to edit and delete existing content items.

## Python Normalizer

The `normalizer.py` script demonstrates how to read raw data (JSON Lines), map it to the `ContentItem` structure, and handle duplicate `externalId` values by keeping the latest entry. The output is a JSON file suitable for import via the API. (Note: API import functionality is not implemented as per requirements).
