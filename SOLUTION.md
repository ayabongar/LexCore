# LexCore Technical Solution

This document outlines the technical design choices and implementation details for the LexCore Content Manager application.

## Backend (LexCore.Api) - C#/.NET Minimal API

### API Shape

The backend exposes a RESTful API for managing `ContentItem` entities. The base URL for content-related operations is `/api/content`.

| Method | Path                 | Description                                                                 | Request Body (DTO)         | Response Body (DTO)          | Status Codes                               |
| :----- | :------------------- | :-------------------------------------------------------------------------- | :------------------------- | :--------------------------- | :----------------------------------------- |
| `GET`  | `/api/content`       | Retrieves a list of content items. Supports filtering by `language` and `status` query parameters. | None                       | `ContentItemResponse[]`      | `200 OK`                                   |
| `GET`  | `/api/content/{id}`  | Retrieves a single content item by its unique ID.                           | None                       | `ContentItemResponse`        | `200 OK`, `404 Not Found`                  |
| `POST` | `/api/content`       | Creates a new content item.                                                 | `CreateContentItemRequest` | `ContentItemResponse`        | `201 Created`, `400 Bad Request`           |
| `PUT`  | `/api/content/{id}`  | Updates an existing content item.                                           | `UpdateContentItemRequest` | None                         | `204 No Content`, `400 Bad Request`, `404 Not Found` |
| `DELETE`| `/api/content/{id}`  | Deletes a content item by its unique ID.                                    | None                       | None                         | `204 No Content`, `404 Not Found`          |

**Example `ContentItemResponse`:**

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "externalId": "EXT-001",
  "title": "Introduction to Legal Tech",
  "language": "en",
  "status": "published",
  "tags": [
    "legal",
    "tech"
  ],
  "publishedAt": "2026-07-15T18:00:00Z",
  "body": "Legal technology refers to the use of technology and software..."
}
```

### Key Design Choices

*   **Minimal APIs:** The backend is implemented using .NET Minimal APIs, which provide a lightweight and performant way to build HTTP APIs with minimal boilerplate. This aligns with the 
focused scope of the project.
*   **Layered Architecture:** Even within a Minimal API structure, the code is organized into distinct layers for better maintainability and testability:
    *   **Models:** Define the core domain entities.
    *   **DTOs (Data Transfer Objects):** Separate the internal domain models from the data sent over the wire.
    *   **Repositories:** Abstract the data access logic. An `InMemoryContentRepository` is used for persistence.
    *   **Services:** Handle business logic and mapping between models and DTOs.
    *   **Endpoints:** Define the API routes and handle incoming requests.
*   **In-Memory Persistence:** A `ConcurrentDictionary` is used within the `InMemoryContentRepository` to provide thread-safe, in-memory storage for content items.
*   **Data Seeding:** A `DataSeeder` class populates the repository with sample data at startup to facilitate immediate testing.
*   **Filtering:** Server-side filtering is implemented in the `GetAllAsync` method of the repository, allowing the API to return only items that match the specified `language` and `status` query parameters.

---

## Frontend (lexcore-web) - Angular SPA

### Structure and Choices

The Angular frontend is built as a standalone application, following modern Angular best practices.

*   **Standalone Components:** The application uses standalone components to reduce the need for `NgModules`, making the code more modular and easier to reason about.
*   **Service-Based Data Access:** The `ContentService` handles all HTTP communication with the backend API, providing typed methods for each operation.
*   **Reactive Filtering:** Filtering is implemented by subscribing to the `getItems` observable from the service whenever the user changes the language or status selection. This ensures that the table is always up-to-date with the latest filtered data from the server.
*   **Form Management:** A single form is used for both creating and editing content items. The component state tracks whether the form is in "create" or "edit" mode and handles the corresponding API calls.
*   **Loading and Error States:** The component includes flags to manage loading indicators and error messages, providing feedback to the user during asynchronous operations.

---

## Python Data Normalization

### Script Overview

The `normalizer.py` script provides a way to transform raw data into the `ContentItem` format used by the LexCore system.

*   **Data Mapping:** The script maps fields from the raw input (e.g., `ext_id` to `externalId`, `lang` to `language`) to the normalized structure.
*   **Duplicate Handling:** It uses a dictionary to store normalized items, keyed by `externalId`. This naturally handles duplicates by ensuring that only one entry exists for each unique `externalId`, with later entries overwriting previous ones.
*   **JSON Lines Input:** The script is designed to read from a JSON Lines file, which is a common format for large datasets.
*   **Import Strategy:** While not implemented, the normalized output JSON file can be imported into the LexCore system by iterating through the items and making a `POST /api/content` request for each item. This could be automated with a separate migration script or integrated into a backend import service.
