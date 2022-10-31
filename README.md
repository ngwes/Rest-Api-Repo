# Rest Api Repo
**Sample**  .NET Core 3.1 WebApi backend implementation for simple imaginary social network features. 
The **purpose** of this solution is to play with the features of the frameworks, trying to implement as many thing as possibile in as many way as possible.
The overall repository structure consists of the following projects:

- Envoy - API gateway providing a public facade for the underlying, internal services
- src - directory with the main service and the email service 
  - all the directories representing the above mentioned services, divided in an hexagonal (clean/ports and adapters) architecture
- tests - directory containing some unit and api tests 
- All the files for docker/docker-compose containerization and orchestration

For more information, don't hesitate to get in touch with me

# Requirements

- [.NET 3.1.](https://dotnet.microsoft.com/en-us/download/dotnet/3.1)
- [Docker](https://docs.docker.com/get-docker)
