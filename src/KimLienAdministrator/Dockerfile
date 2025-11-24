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

# Copy solution
COPY src/*.sln .

# Copy project files
COPY src/KimLienAdministrator/*.csproj KimLienAdministrator/
COPY src/KimLienCustomerView/*.csproj KimLienCustomerView/
COPY src/AppCore/*.csproj AppCore/
COPY src/AppService/*.csproj AppService/
COPY src/AppRepository/*.csproj AppRepository/

# Restore dependencies
RUN dotnet restore

# Copy everything
COPY src/ .

# Publish the Admin app
WORKDIR /src/KimLienAdministrator
RUN dotnet publish -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "KimLienAdministrator.dll"]
