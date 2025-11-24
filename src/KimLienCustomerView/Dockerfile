FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
       curl \
       iputils-ping \
       dnsutils \
       iproute2 \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy solution file
COPY src/*.sln .

# Copy csproj files for all required projects
COPY src/KimLienAdministrator/*.csproj KimLienAdministrator/
COPY src/KimLienCustomerView/*.csproj KimLienCustomerView/
COPY src/AppCore/*.csproj AppCore/
COPY src/AppService/*.csproj AppService/
COPY src/AppRepository/*.csproj AppRepository/

# Restore only what we copied
RUN dotnet restore

# Copy everything under src/
COPY src/ .

# Publish project
WORKDIR /src/KimLienCustomerView
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "KimLienCustomerView.dll"]