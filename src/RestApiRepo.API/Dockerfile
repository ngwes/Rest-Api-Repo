#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/RestApiRepo.API/RestApiRepo.Api.csproj", "src/RestApiRepo.API/"]
COPY ["src/RestApiRepo.Domain/RestApiRepo.Domain.csproj", "src/RestApiRepo.Domain/"]
COPY ["src/RestApiRepo.Infrastructure/RestApiRepo.Infrastructure.csproj", "src/RestApiRepo.Infrastructure/"]
RUN dotnet restore "src/RestApiRepo.API/RestApiRepo.Api.csproj"
COPY . .
WORKDIR "/src/src/RestApiRepo.API"
RUN dotnet build "RestApiRepo.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestApiRepo.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestApiRepo.Api.dll"]
