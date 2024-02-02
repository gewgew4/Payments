# Payments

## Summary
This is a PoC repo that follows a given set of specifications for a payment service and an external service.
- Xunit is used for unit testing
- Docker is mostly configured and ready to use with relevant commands and configurations (see below)
- Exceptions are handled with a middleware class
- Result pattern is used for indicating success and providing data (used also on the exception's middleware)
- Unit of work and repository patterns are used for the database

## Specifications
A payments' authorization solution is required for our clients.

The solution will contain a public API which will be consumed by our clients where we will receive an authorization request and we'll respond according to the response of a third-party payment processor (simulated).

The payment processor will be an independent service that will simply respond that a authorization is approved when the total amount is an integer value and denied when the total has decimals.

Authorizations may be for a *charge*, *return* or *reversal*.

Depending on the client, there may be different authorization models:
- First: authorization request
- Second: authorization request followed by a confirmation

For clients on the second model, an action should be allowed/performed so that a reversal authorization is generated, if it is not confirmed within the first five minutes.

All authorizations should be registered with their status and relevant information.

Additionally, we should keep another registration including only approved authorizations (necessary for feeding internal reporting). This registry may only be a simple table containing dates, totals and clients. This should be asynchronous so that the authorization process is not interfered with.

Since this is a PoC, we can assume all clients are already registered and authentication is already managed.

> Bear in mind that the external payments processor is not the aim of this PoC, so its architecture is quite simplified. CronJobs are used to check every 10 seconds unconfirmed authorization requests. Dapper is used at this project while EF Core is used at the Payments solution

## Install instructions for Docker

1. Go to https://www.docker.com/products/docker-desktop/ and look for *Download for Windows*
2. For installing it, simply open the downloaded *exe* and click on *Ok*
3. You'll be asked to restart your system, please do it
4. You may need to accept Docker's license
5. Leave *Use recommended settings (requires administrator password)*
6. You may click on *Continue without signing in* and *Skip* when you're asked about yourself
7. Open a CMD/PowerShell console and use command `wsl --version` and ensure you have at least WSL version 2
> Otherwise, please, see https://learn.microsoft.com/en-us/windows/wsl/install for more info and instructions

8. Run command (replacing with the correct folder)
`docker-compose -f "SomeFolders\Payments\docker-compose.yml" -f "SomeFolders\Payments\docker-compose.override.yml" up -d`
- For example: `docker-compose -f "C:\Users\SomeUser\source\repos\gewgew4\Payments\docker-compose.yml" -f "C:\Users\SomeUser\source\repos\gewgew4\Payments\docker-compose.override.yml" up -d`
> This command should build images and run containers in a way the apps can communicate properly

9. You may use Swagger or Postman to call the different endpoints
> You can import the *Payments.Presentacion.postman_collection.json* file into Postman

> Ports shouldn't but may change due to availability, you can check in *Docker for desktop* which are the ports that the containers use

### Notes
Bear in mind you should rebuild when you make changes to the application.

First, you need to know the service's name, run command:

`docker compose ps`

Which will show you a list of the services running

In order to build again, run command

`docker compose up -d --no-deps --build SomeName`

- For example

`docker compose up -d --no-deps --build payments.presentacion` for the Payments solution

`docker compose up -d --no-deps --build externalprocessor` for the simulated external processor

## Setting up the app
We are using a SQL Server database, see https://www.microsoft.com/en-us/sql-server/sql-server-downloads Express or Developer versions for more info

Remember the instance should have TCP enabled and with a listening port (default is 1433)

This can be set via the SQL Server 20XX Configuration Manager:
1. Open *SQL Server Network Configuration*
2. Double click on *TCP/IP* to open *Properties*
3. Check that *Enabled* and *Listen All* are set to *Yes*
4. And on *IP Addresses* tab, below at *IPAll* section, check that *TCP Port* is set to 1433 (default)
5. You may need to restart SQL's services (or reboot your system) for the changes to have effect
6. Depending on your configuration, you may have to check and/or allow communications on the ports used in *Windows Firewall* or whichever firewall software you may have

Remember to change the connection string (user, password) at *appsettings.json* and running EF Core command
`update-database` while on *Payments.Infrastructure* project
