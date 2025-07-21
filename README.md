# VendorTestProject

## Vendor API

The API is designed to facilitate efficient CRUD operations on Vendors based on the specified provider.

## Vendor API
- The Wallet API is a simple REST API that allows users to retrieve and manage vendors
- The API controller relies on the IVendorLoader interface to perform the operations.
  - The API follows the Adapter Pattern where we design a common interface and contract for all the possible loaders and create one adapter for each concrete loader
  - A custom mappers are implemented to transform the loader specific data to the contract dto.
- The Api also has an exception handler middleware that handles exceptions and returns a meaningful error response.

## 🚀 Getting Started
Simply clone the project and run the application.  
Select the desired provider by changing the VendorLoader value in appsettings.  
Accepted values: "sql" or "file"

Areas of improvement:
* Add Unit Tests
* Better logging & Error handling
