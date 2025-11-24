FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY *.sln .
COPY KimLienAdministrator/*.csproj KimLienAdministrator/
COPY AppCore/*.csproj AppCore/
COPY AppService/*.csproj AppService/
COPY AppRepository/*.csproj AppRepository/

RUN dotnet restore

COPY . .
WORKDIR /src/KimLienAdministrator
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "KimLienAdministrator.dll"]