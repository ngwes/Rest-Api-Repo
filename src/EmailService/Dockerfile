#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/EmailService/EmailService.Api.csproj", "src/EmailService/"]
COPY ["src/EmailService.Domain/EmailService.Domain.csproj", "src/EmailService.Domain/"]
COPY ["src/EmailService.Infrastructure/EmailService.Infrastructure.csproj", "src/EmailService.Infrastructure/"]
RUN dotnet restore "src/EmailService/EmailService.Api.csproj"
COPY . .
WORKDIR "/src/src/EmailService"
RUN dotnet build "EmailService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmailService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmailService.Api.dll"]