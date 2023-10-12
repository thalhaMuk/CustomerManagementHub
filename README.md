# CustomerManagementHub 

## Project Overview

This project is a simple ASP.NET Core application that demonstrates user authentication and various API endpoints for managing customer data. The application utilizes JWT for endpoint protection and Entity Framework Core for ORM. It has been developed using .NET Core version 6.0.

## Login Information

### User Login:
- Username: the_user
- Password: userPassword

### Admin Login:
- Username: the_admin
- Password: adminPassword

## API Endpoints

### Edit Customer:
- Endpoint: `api/v{version:apiVersion}/customer/edit/{customerId}`
- Method: `PUT`
- Input Parameters: `name`, `email`, or `phone number`
- Output: Success or error status indicating whether customer data was updated.

### Get Distance:
- Endpoint: `api/v{version:apiVersion}/customer/getdistance/{customerId}`
- Method: `GET`
- Input Parameters: `longitude`, `latitude`
- Output: Returns distance calculated in kilometers from the given latitude and longitude.

### Search Customer:
- Endpoint: `api/v{version:apiVersion}/customer/search`
- Method: `GET`
- Input Parameters: `searchText`
- Output: Lists customer details matching the provided search text.

### Get Customer List Grouped by Zip Code:
- Endpoint: `api/v{version:apiVersion}/customer/groupedbyzipcode`
- Method: `GET`
- Input Parameters: None
- Output: Lists all customers grouped by their "zip code."

## View All

View All page is available only for admins to display the customer list from the database:
- URL: `api/v{version:apiVersion}/customer/viewall`

## Technologies Used

- ASP.NET Core
- Entity Framework Core (ORM)
- JWTBearer for Authentication
- IdentityModel.Tokens for Token Management
- .NET Core version 6.0

## Installation and Usage

1. Clone this repository.
2. Open the solution in Visual Studio or your preferred IDE.
3. Build and run the application.
4. Access the endpoints using the provided login credentials using postman or other similar application.

