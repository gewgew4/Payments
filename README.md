# Payments

## Summary
This is a PoC repo that follows a given set of specifications for a payment service and an external service.

## Specifications
WIP

## Install instructions for Docker

1. Go to https://www.docker.com/products/docker-desktop/ and *Download for Windows*
2. For installing it, simply open the downloaded exe and click on *Ok*
3. You'll be asked to restart your system, please do it
4. You may need to accept Docker's license
5. Leave *Use recommended settings (requires administrator password)*
6. You may click on *Continue without signing in* and *Skip* when you're asked about you
7. Open a CMD/PowerShell console and use command `wsl --version` and ensure you have at least WSL version 2
> Otherwise, please, see https://learn.microsoft.com/en-us/windows/wsl/install for more info and instructions

8. Run command (replacing with the correct folder)
`docker-compose -f "SomeFolders\Payments\docker-compose.yml" -f "SomeFolders\Payments\docker-compose.override.yml" up -d`
- For example: `docker-compose -f "C:\Users\SomeUser\source\repos\gewgew4\Payments\docker-compose.yml" -f "C:\Users\SomeUser\source\repos\gewgew4\Payments\docker-compose.override.yml" up -d`
> This command should build images and run containers in a way the apps can communicate properly

9. You may use Swagger or Postman to call the different endpoints
> You can import the *Payments.Presentacion.postman_collection.json* file in Postman

> Ports shouldn't but may change due to availability, you can check in *Docker for desktop* which are the ports that the container uses

### Notes
Bear in mind you should rebuild when you make changes to the application
First, you need to know the service's name, run command
`docker compose ps`
Which will show you a list of the services running
In order to build again, run command
`docker compose ps`
- For example
`docker compose up -d --no-deps --build payments.presentacion`
`docker compose up -d --no-deps --build externalprocessor`

## Setting up the app
We are using a SQL Server database, see https://www.microsoft.com/en-us/sql-server/sql-server-downloads Express or Developer versions for more info

Remember the instance should have TCP enabled and with a listening port (default is 1433)

This can be set via the SQL Server 20XX Configuration Manager:
1. Open SQL Server Network Configuration
2. Double click on TCP/IP to open Properties
3. Check that Enabled and Listen All are Yes
4. And on IP Addresses tab, below at IPAll section, check that TCP Port is set to 1433 (default)
5. You may need to restart SQL's services (or reboot your system) for the changes to have effect
6. Depending on your configuration, you may have to check and allow communications on the ports used in Windows Firewall or whichever firewall software you may have
