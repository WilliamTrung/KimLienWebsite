# ==========================================
# RESTORE STAGE (copy only .csproj files)
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS restore
WORKDIR /src

# Copy solution file
COPY kim-lien-application.sln ./
COPY . .

COPY . ./tmp

# Create isolated folder for csproj-only restore
RUN mkdir /src/csproj && cp -R tmp/* /src/csproj

# Delete everything except .csproj inside the temporary restore folder
RUN find /src/csproj -type f ! -name '*.csproj' -delete

# Recreate folder structure in /src
RUN mkdir /src/projects && cp -R tmp/* /src/projects

# Restore using preserved structure
RUN dotnet restore /src/projects/Web.ApiHost/Web.ApiHost.csproj


# ==========================================
# BUILD STAGE (copy full source + publish)
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy full source now
COPY . .

# Copy restored artifacts to speed up build
COPY --from=restore /root/.nuget/packages /root/.nuget/packages

# Publish
RUN dotnet publish Web.ApiHost/Web.ApiHost.csproj -c Release -o /app/publish /p:UseAppHost=false


# ==========================================
# RUNTIME STAGE
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV BASE_PATH=/app
ENV APP_SETTING=appsettings.json
ENV ASPNETCORE_URLS=http://0.0.0.0:5000

EXPOSE 5000

# Copy published output
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Web.ApiHost.dll"]
