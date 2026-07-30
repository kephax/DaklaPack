# DaklaPack

## Introduction
At DaklaPack, we work with a variety of APIs, services, and processes. This short assignment will give you a taste of our development environment and give us insight into your approach to technical challenges. Please note that this should take no more than a couple of hours.
We think it is important to be independent, so we expect you to find solutions for your code related problems yourself during this assignment. Of course, if something in the assignment itself is unclear you’re free to contact us for more information.

## Prerequisites
•	A working development environment with an IDE of your choice (e.g., Visual Studio)
•	A public source control repository (e.g., GitHub, GitLab)

## Assignment
Imagine you’re working for DaklaPack and your product owner asks for a new feature. He logs the following ticket in our ticketing system:
Title
As a user I want to upload an existing file to a new API and have a mutated file returned to me.

## Description
•	API: Create a new .NET REST API with a Swagger UI-enabled endpoint.
•	Upload: Allow users to upload a text file through the Swagger UI.
•	Mutate: Add data like the current date and a random character sequence to the file's content.
o	Consider using a separate class or service for this mutation.
o	Explore CQRS patterns (e.g., MediatR) if applicable.
•	Return: Return the mutated file to the user to download.

## Requirements
•	Documentation: Briefly document your methods and classes.
•	Dependency Injection: Utilize dependency injection.
•	Coding Principles: Adhere to principles like DDD, KISS, SOLID, and Clean Code.
•	Source Control: Publish your code to a public repository.
 
## Post-Assignment Review
We'll review your code and discuss your design choices. We're more interested in your problem-solving approach and coding style than in a perfect solution.

## How to Test

### Ideal test
1. Postman
2. POST to: https://localhost:7009/api/files/mutate
3. Body: form-data
4. Key: file, Value: (choose a text file)
5. Send
6. Verify the response containt the mutated file with the current date and a random character sequence added to its content.

### Alternative test
Verify with a text file larger than 10 Mib
Verify with a non text file


