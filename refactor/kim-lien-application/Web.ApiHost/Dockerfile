# =============================
#       BUILD IMAGE
# =============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY refactor/kim-lien-application/kim-lien-application.sln ./


# ------------------------------------------------------------
# COPY ALL .csproj FILES WHILE PRESERVING FOLDER STRUCTURE
# ------------------------------------------------------------

# Create a temp directory for structure
RUN mkdir /projectfiles

# Copy only folder structure and csproj files
# This preserves EXACT folder layout
COPY refactor/kim-lien-application /projectfiles

# Remove all non-.csproj files to keep caching efficient
RUN find /projectfiles -type f ! -name '*.csproj' -delete

# Copy preserved csproj structure into build WORKDIR
COPY /projectfiles/. ./


# ------------------------------------------------------------
# RESTORE ALL DEPENDENCIES
# ------------------------------------------------------------
RUN dotnet restore Web.ApiHost/Web.ApiHost.csproj


# ------------------------------------------------------------
# COPY FULL SOURCE AND BUILD
# ------------------------------------------------------------
COPY refactor/kim-lien-application/. .

RUN dotnet publish Web.ApiHost/Web.ApiHost.csproj -c Release -o /app/publish /p:UseAppHost=false


# =============================
#       RUNTIME IMAGE
# =============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .

HEALTHCHECK --interval=10s --timeout=3s --start-period=5s --retries=3 \
  CMD ["bash", "-c", "netstat -tln | grep 8080"]

ENTRYPOINT ["dotnet", "Web.ApiHost.dll"]
