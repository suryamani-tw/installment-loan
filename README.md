# Installment Loan Project (Sample for Contract Testing)

## Overview
This project is a sample .NET-based application designed for demonstrating contract testing. It includes various components such as API, Application, Domain, Infrastructure, Persistence, and Workers, along with comprehensive test suites to showcase testing practices.

## Purpose
This project serves as a reference implementation for contract testing, allowing developers to understand and practice consumer-driven contract testing methodologies.

## Structure
- **src/InstallmentLoan.Api**: Contains the API controllers and models for handling installment loan operations.
- **src/InstallmentLoan.Application**: Core application logic for processing installment loans.
- **src/InstallmentLoan.Domain**: Domain models and business rules for installment loans.
- **src/InstallmentLoan.Infrastructure**: Infrastructure services and utilities.
- **src/InstallmentLoan.Persistence**: Data access and storage logic.
- **src/InstallmentLoan.Workers**: Background workers for processing tasks related to installment loans.
- **tests/**: Includes unit, integration, and contract tests for ensuring the reliability of the application and demonstrating testing approaches.

## Getting Started
1. **Prerequisites**: Ensure you have .NET 8.0 SDK installed on your machine.
2. **Setup**: Clone the repository and navigate to the project directory.
3. **Build**: Run `dotnet build` to build the solution.
4. **Run**: Navigate to `src/InstallmentLoan.Api` and run `dotnet run` to start the API.
5. **Test**: Run `dotnet test` in the `tests` directory to execute the test suites, with a focus on contract tests.

## Contributing
As this is a sample project for contract testing, contributions are limited to improvements in testing practices. Please fork the repository, make your changes, and submit a pull request if you have enhancements to the testing framework.

