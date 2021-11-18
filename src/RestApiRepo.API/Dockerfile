#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/RestApiRepo.Api/RestApiRepo.Api.csproj", "src/RestApiRepo.Api/"]
COPY ["src/RestApiRepo.Domain/RestApiRepo.Domain.csproj", "RestApiRepo.Domain/"]
COPY ["src/RestApiRepo.Infrastructure/RestApiRepo.Infrastructure.csproj", "RestApiRepo.Infrastructure/"]
RUN dotnet restore "src/RestApiRepo.Api/RestApiRepo.Api.csproj"
COPY . .
WORKDIR "/src/RestApiRepo.Api"
RUN dotnet build "RestApiRepo.Api.csproj"  -o /app/build

FROM build AS publish
RUN dotnet publish "RestApiRepo.Api.csproj"  -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestApiRepo.Api.dll"]