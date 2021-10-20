#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Rest Api Repo/Rest Api Repo.csproj", "Rest Api Repo/"]
RUN dotnet restore "Rest Api Repo/Rest Api Repo.csproj"
COPY . .
WORKDIR "/src/Rest Api Repo"
RUN dotnet build "Rest Api Repo.csproj"  -o /app/build

FROM build AS publish
RUN dotnet publish "Rest Api Repo.csproj"  -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rest Api Repo.dll"]