# ==========================================
# RESTORE STAGE (copy only .csproj files)
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS restore
WORKDIR /src

# Copy solution file
COPY kim-lien-application.sln ./


# Copy all folders but only *.csproj files to keep caching efficient
COPY refactor/kim-lien-application ./tmp

# Delete everything except *.csproj
RUN find ./tmp -type f ! -name '*.csproj' -delete

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
COPY ../.. .

# Copy restored artifacts to speed up build
COPY --from=restore /root/.nuget/packages /root/.nuget/packages

# Publish
RUN dotnet publish Web.ApiHost/Web.ApiHost.csproj -c Release -o /app/publish /p:UseAppHost=false


# ==========================================
# RUNTIME STAGE
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

EXPOSE 8080

# Copy published output
COPY --from=build /app/publish .

# Health check without endpoint
HEALTHCHECK --interval=10s --timeout=3s --start-period=5s --retries=3 \
  CMD ["bash", "-c", "netstat -tln | grep 8080"]

ENTRYPOINT ["dotnet", "Web.ApiHost.dll"]
