# =============================
#       BUILD IMAGE
# =============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY refactor/kim-lien-application/kim-lien-application.sln ./

# Copy all project files (.csproj) in subfolders
# This keeps Docker cache efficient for large solutions
COPY refactor/kim-lien-application/**/*.csproj ./restore/

# Recreate folder structure inside /src for restore
RUN for file in restore/*.csproj; do \
        projectPath=$(dirname "$file" | sed "s#restore/#/#"); \
        mkdir -p "./$projectPath"; \
        cp "$file" "./$projectPath/"; \
    done

# Restore dependencies (fast!)
RUN dotnet restore

# =============================
#    COPY FULL SOURCE + BUILD
# =============================
COPY refactor/kim-lien-application/ .

RUN dotnet publish Web.ApiHost/Web.ApiHost.csproj -c Release -o /app/publish /p:UseAppHost=false

# =============================
#       RUNTIME IMAGE
# =============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

EXPOSE 8080

# Copy published output
COPY --from=build /app/publish .

# Healthcheck without endpoint (checks Kestrel port)
HEALTHCHECK --interval=10s --timeout=3s --start-period=5s --retries=3 \
  CMD [ "bash", "-c", "netstat -tln | grep 8080" ]

# Actual entrypoint
ENTRYPOINT ["dotnet", "Web.ApiHost.dll"]
